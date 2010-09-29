using System;
using System.IO;
using LemurLang;
using LemurLang.Interfaces;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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
                ExpressionHandler handler = new ExpressionHandler();
                
                string testTemplate = File.ReadAllText("TestTemplate.txt");
                IExpression testExpression = handler.BuildExpression(testTemplate);

                Console.WriteLine("TREE -->");
                Console.WriteLine();
                Console.WriteLine();
                
                Console.WriteLine(testExpression.DisplayTree(0));

                var context = new Dictionary<string, object>() {
                    {"Program", new Program(){ Variable = 3 }},
                    {"people", new List<Person>{
                        new Person(){ Age=20, Bio="Woei", Name ="Test1"},
                        new Person(){ Age=25, Bio="Waaaaaaa", Name ="Test2"},
                        new Person(){ Age=30, Bio="Blieeee", Name ="Test3"}
                    }},
                    {"dict", new Dictionary<string, object>(){
                        {"test","test"}
                    }}
                };
                EvaluationContext evaluationContext = new EvaluationContext(context, null);

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                
                Console.WriteLine("Evaluated:");
                Console.WriteLine(testExpression.Evaluate(evaluationContext));

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
