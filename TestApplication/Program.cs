using System;
using System.IO;
using LemurLang;
using LemurLang.Interfaces;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TestApplication
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Bio { get; set; }
    }
   
    class Program
    {
        public int Variable { get; set; }
        
        static void Main(string[] args)
        {
            try
            {
                TemplateEngine engine = new TemplateEngine();

                string testTemplateString = File.ReadAllText("TestTemplate.txt");
                ITemplate testTemplate = engine.BuildTemplate(testTemplateString);

                Console.WriteLine("TREE -->");
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine(testTemplate.DisplayTree(0));

                var context = new Dictionary<string, object>() {
                    {"Program", new Program(){ Variable = 6 }},
                    {"people", new List<Person>{
                        new Person(){ Age=20, Bio="Woei", Name ="Test1"},
                        new Person(){ Age=25, Bio="Waaaaaaa", Name ="Test2"},
                        new Person(){ Age=30, Bio="Blieeee", Name ="Test3"}
                    }},
                    {"dict", new Dictionary<string, object>(){
                        {"test","test"}
                    }},
                    {"emptylist", new List<object>{}}
                };
                EvaluationContext evaluationContext = new EvaluationContext(context, null);

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine("Evaluated:");
                Console.WriteLine(testTemplate.Evaluate(evaluationContext));

                Console.WriteLine();
                Console.WriteLine();

                int times = 1000;
                Console.WriteLine("Running template {0} times -->", times);
                Stopwatch watch = new Stopwatch();
                watch.Start();
                for (int i = 0; i < times; i++)
                {
                    Runtemplate(engine, testTemplateString, evaluationContext);
                }
                watch.Stop();
                Console.WriteLine("Total cost: {0}ms", watch.ElapsedMilliseconds);
                decimal total = watch.ElapsedMilliseconds / (decimal)times;
                Console.WriteLine("Cost per build AND evaluate: {0}ms", total);

                Console.WriteLine();
                Console.WriteLine("Hit enter");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        private static void Runtemplate(TemplateEngine engine, string template, EvaluationContext context)
        {
            ITemplate testTemplate = engine.BuildTemplate(template);
            testTemplate.Evaluate(context);
        }
    }
}
