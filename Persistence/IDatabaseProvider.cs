using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

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
        /// Получить строки из таблицы и колонки, соответсвующие им.
        /// Если фильтр не задан, возвращает все записи
        /// </summary>
        (IEnumerable<DataRow> rows, List<string> columns) GetRowsFromTable(string tableName, Func<IEnumerable<DataRow>, IEnumerable<DataRow>>? filterPredicat = null);

        /// <summary>
        /// Получить список названий колонок таблицы и тип данных в них
        /// </summary>
        Dictionary<string, OleDbType> GetTableColumnsWithTypes(string tableName);

        /// <summary>
        /// Добавить запись в таблицу
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values">значения полей записи (порядок должен соответсвовать столбцам)</param>
        bool AddRowToTable(string tableName, string[] values);

        /// <summary>
        /// Удалить запись из таблицы по первичному ключу
        /// </summary>
        bool RemoveRowFromTable(string tableName, string id);

        /// <summary>
        /// Обновляет поля записи
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="values">Массив новых значений. Содержит ключ, для нахождения записи.
        /// Если значение - пустая строка, то оно не обновится</param>
        bool UpdateRowFromTable(string tableName, string[] values);
    }
}
