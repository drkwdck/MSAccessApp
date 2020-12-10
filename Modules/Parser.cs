using System;
using System.Data.OleDb;
using System.Linq;

namespace MSAccessApp.Modules
{
    class Parser
    {
        /// <summary>
        /// Парсит значение из инпута (обрезает название колонки, которое может приходить из инпута,
        /// парсит под определенный тип)
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetValueFromInput(string inputString, OleDbType? type)
        {
            var splitedValues = inputString.Split(':');
            var cuttedString = "";

            if (splitedValues.Length > 1)
            {
                cuttedString = string.Join("", splitedValues.Skip(1));
            }
            else
            {
                cuttedString = inputString;
            }

            // TODO переложить на плечи .NET
            switch (type)
            {
                case OleDbType.BigInt:
                    if (long.TryParse(cuttedString, out var number)) { return cuttedString; }
                    return "";
                case OleDbType.Guid:
                    if (uint.TryParse(cuttedString, out var number1)) { return cuttedString; }
                    return "";
                case OleDbType.Integer:
                    if (int.TryParse(cuttedString, out var number2)) { return cuttedString; }
                    return "";
                case OleDbType.VarChar:
                case OleDbType.VarWChar:
                case OleDbType.BSTR:
                case OleDbType.Char:
                case OleDbType.LongVarChar:
                case OleDbType.LongVarWChar:
                    return cuttedString;
                case OleDbType.Date:
                case OleDbType.DBDate:
                     if (DateTime.TryParse(cuttedString, out var dateTime)) { return cuttedString; }
                    return "";
                default:
                    break;
            }

            return cuttedString;
        }

        public static Type OleDbTypeToNetType(OleDbType oleDbType)
        {
            switch (oleDbType)
            {
                case OleDbType.BigInt:
                case OleDbType.Guid:
                case OleDbType.Integer:
                    return typeof(int);
                case OleDbType.VarChar:
                case OleDbType.VarWChar:
                case OleDbType.BSTR:
                case OleDbType.Char:
                case OleDbType.LongVarChar:
                case OleDbType.LongVarWChar:
                    return typeof(string);
                case OleDbType.Date:
                case OleDbType.DBDate:
                    return typeof(DateTime);
                default:
                    return typeof(Object);
            }
        }
    }
}
