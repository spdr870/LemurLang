using System.Globalization;
using LemurLang.Conditions;
using LemurLang.Exceptions;
using LemurLang.Test.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LemurLang.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ConditionEngineTest
    {
        [TestMethod]
        public void SimpleTemplates()
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
        public void ComplexTemplate1()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            Assert.IsTrue(conditionHandler.Build("true && (true || (false && false)) && true ").Evaluate(x => x));
        }

        [TestMethod]
        public void ComplexTemplate2()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            Assert.IsFalse(conditionHandler.Build("false && (true || (false && false)) && true ").Evaluate(x => x));
        }

        [TestMethod]
        public void ComplexTemplate3()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            Assert.IsTrue(conditionHandler.Build("true && (false || (false || true)) && true ").Evaluate(x => x));
        }

        [TestMethod]
        public void ComplexTemplate4()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            Assert.IsFalse(conditionHandler.Build("true && (false || (false || true)) && false ").Evaluate(x => x));
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate1()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("&& true");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate2()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("true &&");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate3()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("true && false)");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate4()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true && false");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate5()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true &&& false)");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate6()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true ||| false)");
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate7()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true || |false)").DisplayTree(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate8()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true && &false)").DisplayTree(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate9()
        {
            ConditionEngine conditionHandler = new ConditionEngine();

            conditionHandler.Build("(true && |false)").DisplayTree(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void InvalidTemplate10()
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

        [TestMethod]
        [ExpectedException(typeof(ConditionException))]
        public void CompareBooleanGreaterThan()
        {
            //When > or < operators are used, nummeric values are required.
            //Since these values are strings, this must result in an exception.
            new ComparisonConditionElement(null, "true", ">", "false").Evaluate(x => x);
        }

        [TestMethod]
        public void CompareBooleanToInt()
        {
            //Since the language is loosely typed, this will be compared as strings.
            bool result = new ComparisonConditionElement(null, "true", "==", "1").Evaluate(x => x);
            Assert.IsFalse(result);
        }
    }
}
