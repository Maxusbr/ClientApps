using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Convertors
{
    public class StringToBool
    {
        /// <summary>
        /// Convert string value to boolean
        /// </summary>
        /// <param name="vol"></param>
        /// <returns></returns>
        public bool BoolConverter(string vol)
        {
            if (vol == "1")
                return true;
            else return false;
        }

        /// <summary>
        /// Convert bool value to string
        /// </summary>

        public string BoolConverter(bool vol)
        {
            if (vol)
                return "1";
            else
                return "0";

        }
    }
}
