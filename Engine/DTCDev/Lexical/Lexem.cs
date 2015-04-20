using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Lexical
{
    public class Lexem
    {
        public LexemType Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Type, Value);
        }
    }

}
