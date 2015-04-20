using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Lexical
{
    public abstract class FunctionBase
    {
        public virtual string Name { get; set; }
        public virtual int NeededArgs { get; set; }

        public virtual double Execute(params double[] args)
        {
            return double.NaN;
        }
    }

}
