using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LemurLang.Expression;

namespace LemurLang.Test
{
    [TestClass]
    public class ReversePolishNotationTest
    {
        [TestMethod]
        public void CompareBooleanGreaterThan()
        {
            Assert.AreEqual(5, new ReversePolishNotation("1 + 4").Evaluate(x => x));
            Assert.AreEqual(9, new ReversePolishNotation("1 + 4 * 2").Evaluate(x => x));
            Assert.AreEqual(10, new ReversePolishNotation("(1 + 4) * 2").Evaluate(x => x));

            Assert.AreEqual(8, new ReversePolishNotation("4 * 1 + 4").Evaluate(x => x));
            Assert.AreEqual(10, new ReversePolishNotation("1 + 4 * 2 + 1").Evaluate(x => x));
            Assert.AreEqual(5, new ReversePolishNotation("(1 + 4) * 2 / 2").Evaluate(x => x));

            Assert.AreEqual(3, new ReversePolishNotation("4 + -1").Evaluate(x => x));
            Assert.AreEqual(-3, new ReversePolishNotation("-4 + 1").Evaluate(x => x));

            Assert.AreEqual(-4, new ReversePolishNotation("-4").Evaluate(x => x));

            Assert.AreEqual(4.4m, new ReversePolishNotation("1.1 * 4").Evaluate(x => x));
        }
    }
}
