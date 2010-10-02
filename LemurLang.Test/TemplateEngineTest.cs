﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LemurLang.Interfaces;
using LemurLang.Exceptions;

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
            ITemplate expression = engine.BuildTemplate("#foreach(${person} in ${persons})${person}#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"persons", new List<string>(){
                    "one",
                    "two",
                    "three"
                }}
            },
            null);

            string result = expression.Evaluate(context);
            Assert.AreEqual("onetwothree", result);
        }

        [TestMethod]
        public void ForeachWithOneElement()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#foreach(${person} in ${persons})${person}#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"persons", new List<string>(){
                    "one"
                }}
            },
            null);

            string result = expression.Evaluate(context);
            Assert.AreEqual("one", result);
        }

        [TestMethod]
        public void ForeachWithoutElements()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#foreach(${person} in ${persons})${person}#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"persons", new List<string>(){
                }}
            },
            null);

            string result = expression.Evaluate(context);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Comment()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#comment sdhashd${test}avdjsadhsajvdsvdvshdsajdvsahdvjsad#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() { }, null);

            string result = expression.Evaluate(context);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void Print()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("a${toprint}a");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"toprint", "this"}
            }, null);

            string result = expression.Evaluate(context);
            Assert.AreEqual("athisa", result);
        }

        [TestMethod]
        public void Text()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("a$toprint}a");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"toprint", "this"}
            }, null);

            string result = expression.Evaluate(context);
            Assert.AreEqual("a$toprint}a", result);
        }

        [TestMethod]
        public void IfTrue()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#if(true)indeed#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
            }, null);

            string result = expression.Evaluate(context);
            Assert.AreEqual("indeed", result);
        }

        [TestMethod]
        public void IfFalse()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#if(false)no#end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = expression.Evaluate(context);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ElseFalse()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#if(false) no #else indeed #end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = expression.Evaluate(context);
            Assert.AreEqual(" indeed ", result);
        }

        [TestMethod]
        public void ElseTrue()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#if(true) indeed #else no #end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = expression.Evaluate(context);
            Assert.AreEqual(" indeed ", result);
        }

        [TestMethod]
        public void LongElseElseIfElse()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate(@"
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

            string result = expression.Evaluate(context);
            Assert.IsTrue(result.Contains("indeed"));
        }

        [TestMethod]
        public void LongElseElseIf()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate(@"
            #if(false)
            #elseif(false)
            #elseif(false)
            #elseif(false)
            #else
            #end");

            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>()
            {
            }, null);

            string result = expression.Evaluate(context);
            Assert.AreEqual(string.Empty, result.Trim());
        }

        [TestMethod]
        public void ComplexConditions()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate(@"
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

            string result = expression.Evaluate(context);
            Assert.AreEqual("indeed", result.Trim());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void OneEndTooMany()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#comment a #end #end");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidForeachStart()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#foreach (${person} in ${persons})${person}#end");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidForeachEnd()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#foreach(${person} in ${persons}\n)${person}#end");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidIfStart()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#if (true) #end");
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void InvalidIfEnd()
        {
            TemplateEngine engine = new TemplateEngine();
            ITemplate expression = engine.BuildTemplate("#if(true\r) #end");
        }
    }
}
