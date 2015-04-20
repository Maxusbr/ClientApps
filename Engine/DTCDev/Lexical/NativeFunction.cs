using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Lexical
{
    public class NativeFunction : FunctionBase
    {
        public static NativeFunction Sin
        {
            get
            {
                return new NativeFunction("sin", a => Math.Sin(a[0]), 1);
            }
        }
        public static NativeFunction Cos
        {
            get
            {
                return new NativeFunction("cos", a => Math.Cos(a[0]), 1);
            }
        }
        public static NativeFunction Sqrt
        {
            get
            {
                return new NativeFunction("sqrt", a => Math.Sqrt(a[0]), 1);
            }
        }
        public static NativeFunction Exp
        {
            get
            {
                return new NativeFunction("exp", a => Math.Exp(a[0]), 1);
            }
        }
        public static NativeFunction Abs
        {
            get
            {
                return new NativeFunction("abs", a => Math.Abs(a[0]), 1);
            }
        }
        public static NativeFunction Ln
        {
            get
            {
                return new NativeFunction("ln", a => Math.Log10(a[0]), 1);
            }
        }
        public static NativeFunction Lb
        {
            get
            {
                return new NativeFunction("lb", a => Math.Log(a[0], 2.0), 1);
            }
        }
        public static NativeFunction Log
        {
            get
            {
                return new NativeFunction("log", a => Math.Log(a[0], a[1]), 2);
            }
        }
        public static NativeFunction Pow
        {
            get
            {
                return new NativeFunction("pow", a => Math.Pow(a[0], a[1]), 2);
            }
        }
        public static NativeFunction Atan
        {
            get
            {
                return new NativeFunction("atan", a => Math.Atan(a[0]), 1);
            }
        }

        public override string Name { get; set; }
        public Func<double[], double> Function { get; set; }
        public override int NeededArgs { get; set; }

        public NativeFunction(string name, Func<double[], double> function, int neededArgs)
        {
            this.Name = name;
            this.Function = function;
            this.NeededArgs = neededArgs;
        }

        public override double Execute(params double[] args)
        {
            if (Function != null) return Function(args);
            return double.NaN;
        }
    }

}
