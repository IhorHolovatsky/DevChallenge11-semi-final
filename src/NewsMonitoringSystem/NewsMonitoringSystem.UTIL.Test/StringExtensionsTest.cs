using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsMonitoringSystem.UTIL.Extensions;
using NewsMonitoringSystem.UTIL.Constants;

namespace NewsMonitoringSystem.UTIL.Test
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void ParseUkrainianCultureDateTest()
        {
            //Arrange
            var s1 = "15 Травня 2017";
            var s2 = "5 Травня 2017";

            //Act
            var d1 = s1.ParseUkrainianCultureDate(Constants.Constants.DEFAULT_DATE_TIME_FORMAT);
            var d2 = s2.ParseUkrainianCultureDate(Constants.Constants.DEFAULT_DATE_TIME_FORMAT);

            //Asssert
            Assert.IsNotNull(d1);
            Assert.IsNotNull(d2);
        }

        [TestMethod]
        public void ToIntTest()
        {
            //Arrange
            var s1 = "25";
            var s2 = "0";

            //Act
            var i1 = s1.ToInt();
            var i2 = s2.ToInt();

            //Asssert
            Assert.IsNotNull(i1);
            Assert.IsNotNull(i2);
        }

        [TestMethod]
        public void ParseDocumentIdentifierTest()
        {
            //Arrange
            var s1 = "(№ 342-234)";

            //Act
            var d = s1.ParseDocumentIdentifier();

            //Asssert
            Assert.IsFalse(string.IsNullOrEmpty(d));
        }
    }
}
