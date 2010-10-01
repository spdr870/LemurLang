using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LemurLang.Exceptions;
using System.Text.RegularExpressions;
using LemurLang.Conditions;
using LemurLang.Test.Tools;
using System.Globalization;

namespace LemurLang.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ConditionEngineTest
    {
        [TestMethod]
        public void SimpleExpressions()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            Assert.IsTrue(conditionHandler.Build("true").Evaluate(x => x));
            Assert.IsFalse(conditionHandler.Build("false").Evaluate(x => x));
            Assert.IsTrue(conditionHandler.Build("false || true").Evaluate(x => x));
            Assert.IsTrue(conditionHandler.Build("true || false").Evaluate(x => x));
            Assert.IsFalse(conditionHandler.Build("false && true").Evaluate(x => x));
            Assert.IsFalse(conditionHandler.Build("true && false").Evaluate(x => x));
            Assert.IsTrue(conditionHandler.Build("true && true").Evaluate(x => x));
            Assert.IsFalse(conditionHandler.Build("false && false").Evaluate(x => x));
            Assert.IsFalse(conditionHandler.Build("false && false || true").Evaluate(x => x));
            Assert.IsTrue(conditionHandler.Build("(false && false) || true").Evaluate(x => x));
        }

        [TestMethod]
        public void ComplexExpression1()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            Assert.IsTrue(conditionHandler.Build("true && (true || (false && false)) && true ").Evaluate(x => x));
        }

        [TestMethod]
        public void ComplexExpression2()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            Assert.IsFalse(conditionHandler.Build("false && (true || (false && false)) && true ").Evaluate(x => x));
        }

        [TestMethod]
        public void ComplexExpression3()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            Assert.IsTrue(conditionHandler.Build("true && (false || (false || true)) && true ").Evaluate(x => x));
        }

        [TestMethod]
        public void ComplexExpression4()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            Assert.IsFalse(conditionHandler.Build("true && (false || (false || true)) && false ").Evaluate(x => x));
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression1()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("&& true");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression2()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("true &&");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression3()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("true && false)");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression4()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true && false");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression5()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true &&& false)");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression6()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true ||| false)");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression7()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true || |false)").DisplayTree(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression8()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true && &false)").DisplayTree(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression9()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true && |false)").DisplayTree(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidExpression10()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true |& |false)").DisplayTree(0);
        }

        [TestMethod]
        public void ComparisonConditionElementEqualsTrue()
        {
            bool result = new ComparisonConditionElement(null, "a", "==", "a").Evaluate(null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ComparisonConditionElementEqualsFalse()
        {
            bool result = new ComparisonConditionElement(null, "a", "==", "b").Evaluate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ComparisonConditionElementNotEqualsTrue()
        {
            bool result = new ComparisonConditionElement(null, "a", "!=", "b").Evaluate(null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ComparisonConditionElementNotEqualsFalse()
        {
            bool result = new ComparisonConditionElement(null, "a", "!=", "a").Evaluate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ComparisonConditionElementEqualsNullTrue()
        {
            bool result = new ComparisonConditionElement(null, null, "==", null).Evaluate(null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ComparisonConditionElementEqualsNullFalse()
        {
            bool result = new ComparisonConditionElement(null, null, "==", "a").Evaluate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ComparisonConditionElementEqualsNullFalseReverse()
        {
            bool result = new ComparisonConditionElement(null, "a", "==", null).Evaluate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ComparisonConditionElementNotEqualsNullFalse()
        {
            bool result = new ComparisonConditionElement(null, null, "!=", null).Evaluate(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ComparisonConditionElementNotEqualsNullTrue()
        {
            bool result = new ComparisonConditionElement(null, "a", "!=", null).Evaluate(null);

            Assert.IsTrue(result);
        }


        [TestMethod]
        public void ComparisonConditionElementGreaterThanOrEqualTrue()
        {
            using(new CultureContext(new CultureInfo("en-US")))
            {
                bool result = new ComparisonConditionElement(null, "1", ">=", "1.000000").Evaluate(x => x);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ComparisonConditionElementLessThanOrEqualTrueEqual()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                bool result = new ComparisonConditionElement(null, "1", "<=", "1.000000").Evaluate(x => x);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ComparisonConditionElementLessThanOrEqualTrueLess()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                bool result = new ComparisonConditionElement(null, "0.9", "<=", "1.000000").Evaluate(x => x);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ComparisonConditionElementLessThanOrEqualFalseLess()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                bool result = new ComparisonConditionElement(null, "1.1", "<=", "1.000000").Evaluate(x => x);

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void ComparisonConditionElementGreaterThanTrue()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                bool result = new ComparisonConditionElement(null, "1.5", ">", "1.445").Evaluate(x => x);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ComparisonConditionElementLessThanTrue()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                bool result = new ComparisonConditionElement(null, "1.35555", "<", "1.445").Evaluate(x => x);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ComparisonConditionElementLessThanFalse()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                bool result = new ComparisonConditionElement(null, "1.35555", "<", "1.35555").Evaluate(x => x);

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void ComparisonConditionElementGreaterThanFalse()
        {
            using (new CultureContext(new CultureInfo("en-US")))
            {
                bool result = new ComparisonConditionElement(null, "1.35555", ">", "1.35555").Evaluate(x => x);

                Assert.IsFalse(result);
            }
        }
    }
}
