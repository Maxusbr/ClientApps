using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCDev.Lexical
{
    public class RPNFunction : FunctionBase
    {
        public override string Name { get; set; }
        public RPN Function { get; set; }
        public override int NeededArgs { get; set; }

        public void UpdateExpression(string newExpr, int newNeededArgs)
        {
            this.NeededArgs = newNeededArgs;
            this.Function.Expression = newExpr;
        }

        public RPNFunction(string name, RPN function, int neededArgs)
        {
            this.Name = name;
            this.Function = function;
            this.NeededArgs = neededArgs;
        }

        public override double Execute(params double[] args)
        {
            if (this.Function == null) return double.NaN;
            if (args.Length != this.NeededArgs) return double.NaN;
            for (int i = 0; i < args.Length; ++i)
                this.Function.SetVariable(string.Format("${0}", i + 1), args[i]);
            this.Function.Execute();
            return this.Function.Result;
        }

        public override string ToString()
        {
            return this.Function.Expression;
        }
    }

}
