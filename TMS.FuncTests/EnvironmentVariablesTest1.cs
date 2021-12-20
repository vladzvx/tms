using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text.RegularExpressions;
using TMS.Tests.Common;

namespace TMS.FuncTests
{
    [TestClass]
    public class EnvironmentVariablesTest1
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Utils.EnvFileReader(".env");
        }

        [TestMethod]
        public void EnvVarTest1()
        {
            string TEST1 = Environment.GetEnvironmentVariable("TEST1");
            string TEST2 = Environment.GetEnvironmentVariable("TEST2");
            string TEST3 = Environment.GetEnvironmentVariable("TEST3");
            string TEST4 = Environment.GetEnvironmentVariable("TEST4");
            Assert.IsTrue(TEST1 == "TEST1_Val");
            Assert.IsTrue(TEST2 == "TEST2_Val");
            Assert.IsTrue(TEST3 == "TEST3_Val");
            Assert.IsTrue(TEST4 == "TEST4_Val");
        }
    }
}
