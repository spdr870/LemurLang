using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LemurLang;
using LemurLang.Templates;

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
                RootTemplate testTemplate = engine.BuildTemplate(testTemplateString);

                Console.WriteLine("TREE -->");
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine(testTemplate.DisplayTree(0));

                var context = new Dictionary<string, object>() {
                    {"Program", new Program(){ Variable = 6 }},
                    {"people", new List<Person>{
                        new Person(){ Age=20, Bio="Story1", Name ="Test1"},
                        new Person(){ Age=25, Bio="Story2", Name ="Test2"},
                        new Person(){ Age=30, Bio="Story3", Name ="Test3"}
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
                //string result = testTemplate.Evaluate(evaluationContext);
                //Console.WriteLine(result);
                testTemplate.Evaluate(evaluationContext, x => Console.Write(x));

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
            RootTemplate testTemplate = engine.BuildTemplate(template);
            testTemplate.Evaluate(context, x => { });//without IO or storing in string(builder)
        }
    }
}
