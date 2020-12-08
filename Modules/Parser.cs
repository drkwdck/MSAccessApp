using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSAccessApp.Modules
{
    class Parser
    {
        public static string GetValueFromInput(string inputString)
        {
            var splitedValues = inputString.Split(':');

            if (splitedValues.Length > 1)
            {
                return string.Join("", splitedValues.Skip(1));
            }

            return inputString;
        }
    }
}
