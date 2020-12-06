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
        /// <inheritdoc />
        public IEnumerable<DataRow> GetRowsFromTable(string tableName, Func<IEnumerable<DataRow>, IEnumerable<DataRow>>? filterPredicat = null)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["Database"];

            using (var connection = new OleDbConnection(connectionsString.ConnectionString))
            {
                try
                {
                    var stringQuery = $"SELECT * FROM {tableName}";
                    var adapter = new OleDbDataAdapter(stringQuery, connection);
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    if (dataSet.Tables.Count == 0) { return new List<DataRow>(); }

                    return dataSet.Tables[0].AsEnumerable();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка во время получения строк таблицы {tableName}: {e.Message}");
                }
            }

            return new List<DataRow>();
        }

        public List<string> GetColumnsFromTable(string tableName)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            using (var connection = new OleDbConnection(connectionsString))
            {
                try
                {
                    connection.Open();
                    var schemaTable = connection.GetOleDbSchemaTable(
                      OleDbSchemaGuid.Columns,
                      new Object[] { null, null, tableName });

                    if (schemaTable == null)
                    {
                        return new List<string>();
                    }

                    var columnOrdinalForName = schemaTable.Columns["COLUMN_NAME"].Ordinal;

                    return (from DataRow row in schemaTable.Rows select row.ItemArray[columnOrdinalForName]?.ToString()).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка получения списка столбцов таблицы {tableName}: {e.Message}");
                }
            }

            return new List<string>();
        }
    }
}
