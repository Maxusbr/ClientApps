using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Convertors
{
    public class StringToInt
    {
        public int ConvertToInt(string vol)
        {
            int i = 0;
            Int32.TryParse(vol, out i);
            return i;
        }
    }
}
