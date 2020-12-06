using System;
using System.Collections.Generic;
using System.Data;

namespace MSAccessApp.Persistence
{
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Получить строки из таблицы.
        /// Если фильтр не задан, возвращает все записи
        /// </summary>
        IEnumerable<DataRow> GetRowsFromTable(string tableName, Func<IEnumerable<DataRow>, IEnumerable<DataRow>>? filterPredicat = null);

        List<string> GetColumnsFromTable(string tableName);
    }
}
