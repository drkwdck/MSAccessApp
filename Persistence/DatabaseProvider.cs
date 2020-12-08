using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace MSAccessApp.Persistence
{
    class DatabaseProvider : IDatabaseProvider
    {
        #region Fields

        private static readonly DatabaseProvider _instance = new DatabaseProvider();
        private static readonly object _syncRoot = new Object();
        #endregion

        #region .ctor

        private DatabaseProvider() {}

        public static DatabaseProvider Get()
        {
            return _instance;
        }

        #endregion

        #region Implementation of IDatabaseProvider

        /// <inheritdoc />
        public List<string> GetTables()
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    lock (_syncRoot)
                    {
                        // Отсеиваем системеные таблицы
                        string[] restrictions = new string[4];
                        restrictions[3] = "Table";
                        connection.Open();
                        var userTables = connection.GetSchema("Tables", restrictions);
                        List<string> tableNames = new List<string>();
                        
                        foreach (DataRow row in userTables.Rows)
                        {
                            tableNames.Add(row[2]?.ToString());
                        }

                        return tableNames;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка во время получения списка таблиц: {e.Message}");
                }
                finally
                {
                    connection?.Close();
                }
            }

            return new List<string>();
        }

        /// <inheritdoc />
        public IEnumerable<DataRow> GetRowsFromTable(string tableName, Func<IEnumerable<DataRow>, IEnumerable<DataRow>>? filterPredicat = null)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    lock (_syncRoot)
                    {
                        var stringQuery = $"SELECT * FROM {tableName}";
                        var adapter = new OleDbDataAdapter(stringQuery, connection);
                        var dataSet = new DataSet();

                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count == 0) { return new List<DataRow>(); }

                        return dataSet.Tables[0].AsEnumerable();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка во время получения строк таблицы {tableName}: {e.Message}");
                }
                finally
                {
                    connection?.Close();
                }
            }

            return new List<DataRow>();
        }

        /// <inheritdoc />
        public Dictionary<string, OleDbType> GetTableColumnsWithTypes(string tableName)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            var result = new Dictionary<string, OleDbType>();

            using (var connection = new OleDbConnection(connectionsString))
            {
                try
                {
                    lock (_syncRoot)
                    {
                        connection.Open();
                        var schemaTable = connection.GetOleDbSchemaTable(
                          OleDbSchemaGuid.Columns,
                          new Object[] { null, null, tableName });

                        if (schemaTable == null)
                        {
                            return result;
                        }

                        var columnOrdinalForName = schemaTable.Columns["COLUMN_NAME"].Ordinal;
                        var columnOrdinalForType = schemaTable.Columns["DATA_TYPE"].Ordinal;

                        var columns = (from DataRow row in schemaTable.Rows select row.ItemArray[columnOrdinalForName]?.ToString()).ToList();

                        foreach(var row in schemaTable.Rows)
                        {
                            var dataRow = row as DataRow;

                            if (dataRow == null) { continue; }

                            result[dataRow.ItemArray[columnOrdinalForName].ToString()] = dataRow.ItemArray[columnOrdinalForType] as OleDbType? ?? OleDbType.IUnknown;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка получения списка столбцов таблицы {tableName}: {e.Message}");
                }
                finally
                {
                    connection?.Close();
                }
            }

            return result;
        }


        /// <inheritdoc />
        public void AddRowToTable(string tableName, string[] values)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (var connection = new OleDbConnection(connectionsString))
            {
                try
                {
                    lock (_syncRoot)
                    {
                        connection.Open();
                        var cmd = new OleDbCommand();
                        cmd.Connection = connection;
                        cmd.CommandText = $"INSERT INTO {tableName} VALUES({string.Join(", ", values)})";
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка во время добавления записи в таблицу {tableName}: {e.Message}");
                }
                finally
                {
                    connection?.Close();
                }
            }
        }

        public void RemoveRowFromTable(string tableName, string id)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (var connection = new OleDbConnection(connectionsString))
            {
                try
                {
                    lock (_syncRoot)
                    {
                        connection.Open();
                        var cmd = new OleDbCommand();
                        cmd.Connection = connection;

                        DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                        new object[] { null, null, tableName });

                        var keyColumn = schemaTable.Rows[0][3];

                        var stringQuery = $"DELETE FROM {tableName}  WHERE [{keyColumn}]={id}";
                        cmd.CommandText = stringQuery;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка во время удаления записи из таблицы {tableName}: {e.Message}");
                }
                finally
                {
                    connection?.Close();
                }
            }
        }

        #endregion
    }
}
