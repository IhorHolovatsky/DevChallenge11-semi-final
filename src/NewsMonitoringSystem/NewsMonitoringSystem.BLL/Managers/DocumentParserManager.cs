using System.Collections.Generic;
using System.Linq;
using System.Text;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using log4net;
using NewsMonitoringSystem.Data.Entities.Generated;
using NewsMonitoringSystem.Managers.Intefaces;
using NewsMonitoringSystem.UTIL.Constants;
using NewsMonitoringSystem.UTIL.Extensions;
using System.Data.SqlTypes;

namespace NewsMonitoringSystem.Managers
{
    /// <summary>
    /// Manager which has responsibilities to Parse document info from HTML dom
    /// </summary>
    public class DocumentParserManager : IDocumentParserManager
    {
        #region Private Properties

        private readonly ILog _logger = LogManager.GetLogger(typeof(DocumentParserManager));

        #endregion

        #region Public Methods

        /// <summary>
        /// Parse Documents from Html dom
        /// </summary>
        /// <returns>List of parsed Documents</returns>
        public List<Document> ParseCurrentPage(IHtmlDocument htmlDom)
        {
            if (htmlDom == null)
            {
                return new List<Document>();
            }

            var htmlDocumentsDiv = htmlDom.QuerySelectorAll("div.bg1-content.col-md-8.col-sm-8").FirstOrDefault();

            if (htmlDocumentsDiv == null)
            {
                //Silently log this error
                _logger.Error($"Documents Div was not found. Url: {htmlDom.Url}");
                return new List<Document>();
            }
            
            return GetDocuments(htmlDocumentsDiv); ;
        }

        /// <summary>
        /// Parse Document Content, fill Html Content and Text content to document object and return string.
        /// </summary>
        public string ParseDocument(IHtmlDocument htmlDom, Document document)
        {
            if (htmlDom == null)
            {
                return string.Empty;
            }

            var documentContent = new StringBuilder();

            var htmlDocumentDiv = htmlDom.QuerySelectorAll("div.bg1-content.col-md-8.col-sm-8").FirstOrDefault();

            //We know that all important text are in <p> tags
            var htmlDocumentParagraphs = htmlDocumentDiv.ChildNodes.Where(n => n is IHtmlParagraphElement)
                                                                   .ToList();

            htmlDocumentParagraphs.ForEach(p => documentContent.AppendLine(p.TextContent));

            document.TextContent = documentContent.ToString();
            document.HtmlContent = htmlDocumentDiv.InnerHtml;

            return documentContent.ToString();
        }
        #endregion


        #region Private Methods

        /// <summary>
        /// Get List of Documents from Html DOM
        /// </summary>
        /// <param name="htmlDocumentsDiv">div with html document list</param>
        /// <returns>Parsed List of Documents</returns>
        private List<Document> GetDocuments(INode htmlDocumentsDiv)
        {
            var documents = new List<Document>();
            var htmlDocuments = htmlDocumentsDiv.ChildNodes.Where(n => n is IHtmlAnchorElement
                                                                      || n is IHtmlSpanElement)
                                                           .ToList();

            //Because of not good design of site, Document html item has following structure:
            // -- <span> with publish date
            // -- <a> with document title and link
            for (var i = 0; i < htmlDocuments.Count; i++)
            {
                var document = new Document();
                var element = htmlDocuments[i];

                if (element is IHtmlSpanElement)
                {
                    var publishDateElement = element as IHtmlSpanElement;
                    var publishDate = publishDateElement.TextContent.ParseUkrainianCultureDate(Constants.DEFAULT_DATE_TIME_FORMAT);

                    if (!publishDate.HasValue)
                    {
                        //We don't want to stop synchronization because of error in parsing publish date, that why, we log this and continue
                        _logger.Error($"Can't find date for document. Node Value: {element.NodeValue}");
                    }

                    document.PublishDate = publishDate ?? SqlDateTime.MinValue.Value;
                }

                element = htmlDocuments[++i];

                if (element is IHtmlAnchorElement)
                {
                    var documentTitleElement = element as IHtmlAnchorElement;
                    document.Title = documentTitleElement.TextContent;
                    document.Link = documentTitleElement.PathName;
                    document.DocumentIdentifier = documentTitleElement.TextContent.ParseDocumentIdentifier();
                }

                documents.Add(document);
            }

            return documents;
        }        

        #endregion
    }
}