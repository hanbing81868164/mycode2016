using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace System.Common.Tests
{
    [TestClass()]
    public class LogHelpTests
    {
        [TestMethod()]
        public void DebugTest()
        {
            LogHelp.Debug("测试日志功能");
            try
            {
              int i=  int.Parse("abc");
            }
            catch (Exception ex) {
                LogHelp.Error(null, ex);
            }
            Assert.Fail();
        }

        [TestMethod()]
        public void DebugTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InfoTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InfoTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WarnTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void WarnTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ErrorTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ErrorTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FatalTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FatalTest1()
        {
            Assert.Fail();
        }
    }
}
