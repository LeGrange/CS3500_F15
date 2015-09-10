using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FormulaEvaluator
{
    /// <summary>
    /// This is the first class
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// This is a delegate that you can use
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public delegate int Lookup(String s);

        public static int Evaluate(String s, Lookup varEvaluator)
        {
            string[] substrings = Regex.Split(s, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            return 0;
        }

        
    }
}
