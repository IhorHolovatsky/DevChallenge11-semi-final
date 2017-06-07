using System.Collections.Generic;
using AngleSharp.Dom.Html;
using NewsMonitoringSystem.Data.Entities.Generated;

namespace NewsMonitoringSystem.Managers.Intefaces
{
    /// <summary>
    /// Manager which has responsibilities to Parse document info from HTML dom
    /// </summary>
    public interface IDocumentParserManager
    {
        /// <summary>
        /// Parse Documents from Html dom
        /// </summary>
        /// <returns>List of parsed Documents</returns>
        List<Document> ParseCurrentPage(IHtmlDocument htmlDom);

        /// <summary>
        /// Parse Document Content, fill Html Content and Text content to document object and return string.
        /// </summary>
        string ParseDocument(IHtmlDocument htmlDom, Document document);
    }
}