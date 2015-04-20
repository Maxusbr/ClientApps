using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DTCDev.Lexical
{
    public class RPN
    {
        public Dictionary<string, double> Variables { get; set; }
        public Dictionary<string, FunctionBase> Functions { get; set; }
        public Stack<double> Stack { get; set; }
        public string Expression { get; set; }
        public List<Lexem> Ast { get; private set; }
        public double Result { get; private set; }
        public string Errors { get; private set; }
        public string CurentFormula { get; private set; }

        public double this[params Tuple<string, double>[] args]
        {
            get
            {
                RPN r = this.MemberwiseClone() as RPN;
                foreach (var arg in args) r.SetVariable(arg.Item1, arg.Item2);
                r.Execute();
                return r.Result;
            }
        }

        public RPN()
        {
            this.Variables = new Dictionary<string, double>();
            this.Functions = new Dictionary<string, FunctionBase>();
            AddUsesFunction();
            this.Stack = new Stack<double>();
        }

        private void AddUsesFunction()
        {
            AddFunction(NativeFunction.Sin);
            AddFunction(NativeFunction.Cos);
            /// Add more uses functions
        }


        private bool IsNumber(string s)
        {
            bool res = Regex.IsMatch(s, @"^(-?[\d]+)(\.\d+)?");//, RegexOptions.Compiled);
            return res;
        }
        private bool IsVariable(string s)
        {
            bool res = Regex.IsMatch(s, @"[$_a-zA-Z]+[$_a-zA-Z0-9]*");//, RegexOptions.Compiled);
            return res;
        }
        private bool IsOperation(string s)
        {
            return s.IsIn("+", "-", "*", "/", "^");
        }
        private bool IsDelimier(string s)
        {
            return s == ",";
        }
        private bool IsFunction(string s)
        {
            return HasFunction(s);
        }

        public void CreateAst()
        {
            string[] t = Expression.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Ast = new List<Lexem>();
            foreach (var l in t)
            {
                if (IsNumber(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Constant });
                else if (IsFunction(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Function });
                else if (IsVariable(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Variable });
                else if (IsOperation(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Operator });
                else if (IsDelimier(l)) Ast.Add(new Lexem { Value = l, Type = LexemType.Delimier });
                else throw new Exception("Unexpected lexem!");
            }
        }

        public bool SetVariables(string[] values)
        {
            string[] variables = this.Ast.Where(o => o.Type == LexemType.Variable).Select(s => s.Value).Distinct().ToArray();
            if (variables.Length != values.Length)
            {
                Errors = "The number of values not equal to the number of variables.";
                return false;
            }

            double[] val = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (!double.TryParse(values[i], out val[i]))
                {
                    Errors = string.Format("Error value: {0}", values[i]);
                    return false;
                }
                this.SetVariable(variables[i], val[i]);
            }
            return true;
        }

        public void SetVariable(string name, double value)
        {
            if (HasVariable(name.ToLower())) Variables[name.ToLower()] = value;
            else Variables.Add(name.ToLower(), value);
        }

        public void AddFunction(FunctionBase func)
        {
            Functions.Add(func.Name, func);
        }

        public bool HasVariable(string name)
        {
            return Variables.ContainsKey(name.ToLower());
        }

        public bool HasFunction(string name)
        {
            return Functions.ContainsKey(name.ToLower());
        }

        public void Execute()
        {
            foreach (var l in Ast)
            {
                switch (l.Type)
                {
                    case LexemType.Constant: Stack.Push(convertDouble(l.Value)); break;
                    case LexemType.Variable: Stack.Push(Variables[l.Value]); break;
                    case LexemType.Function:
                        {
                            var func = Functions[l.Value];
                            var args = new List<double>();
                            for (int i = 0; i < func.NeededArgs; ++i) args.Add(Stack.Pop());
                            Stack.Push(func.Execute(args.ToArray()));
                        } break;
                    case LexemType.Operator: Stack.Push(BasicFunction(l.Value)); break;
                    default: throw new Exception("Unexpected lexem!");
                }
            }
            Result = Stack.Pop();
        }

        private double convertDouble(string vol)
        {
            double answer = 0;
            Double.TryParse(vol, out answer);
            if (answer == 0)
                Double.TryParse(vol.Replace('.', ','), out answer);
            if (answer == 0)
                Double.TryParse(vol.Replace(',', '.'), out answer);
            return answer;
        }

        public double BasicFunction(string f)
        {
            switch (f)
            {
                case "+":
                    {
                        var t1 = Stack.Count > 0 ? Stack.Pop() : 0;
                        var t2 = Stack.Count > 0 ? Stack.Pop() : 0;
                        return t1 + t2;
                    }
                case "-":
                    {
                        var t1 = Stack.Count > 0 ? Stack.Pop() : 0;
                        var t2 = Stack.Count > 0 ? Stack.Pop() : 0;
                        return t2 - t1;
                    }
                case "*":
                    return Stack.Pop() * Stack.Pop();
                case "/":
                    {
                        var t1 = Stack.Count > 0 ? Stack.Pop() : 1;
                        var t2 = Stack.Count > 0 ? Stack.Pop() : 1;
                        return t2 / t1;
                    }
                case "^":
                    {
                        var t1 = Stack.Count > 0 ? Stack.Pop() : 1;
                        var t2 = Stack.Count > 0 ? Stack.Pop() : 1;
                        return Math.Pow(t2, t1);
                    }
                default: return double.NaN;
            }
        }

        public static RPN CreateRPN(string input)
        {
            RPN rpn = new RPN();
            rpn.Expression = rpn.StandartToRPN(input);
            rpn.CreateAst();
            return rpn;
        }

        private string StandartToRPN(string expr)
        {
            var ast = GetAst(expr);
            Stack<Lexem> operators = new Stack<Lexem>();
            List<string> result = new List<string>();
            foreach (var l in ast)
            {
                if (l.Type.IsIn(LexemType.Variable, LexemType.Constant))
                    result.Add(l.Value);
                else if (l.Type == LexemType.Operator && l.Value == ")")
                {
                    Lexem l0 = new Lexem();
                    while ((l0 = operators.Pop()).Value != "(")
                        if (l0.Value != ",")
                            result.Add(l0.Value);
                    if (operators.Count > 0 && operators.Peek().Type == LexemType.Function)
                        result.Add((l0 = operators.Pop()).Value);
                }
                else if (l.Type.IsIn(LexemType.Operator, LexemType.Function))
                {
                    if (operators.Count > 0 &&
                        GetPriority(operators.Peek()) >= GetPriority(l) &&
                        operators.Peek().Value != "(")
                    {
                        var l0 = operators.Pop();
                        if (l0.Value != ",") result.Add(l0.Value);
                    }
                    operators.Push(l);
                }
            }
            while (operators.Count > 0)
            {
                var l0 = operators.Pop();
                if (l0.Value != ",") result.Add(l0.Value);
            }
            string rpnExpr = string.Join(" ", result);
            return rpnExpr;
        }

        public int GetPriority(Lexem l)
        {
            if (l.Type == LexemType.Operator ||
                l.Type == LexemType.Function)
            {
                if (l.Value.IsIn("+", "-")) return 1;
                else if (l.Value.IsIn("*", "/")) return 2;
                else if (l.Value.IsIn("^")) return 3;
                else if (l.Value.IsIn("(")) return 5;
                else return 4;
            }
            return -1;
        }

        public List<Lexem> GetAst(string expr)
        {
            List<Lexem> lexems = new List<Lexem>();
            string acc = "";
            bool readingId = false;
            LexemType nextType = LexemType.Constant;
            for (int i = 0; i < expr.Length; ++i)
            {
                char c = expr[i];
                if (char.IsWhiteSpace(c))
                {
                    /*if (readingId)
                    {
                        lexems.Add(new Lexem { Value = acc, Type = nextType });
                        acc = "";
                    }
                    readingId = false;*/
                    continue;
                }
                else if (char.IsLetter(c))
                {
                    if (!readingId)
                    {
                        nextType = LexemType.Identifier;
                        readingId = true;
                    }
                    if (c == '.') throw new Exception("Unexpected char!");
                    acc += c;
                }
                else if (char.IsDigit(c))
                {
                    if (!readingId)
                    {
                        nextType = LexemType.Constant;
                        readingId = true;
                    }
                    acc += c;
                }
                else if (c == '.' && readingId && nextType == LexemType.Constant)
                    acc += '.';
                else if (char.IsLetterOrDigit(c)) acc += c;
                else if (c.IsIn('+', '-') && (i > 0 && expr[i - 1].IsIn('+', '-')) && char.IsDigit(expr[i + 1]) && !readingId)
                {
                    nextType = LexemType.Constant;
                    readingId = true;
                    acc += c;
                }
                else if (c.IsIn('(', ')', '+', '-', '*', '/', '^'))
                {
                    if (readingId)
                    {
                        readingId = false;
                        if (c == '(' && nextType == LexemType.Identifier) nextType = LexemType.Function;
                        else if (c != '(' && nextType == LexemType.Identifier) nextType = LexemType.Variable;
                        lexems.Add(new Lexem { Value = acc, Type = nextType });
                        acc = "";
                    }
                    lexems.Add(new Lexem { Value = c.ToString(), Type = LexemType.Operator });
                }
                else if (c == ',')
                {
                    if (readingId)
                    {
                        readingId = false;
                        if (c == '(' && nextType == LexemType.Identifier) nextType = LexemType.Function;
                        else if (c != '(' && nextType == LexemType.Identifier) nextType = LexemType.Variable;
                        lexems.Add(new Lexem { Value = acc, Type = nextType });
                        acc = "";
                    }
                    lexems.Add(new Lexem { Value = c.ToString(), Type = LexemType.Delimier });
                }
            }
            if (acc != "" && nextType == LexemType.Identifier)
            {
                nextType = LexemType.Variable;
                lexems.Add(new Lexem { Value = acc, Type = nextType });
            }
            else lexems.Add(new Lexem { Value = acc, Type = nextType });
            return lexems;
        }
    }

}
