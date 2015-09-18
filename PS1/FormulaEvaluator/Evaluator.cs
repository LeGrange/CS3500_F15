using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    /// <summary>
    /// This is the static Evaluator Class. Containing 
    /// the necessary methods to complete PS1
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// Lookup delegate holds the value that will be 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public delegate double Lookup(String s);

        

        /// <summary>
        /// This is the Evaluate Method
        /// </summary>
        /// <param name="s"></param>
        /// <param name="varEvaluator"></param>
        /// <returns></returns>
        public static int Evaluate(String s, Lookup varEvaluator)
        {
            
            Stack<string> operatorStack = new Stack<string>();

            Stack<double> numberStack = new Stack<double>();
            try{
            //This helps to split strings to the proper operator.
            string[] substrings = Regex.Split(s, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)", RegexOptions.IgnorePatternWhitespace);

            //run through each string and do things to it.
            //Lets populate the stacks here.
            foreach(String t in substrings)
            {
                if (isNum(t))
                {
                    double newNum = Double.Parse(t);
                    string newOp = operatorStack.Peek();
                    if (newOp.Equals("*") || newOp.Equals("/"))
                    {
                        numberStack.Pop();
                        operatorStack.Pop();
                        
                    }
                }
                else if (isVar(t))
                {

                }
                else if (isOp(t))
                {
                    if (operatorStack.Peek().Equals("+") || operatorStack.Peek().Equals("-"))
                    {
                        double x = numberStack.Pop();
                        double y = numberStack.Pop();
                        string poppedOp = operatorStack.Pop();
                        if(poppedOp.Equals("+"))
                        {
                            x = x + y;
                        } else
                        {
                            x = x - y;
                        }

                    }
                }
            }
            
            }catch(ArgumentException){

            }
            return 0;
        }

        private static bool isVar(String v)
        {
            return Regex.IsMatch(v, "(^[a-z]|[A-Z]) + \\d+$");
        }

        private static bool isNum(String v)
        {
            return Regex.IsMatch(v, "^[0-9]$");
        }

        private static bool isOp(String v)
        {
            return Regex.IsMatch(v, "\\(|\\)|-|\\+|\\*|/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number1">left operand</param>
        /// <param name="number2">right operand</param>
        /// <param name="opSign">operator sign</param>
        /// <returns>the result of the operation</returns>
        private static double Math(double number1, double number2, string opSign)
        {
            double result = 0.0;
            switch (opSign)
            {
                case "*":
                    result = number1 * number2;
                    break;
                case "/":
                    if (number2 == 0)
                    {
                        throw new ArgumentException("ERROR: Can't divide by zero!");
                    }
                    result = number1 / number2;
                    break;
                case "+":
                    result = number1 + number2;
                    break;
                case "-":
                    result = number1 - number2;
                    break;
            }
            return result;
        }

        
        
    }

    public static class Extensions 
    {
 
    }
}
