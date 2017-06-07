using System;
using NewsMonitoringSystem.UTIL.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewsMonitoringSystem.UTIL.Test
{
    [TestClass]
    public class DoubleExtensionsTest
    {
        [TestMethod]
        public void TestRound()
        {
            //Arrange
            var double1 = 5.0;
            var double2 = 3.0;

            //Act
            var result = (double1 / double2).Round();

            //Assert
            Assert.AreEqual(result, 2);
        }
    }
}
