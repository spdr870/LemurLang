using System.Collections.Generic;
using LemurLang.Exceptions;
using LemurLang.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LemurLang.Test
{
    /// <summary>
    /// Summary description for TemplateEngineTest
    /// </summary>
    [TestClass]
    public class TemplateEngineTest
    {
        [TestMethod]
        public void ForeachWithMultipleElements()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#foreach(${person} in ${persons})${person}#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"persons", new List<string>(){
                    "one",
                    "two",
                    "three"
                }}
            },
            null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual("onetwothree", result);
        }

        [TestMethod]
        public void ForeachWithOneElement()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#foreach(${person} in ${persons})${person}#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"persons", new List<string>(){
                    "one"
                }}
            },
            null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual("one", result);
        }

        [TestMethod]
        public void ForeachWithoutElements()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#foreach(${person} in ${persons})${person}#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"persons", new List<string>(){
                }}
            },
            null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void SingleLineCommentWithLineEnding()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("##aaa${test}aaa#end#foreacg#if###\n");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() { }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void SingleLineCommentWithoutLineEnding()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("test##aaa${test}aaa#end#foreacg#if###");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() { }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual("test", result);
        }

        [TestMethod]
        public void MultiLineCommentInOneLine()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#*#foreach(${person} in ${persons})${person}#end*#");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() { }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void MultiLineCommentInMultipleLines()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("\r\tq#*#foreach(\r${person} in ${persons})${pe\nrson}#end*#\na");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() { }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual("\r\tqa", result);
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void MultiLineCommentInMultipleLinesWithoutEnding()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("\n\n\r\tq#*#foreach(\r${person} in ${persons})${pe\nrson}#end\na");
        }

        [TestMethod]
        public void Print()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("a${toprint}a");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"toprint", "this"}
            }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual("athisa", result);
        }

        [TestMethod]
        public void Text()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("a$toprint}a");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"toprint", "this"}
            }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual("a$toprint}a", result);
        }

        [TestMethod]
        public void IfTrue()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#if(true)indeed#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
            }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual("indeed", result);
        }

        [TestMethod]
        public void IfFalse()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#if(false)no#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ElseFalse()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#if(false) no #else indeed #end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual(" indeed ", result);
        }

        [TestMethod]
        public void ElseTrue()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#if(true) indeed #else no #end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual(" indeed ", result);
        }

        [TestMethod]
        public void LongElseElseIfElse()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate(@"
            #if(false)
            #elseif(false)
            #elseif(false)
            #elseif(false)
            #else
                indeed
            #end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = templateItem.Evaluate(context);
            Assert.IsTrue(result.Contains("indeed"));
        }

        [TestMethod]
        public void LongElseElseIf()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate(@"
            #if(false)
            #elseif(false)
            #elseif(false)
            #elseif(false)
            #else
            #end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual(string.Empty, result.Trim());
        }

        [TestMethod]
        public void ComplexConditions()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate(@"
            #if(false)
            #elseif(false)
            #elseif(true)
                #if(false)
                #elseif(false)
                    faky
                #elseif(true)
                    #if(false)
                    #elseif(false)
                    #elseif(true)
                        #if(false)
                        #else
                            indeed
                        #end
                    #elseif(false)
                    #else
                    #end
                #elseif(false)
                    test
                #else
                #end
            #elseif(false)
            #else
                some
            #end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = templateItem.Evaluate(context);
            Assert.AreEqual("indeed", result.Trim());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void OneEndTooMany()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#comment a #end #end");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidForeachStart()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#foreach (${person} in ${persons})${person}#end");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidForeachEnd()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#foreach(${person} in ${persons}\n)${person}#end");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidIfStart()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#if (true) #end");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidIfEnd()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("#if(true\r) #end");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidIfPrintStatement()
        {
            TemplateEngine engine = new TemplateEngine();
            RootTemplate templateItem = engine.BuildTemplate("test${asddsa\na}");
        }
    }
}
