using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LemurLang.Test
{
    /// <summary>
    /// Summary description for EvaluationContextTest
    /// </summary>
    [TestClass]
    public class EvaluationContextTest
    {
        private class Customer
        {
            public string Name { get; set; }
            public Customer Friend { get; set; }
            public int Age { get; set; }
        }
        
        [TestMethod]
        public void SimplePOCO()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"customer", new Customer(){ Age=12, Name="Customer1"}}
            }, null);

            Assert.AreEqual("Customer1", context.GetValue("customer.Name").ToString());
            Assert.AreEqual(12, (int)context.GetValue("customer.Age"));
        }

        [TestMethod]
        public void SimplePOCOs()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {
                    "customer",
                    new Customer()
                    {
                        Age=12,
                        Name="Customer1",
                        Friend = new Customer()
                        {
                             Name= "Friend",
                             Friend = new Customer()
                             {
                                 Name = "Subfriend"
                             }
                        }
                    }
                }
            }, null);

            Assert.AreEqual("Subfriend", context.GetValue("customer.Friend.Friend.Name").ToString());
        }

        [TestMethod]
        public void Dictionary()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"customer", new Dictionary<string, string>(){ {"Name", "Customer1"}}}
            }, null);

            Assert.AreEqual("Customer1", context.GetValue("customer.Name").ToString());
        }

        [TestMethod]
        public void ListCount()
        {
            EvaluationContext context = new EvaluationContext(new Dictionary<string, object>() {
                {"integerlist", new List<int>(){1, 2}}
            }, null);

            Assert.AreEqual(2, (int)context.GetValue("integerlist.Count"));
        }
    }
}
