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
        public delegate int Lookup(String s);

        

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
            string[] substrings = Regex.Split(s, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            //run through each string and do things to it.
            foreach(String t in substrings)
            {
                if (isVar(t))
                {

                }
                else if (isInt(t))
                {

                }
                else if (isOP(t))
                {

                }
            }
            //if (substrings[i] is number)
            //{
            //    //put on number stack
            //}
            //else if(substrings[i] is operator)
            //{
            //    //put on operator stack
            //}
            }catch(ArgumentException){

            }
            return 0;
        }

        private static bool isVar(String v)
        {
            return Regex.IsMatch(v, "(^[a-z]|[A-Z]) + \\d+$");
        }

        
    }

    public static class Extensions 
    {
 
    }
}
