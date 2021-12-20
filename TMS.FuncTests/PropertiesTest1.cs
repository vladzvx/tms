using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TMS.FuncTests
{
    [TestClass]
    public class PropertiesTest1
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            CheckProperties(context);
            Environment.SetEnvironmentVariable("TEST", context.Properties["TEST"].ToString());
        }
        private static void CheckProperties(TestContext context)
        {
            Assert.IsTrue(context.Properties["TEST"] != null);
        }

        [TestMethod]
        public void EnvVarTest()
        {
            string var = Environment.GetEnvironmentVariable("TEST");
            Assert.IsTrue(var != null);
        }
    }
}
