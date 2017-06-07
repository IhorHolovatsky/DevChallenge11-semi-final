using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsMonitoringSystem.Builders.Interfaces;
using NewsMonitoringSystem.Builders;

namespace NewsMonitoringSystem.BLL.Test
{
    [TestClass]
    public class DocumentQueryStringBuilderTest
    {
        private readonly IDocumentQueryStringBuilder _docQueryStringBuilder = new DocumentQueryStringBuilder();

        [TestMethod]
        public void GetAllDocumentsQueryTest()
        {
            //Arrange
            var startFrom = 0;
            var startDate = DateTime.Parse("1/1/2011");
            var endDate = DateTime.Parse("1/1/2011");
            DocumentQueryStringBuilder.BaseUrlToMonitor = "http://google.com";

            //Act
            var url = _docQueryStringBuilder.GetAllDocumentsQuery(startDate, endDate, startFrom);

            //Asssert
            Assert.AreEqual(url, "http://google.com:80/documents?start=0&c=0&d=0&dn=&fd=1&fm=1&fy=2011&td=1&tm=1&ty=2011&o=DESC&s=");
        }
    }
}
