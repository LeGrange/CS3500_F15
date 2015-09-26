using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace PS3Test
{
    /// <summary>
    /// This is a test class that will test the Formula Class.
    /// </summary>
    [TestClass]
    public class FormulaTest
    {
        /// <summary>
        /// This is a test to test the constructor
        /// </summary>
        [TestMethod]
        public void ConstructorTest1()
        {
            Formula f1 = new Formula("2.0*3.0");
            Assert.AreEqual("2.0*3.0", f1.ToString());
        }
        /// <summary>
        /// This will test if we can use variables
        /// </summary>
        [TestMethod]
        public void ConstructorTest2()
        {
            Formula f1 = new Formula("x2*y3");
            Assert.AreEqual("x2*y3", f1.ToString());
        }
        /// <summary>
        /// This will test if we can use parentheses;
        /// </summary>
        [TestMethod]
        public void ConstructorTest3()
        {
            Formula f1 = new Formula("x2*(y3)");
            Assert.AreEqual("x2*(y3)", f1.ToString());
        }
        
    }
}
