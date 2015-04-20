using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Convertors
{
    public class StringToDouble
    {
        /// <summary>
        /// convert string to decimal
        /// </summary>
        /// <param name="vol"></param>
        /// <returns></returns>
        public double DoubleConvertor(string vol)
        {
            double answer = 0;

            Double.TryParse(vol, out answer);

            if (answer == 0)
                Double.TryParse(vol.Replace('.', ','), out answer);
            if (answer == 0)
                Double.TryParse(vol.Replace(',', '.'), out answer);

            return answer;
        }
    }
}
