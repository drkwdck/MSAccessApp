using System;
using System.Collections.Generic;
using System.Data;

namespace MSAccessApp.Persistence
{
    /// <summary>
    /// Потокобезопасный провайдер для работы с БД
    /// </summary>
    public interface IDatabaseProvider
    {
        /// <summary>
        /// Получить список таблиц из БД
        /// </summary>
        /// <returns></returns>
        List<string> GetTables();

        /// <summary>
        /// Получить строки из таблицы.
        /// Если фильтр не задан, возвращает все записи
        /// </summary>
        IEnumerable<DataRow> GetRowsFromTable(string tableName, Func<IEnumerable<DataRow>, IEnumerable<DataRow>>? filterPredicat = null);

        /// <summary>
        /// Получить список названий колонок таблицы
        /// </summary>
        List<string> GetColumnsFromTable(string tableName);

        /// <summary>
        /// Добавить запись в таблицу
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values">значения полей записи (порядок должен соответсвовать столбцам)</param>
        void AddRowToTable(string tableName, string[] values);

        /// <summary>
        /// Удалить запись из таблицы по первичному ключу
        /// </summary>
        void RemoveRowFromTable(string tableName, string id);
    }
}
