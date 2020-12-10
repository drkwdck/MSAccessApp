﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using MSAccessApp.Modules;

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
        public (IEnumerable<DataRow> rows, List<string> columns) GetRowsFromTable(string tableName, Func<IEnumerable<DataRow>, IEnumerable<DataRow>>? filterPredicat = null)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    lock (_syncRoot)
                    {
                        var stringQuery = $"SELECT * FROM [{tableName}]";
                        var adapter = new OleDbDataAdapter(stringQuery, connection);
                        var dataSet = new DataSet();

                        adapter.Fill(dataSet);

                        if (dataSet.Tables.Count == 0) { return (new List<DataRow>(), new List<string>()); }

                        var orderedColumns = new List<string>();
                        foreach(var column in dataSet.Tables[0].Columns)
                        {
                            orderedColumns.Add(column.ToString());
                        }

                        return (dataSet.Tables[0].AsEnumerable(), orderedColumns);  
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

            return (new List<DataRow>(), new List<string>());
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
        public bool AddRowToTable(string tableName, string[] values)
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
                        cmd.CommandText = $"INSERT INTO [{tableName}] VALUES({string.Join(", ", values)})";
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка во время добавления записи в таблицу {tableName}: {e.Message}");
                    return false;
                }
                finally
                {
                    connection?.Close();
                }
            }

            return true;
        }

        public bool RemoveRowFromTable(string tableName, string id)
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

                        var stringQuery = $"SELECT * FROM [{tableName}] WHERE [{keyColumn}]={id}";
                        var adapter = new OleDbDataAdapter(stringQuery, connection);
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables[0].Rows.Count == 0) { return false; }

                        stringQuery = $"DELETE FROM [{tableName}]  WHERE [{keyColumn}]={id}";
                        cmd.CommandText = stringQuery;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка во время удаления записи из таблицы {tableName}: {e.Message}");
                    return false;
                }
                finally
                {
                    connection?.Close();
                }
            }

            return true;
        }

        public bool UpdateRowFromTable(string tableName, string[] values)
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

                        // Выбираем из аргументов ключевое поле
                        var schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                        new object[] { null, null, tableName });
                        var keyColumn = schemaTable.Rows[0][3];

                        schemaTable = connection.GetOleDbSchemaTable(
                          OleDbSchemaGuid.Columns,
                          new Object[] { null, null, tableName });
                        var columnOrdinalForName = schemaTable.Columns["COLUMN_NAME"].Ordinal;
                        var columnOrdinalForType = schemaTable.Columns["DATA_TYPE"].Ordinal;
                        var columns = new string[schemaTable.Rows.Count];
                        var typeOfColumns = new Dictionary<string, OleDbType>();

                        for (var i = 0; i < schemaTable.Rows.Count; ++i)
                        {
                            var dataRow = schemaTable.Rows[i] as DataRow;
                            columns[i] = dataRow.ItemArray[columnOrdinalForName].ToString();
                            typeOfColumns[columns[i]] = (OleDbType)schemaTable.Rows[i].ItemArray[columnOrdinalForType];
                        }

                        // Значения приходят к нам в определнном порядке, т.к. колонки отсротированы
                        Array.Sort(columns);
                        var keyIndex = Array.IndexOf(columns, keyColumn);

                        // Проверяем, есть ли такая запись
                        var stringQuery = $"SELECT * FROM {tableName} WHERE [{keyColumn}]={values[keyIndex]}";
                        var adapter = new OleDbDataAdapter(stringQuery, connection);
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        if (dataSet.Tables[0].Rows.Count == 0) { return false; }

                        // формируем кусок запроса на обновление, который идет после SET
                        var setString = "";

                        for (var i = 0; i < values.Length; ++i)
                        {
                            if (string.IsNullOrEmpty(values[i])) { continue; }
                            if (i == keyIndex) { continue; }

                            setString += $"[{columns[i]}]="
                                + (Parser.OleDbTypeToNetType(typeOfColumns[columns[i]]) == typeof(string)
                                    ? $"'{values[i]}', "
                                    : $"{values[i]}, ");
                        }

                        if (string.IsNullOrEmpty(setString))
                        {
                            return false;
                        }
                        else
                        {
                            setString = setString.Substring(0, setString.Length - 2);
                        }

                        stringQuery = $"UPDATE {tableName} SET {setString} WHERE [{keyColumn}]={values[keyIndex]}";
                        cmd.CommandText = stringQuery;
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка во время удаления записи из таблицы {tableName}: {e.Message}");
                    return false;
                }
                finally
                {
                    connection?.Close();
                }
            }

            return true;
        }

        #endregion
    }
}
