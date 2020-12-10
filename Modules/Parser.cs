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
        public static string GetValueFromInput(string inputString, Type? type)
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

            if (type == typeof(string))
            {
                return "'" + cuttedString + "'";
            }

            if (type == typeof(DateTime))
            {
                return "#" + cuttedString + "#";
            }

            return cuttedString;
        }
    }
}
