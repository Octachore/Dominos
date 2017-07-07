using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
    public class TestClass1 : AbstractApiTest
    {
        [Test]
        public void Dummy()
        {
            Assert.That(Get("/hello").Result, Is.EqualTo("Hello GET"));
        }
    }
}
