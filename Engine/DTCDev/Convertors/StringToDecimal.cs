using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Convertors
{
    public class StringToDecimal
    {
        /// <summary>
        /// convert string to decimal
        /// </summary>
        /// <param name="vol"></param>
        /// <returns></returns>
        public decimal DecimalConvertor(string vol)
        {
            decimal answer = 0;

            Decimal.TryParse(vol, out answer);

            if (answer == 0)
                Decimal.TryParse(vol.Replace('.', ','), out answer);
            if (answer == 0)
                Decimal.TryParse(vol.Replace(',', '.'), out answer);

            return answer;
        }
    }
}
