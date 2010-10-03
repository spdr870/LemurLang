using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LemurLang.Expression;
using LemurLang.Test.Tools;
using System.Globalization;

namespace LemurLang.Test
{
    [TestClass]
    public class ReversePolishNotationTests
    {
        #region Basic math test
        [TestMethod]
        public void PowTest()
        {
            Assert.AreEqual(4m, new ReversePolishNotation("2^2").Evaluate(x => x));
        }

        [TestMethod]
        public void MultiplyTest()
        {
            Assert.AreEqual(4m, new ReversePolishNotation("2*2").Evaluate(x => x));
        }

        [TestMethod]
        public void UnaryMinusTest()
        {
            Assert.AreEqual(-4m, new ReversePolishNotation("-4").Evaluate(x => x));
        }

        [TestMethod]
        public void DoubleUnaryMinusTest()
        {
            Assert.AreEqual(4m, new ReversePolishNotation("--4").Evaluate(x => x));
        }

        [TestMethod]
        public void MinusTest()
        {
            Assert.AreEqual(4m, new ReversePolishNotation("8-4").Evaluate(x => x));
        }

        [TestMethod]
        public void PlusTest()
        {
            Assert.AreEqual(4m, new ReversePolishNotation("2+2").Evaluate(x => x));
        }

        [TestMethod]
        public void DevideTest()
        {
            Assert.AreEqual(4m, new ReversePolishNotation("8/2").Evaluate(x => x));
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void DevideByZeroTest()
        {
            new ReversePolishNotation("8/0").Evaluate(x => x);
        }
        #endregion

        #region Basic logical operators

        [TestMethod]
        public void EqualsTest()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                Assert.AreEqual(true, new ReversePolishNotation("true == true").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("true != false").Evaluate(x => x));
            }
        }

        [TestMethod]
        public void StringEqualsTest()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                Assert.AreEqual(true, new ReversePolishNotation("Test == Test").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("string1 != string2").Evaluate(x => x));
            }
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NonNummericGreaterThanTest()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                Assert.AreEqual(true, new ReversePolishNotation("true > true").Evaluate(x => x));
            }
        }

        [TestMethod]
        public void LogicalAndOrTest()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                Assert.AreEqual(true, new ReversePolishNotation("true && true").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("true || false").Evaluate(x => x));
            }
        }

        [TestMethod]
        public void NotTest()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                Assert.AreEqual(true, new ReversePolishNotation("!false").Evaluate(x => x));
                Assert.AreEqual(false, new ReversePolishNotation("!true").Evaluate(x => x));

                Assert.AreEqual(false, new ReversePolishNotation("!(1 + 1 == 2)").Evaluate(x => x));
            }
        }

        #endregion

        #region Complex math
        [TestMethod]
        public void ReversePolishNotationTest()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                Assert.AreEqual(5m, new ReversePolishNotation("1 + 4").Evaluate(x => x));
                Assert.AreEqual(9m, new ReversePolishNotation("1 + 4 * 2").Evaluate(x => x));
                Assert.AreEqual(10m, new ReversePolishNotation("(1 + 4) * 2").Evaluate(x => x));

                Assert.AreEqual(8m, new ReversePolishNotation("4 * 1 + 4").Evaluate(x => x));
                Assert.AreEqual(10m, new ReversePolishNotation("1 + 4 * 2 + 1").Evaluate(x => x));
                Assert.AreEqual(5m, new ReversePolishNotation("(1 + 4) * 2 / 2").Evaluate(x => x));

                Assert.AreEqual(3m, new ReversePolishNotation("4 + -1").Evaluate(x => x));
                Assert.AreEqual(-3m, new ReversePolishNotation("-4 + 1").Evaluate(x => x));

                Assert.AreEqual(-4m, new ReversePolishNotation("-4").Evaluate(x => x));

                Assert.AreEqual(4.4m, new ReversePolishNotation("1.1 * 4").Evaluate(x => x));
            }
        }

        [TestMethod]
        public void MathExpressionTest()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                Assert.AreEqual(true, new ReversePolishNotation("5 == 5").Evaluate(x => x));
                Assert.AreEqual(false, new ReversePolishNotation("5 != 5").Evaluate(x => x));
                Assert.AreEqual(false, new ReversePolishNotation("5 == 6").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("6 != 5").Evaluate(x => x));

                Assert.AreEqual(true, new ReversePolishNotation("5 < 6").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("5 <= 6").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("5 <= 5").Evaluate(x => x));

                Assert.AreEqual(true, new ReversePolishNotation("5 >= 5").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("6 >= 5").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("6 > 5").Evaluate(x => x)); 
                
                Assert.AreEqual(true, new ReversePolishNotation("5 * 5 == 5 + 5 + 5 + 5 + 5").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("5 * 5 * 2 == 5 * 5 + 5 * 5").Evaluate(x => x));
                Assert.AreEqual(false, new ReversePolishNotation("5 * 5 * 2 > 5 * 5 + 5 * 5").Evaluate(x => x));

                //Test without spaces
                Assert.AreEqual(false, new ReversePolishNotation("5*5*2>5*5+5*5").Evaluate(x => x));
            }
        }

        #endregion

        #region Integration tests

        [TestMethod]
        public void CompleteTest()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                Assert.AreEqual(true, new ReversePolishNotation("true && (1 + 1 == 2)").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("false || (1 + 1 == 2)").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("true || (1 * 1 == 2)").Evaluate(x => x));
                Assert.AreEqual(true, new ReversePolishNotation("(6 > 4) && (8 < 9)").Evaluate(x => x));
            }
        }

        private class Customer
        {
            public string Name { get; set; }
            public Customer Friend { get; set; }
            public int Age { get; set; }
        }

        [TestMethod]
        public void ContextTest()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"customer", new Customer(){ Age=12, Name="Customer1"}}
            }, null);

            Assert.AreEqual(true, new ReversePolishNotation("${customer.Age} == 12").Evaluate(context.GetValue));
            Assert.AreEqual(true, new ReversePolishNotation("${customer.Age} + 8 == 20").Evaluate(context.GetValue));
            Assert.AreEqual(true, new ReversePolishNotation("${customer.Age} - 2 == 10").Evaluate(context.GetValue));
            Assert.AreEqual(true, new ReversePolishNotation("-${customer.Age} == -12").Evaluate(context.GetValue));
        }

        #endregion
    }
}
