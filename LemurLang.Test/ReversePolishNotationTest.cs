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
        public void NonNummericGreaterThanTest()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                Assert.AreEqual(false, new ReversePolishNotation("a > a").Evaluate(x => x));
                Assert.AreEqual(false, new ReversePolishNotation("a > b").Evaluate(x => x));
                Assert.AreEqual(false, new ReversePolishNotation("(a > b) && (b < c)").Evaluate(x => x));
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
            }
        }

        [TestMethod]
        public void WhitespaceTest()
        {
            //Test without spaces
            Assert.AreEqual(false, new ReversePolishNotation("5*5*2>5*5+5*5").Evaluate(x => x));
            //Test with many spaces
            Assert.AreEqual(false, new ReversePolishNotation("5      *5        * 2 >        5  *  5   + 5  *  5").Evaluate(x => x));
            //Test with newlines
            Assert.AreEqual(false, new ReversePolishNotation("5\n\n*\n5\n*\n\n\n\n2>\n\n5\n*\n5\n\n\n\n+5*5").Evaluate(x => x));
            //Test with tabs
            Assert.AreEqual(false, new ReversePolishNotation("5\t*\t5\t*\t2\t>\t5\t\t\t\t*5\t\t\t+5*5").Evaluate(x => x));
        }

        [TestMethod]
        public void BigNumberTest()
        {
            Assert.AreEqual(5000000000000m, new ReversePolishNotation("5000000000*1000").Evaluate(x => x));
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
            Assert.AreEqual(true, new ReversePolishNotation("${customer.Age} + ${customer.Age} == ${customer.Age} * 2").Evaluate(context.GetValue));
        }

        [TestMethod]
        public void NoContextTest()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"customer", new Customer(){ Age=12, Name="Customer1"}}
            }, null);

            Assert.AreEqual(true, new ReversePolishNotation("${customer.Age != 12").Evaluate(context.GetValue));
            Assert.AreEqual("${customer.Age", new ReversePolishNotation("${customer.Age").Evaluate(context.GetValue));
            Assert.AreEqual(true, new ReversePolishNotation("{customer.Age} != 12").Evaluate(context.GetValue));
            Assert.AreEqual("{customer.Age", new ReversePolishNotation("{customer.Age").Evaluate(context.GetValue));
            Assert.AreEqual(true, new ReversePolishNotation("$customer.Age} != 12").Evaluate(context.GetValue));
            Assert.AreEqual("$customer.Age}", new ReversePolishNotation("$customer.Age}").Evaluate(context.GetValue));
            Assert.AreEqual(true, new ReversePolishNotation("customer.Age != 12").Evaluate(context.GetValue));
            Assert.AreEqual("customer.Age", new ReversePolishNotation("customer.Age").Evaluate(context.GetValue));
        }

        #endregion
        
        #region Constant tests (empty)

        [TestMethod]
        public void CompareToEmptyTest()
        {
            Assert.AreEqual(true, new ReversePolishNotation("Empty == Empty").Evaluate(x => x));
        }

        [TestMethod]
        public void EmptyCaseSensitiveTest()
        {
            //Only "Empty" and "empty" are supported. 
            Assert.AreEqual(true, new ReversePolishNotation("Empty == empty").Evaluate(x => x));
        }

        [TestMethod]
        public void CompareEmptyToIntegerTest()
        {
            Assert.AreEqual(true, new ReversePolishNotation("Empty != 1").Evaluate(x => x));
            Assert.AreEqual(true, new ReversePolishNotation("Empty != 0").Evaluate(x => x));
        }

        [TestMethod]
        public void CompareEmptyToContextTest()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"customer", null}
            }, null);

            Assert.AreEqual(true, new ReversePolishNotation("${customer} == Empty").Evaluate(context.GetValue));
        }

        [TestMethod]
        public void CompareEmptyToEmptyStringTest()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"customer", new Customer(){ Name=""}}
            }, null);

            Assert.AreEqual(true, new ReversePolishNotation("${customer.Name} == Empty").Evaluate(context.GetValue));
        }

        #endregion

        #region Datetime tests

        [TestMethod]
        public void DateTimeTest()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"date", new DateTime(2010,1,1)}
            }, null);

            Assert.AreEqual(new DateTime(2010, 1, 1), new ReversePolishNotation("${date}").Evaluate(context.GetValue));
        }

        [TestMethod]
        public void DateTimeOperatorTest()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"date1", new DateTime(2010,1,1)},
                {"date2", new DateTime(2011,1,1)}
            }, null);

            Assert.AreEqual(true, new ReversePolishNotation("${date1}<${date2}").Evaluate(context.GetValue));
            Assert.AreEqual(true, new ReversePolishNotation("${date1}!=${date2}").Evaluate(context.GetValue));
            Assert.AreEqual(true, new ReversePolishNotation("${date2}>${date1}").Evaluate(context.GetValue));
        }

        #endregion
    }
}
