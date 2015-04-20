using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DTCDev.Lexical
{
    public static class LexicalAnalysis
    {
        /// <summary>
        /// Функция возвращает результат вычисления формулы по известным значениям параметров
        /// </summary>
        /// <param name="formula">Строковое представление формулы</param>
        /// <param name="values">Набор значений параметров (значения тригонометрических функций в радианах)</param>
        /// <returns></returns>
        public static string Analysator(string formula, string[] values)
        {
            string Result = string.Empty;

            RPN rpn = RPN.CreateRPN(formula);
            if (!rpn.SetVariables(values)) return rpn.Errors;

            rpn.Execute();
            return Result = rpn.Result.ToString();
        }

        public static RPN GetRPN(string formula)
        {
            return RPN.CreateRPN(formula);
        }

        public static bool IsIn<T>(this T source, params T[] values)
        {
            if (values == null) { return false; }
            return values.Contains(source);
        }

    }


}
