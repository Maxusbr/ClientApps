using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Convertors
{
    public class ByteToString
    {
        public string MakeStringFromByte(byte a)
        {
            int vol = a;
            StringBuilder sb = new StringBuilder();
            int rez = a / 128;
            sb.Append(rez.ToString());
            sb.Append(";");
            if (rez == 1)
                a -= 128;
            rez = a / 64;
            sb.Append(rez.ToString());
            sb.Append(";");
            if (rez == 1)
                a -= 64;
            rez = a / 32;
            sb.Append(rez.ToString());
            sb.Append(";");
            if (rez == 1)
                a -= 32;
            rez = a / 16;
            sb.Append(rez.ToString());
            sb.Append(";");
            if (rez == 1)
                a -= 16;
            rez = a / 8;
            sb.Append(rez.ToString());
            sb.Append(";");
            if (rez == 1)
                a -= 8;
            rez = a / 4;
            sb.Append(rez.ToString());
            sb.Append(";");
            if (rez == 1)
                a -= 4;
            rez = a / 2;
            sb.Append(rez.ToString());
            sb.Append(";");
            if (rez == 1)
                a -= 2;
            sb.Append(a.ToString());
            return sb.ToString();
        }
    }
}
