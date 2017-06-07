using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsMonitoringSystem.UTIL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsMonitoringSystem.UTIL.Test
{
    [TestClass]
    public class NullableExtensionsTest
    {
        [TestMethod]
        public void TestToString()
        {
            //Arrange
            int? i1 = 5;
            int? i2 = null;

            //Act
            var s1 = i1.ToString<int>();
            var s2 = i2.ToString<int>();

            //Assert
            Assert.AreEqual(s1, "5");
            Assert.AreEqual(s2, "");
        }
    }
}
