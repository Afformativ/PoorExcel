using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoorExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoorExcel.Tests
{
    [TestClass()]
    public class Unit_testsTests
    {
        [TestMethod()]
        public void OrderedOperation()
        {
            var expr = "1+6-2*3/2";
            // 2 * 3 = 6
            // 6 / 2 = 3
            // 1 + 6 = 7
            // 7 - 3 = 4
            Assert.AreEqual(4, Calculator.Evaluate(expr));
        }
        [TestMethod]
        public void Decimal_Division()
        {
            var expr = "5/10";
            Assert.AreEqual(.5, Calculator.Evaluate(expr));
        }
        [TestMethod]
        public void mmax()
        {
            var expr = "mmax(1,5,7,12,0,-5)";
            Assert.AreEqual(12, Calculator.Evaluate(expr));
        }
        [TestMethod]
        public void mmin()
        {
            var expr = "mmin(1,5,7,12,0,-5)";
            Assert.AreEqual(-5, Calculator.Evaluate(expr));
        }
        [TestMethod]
        public void not()
        {
            var expr = "not(5)";
            Assert.AreEqual(0, Calculator.Evaluate(expr));
        }
        [TestMethod]
        public void Higher()
        {
            var expr = "2566>444";
            Assert.AreEqual(1, Calculator.Evaluate(expr));
        }
        [TestMethod]
        public void Parens_EvaluatedBeforeDivision()
        {
            var expr = "(5+5)/(2+3)";
            // 10/5
            Assert.AreEqual(2, Calculator.Evaluate(expr));
        }
    }
}