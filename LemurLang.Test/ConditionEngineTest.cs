using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LemurLang.Exceptions;
using System.Text.RegularExpressions;

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
    }
}
