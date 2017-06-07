using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using log4net;
using NewsMonitoringSystem.Builders.Interfaces;
using NewsMonitoringSystem.Data.Entities.Generated;
using NewsMonitoringSystem.Managers.Intefaces;
using NewsMonitoringSystem.UTIL.Constants;
using NewsMonitoringSystem.UTIL.Extensions;
using AngleSharp.Dom.Html;
using NewsMonitoringSystem.Data;
using NewsMonitoringSystem.Builders;

namespace NewsMonitoringSystem.Managers
{
    /// <summary>
    /// Manager which has responsibility to find documents and import them to our system
    /// </summary>
    public class DocumentImportManager : IDocumentImportManager
    {
        #region Constants and read-only Properties

        private readonly IDocumentQueryStringBuilder _documentQueryStringBuilder;
        private readonly IDocumentParserManager _documentParserManager;
        private readonly IDocumentManager _documentManager;
        private readonly ILog _logger = LogManager.GetLogger(typeof(DocumentImportManager));

        private HtmlParser _parser;
        /// <summary>
        /// Singelton Parser Object
        /// </summary>
        private HtmlParser Parser
        {
            get
            {
                return _parser ?? (_parser = new HtmlParser());
            }
        }
        #endregion

        #region Constructors

        public DocumentImportManager(IDocumentQueryStringBuilder documentQueryStringBuilder,
                                     IDocumentParserManager documentParserManager,
                                     IDocumentManager documentManager)
        {
            _documentQueryStringBuilder = documentQueryStringBuilder;
            _documentParserManager = documentParserManager;
            _documentManager = documentManager;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Synchronize all documents that are on site with documents in system
        /// It Inserts new documents, update existing documents, and delete deleted documents
        /// </summary>
        public void SyncAllDocuments()
        {
            SyncAllDocuments(DocumentQueryStringBuilder.MinDateTime.Date,
                             DateTime.Now.Date);
        }

                /// <summary>
        /// Synchronize all documents that are on site with documents in system
        /// It Inserts new documents, update existing documents, and delete deleted documents
        /// </summary>
        /// <param name="startDate">Search for documents which were published after/on startDate and to current time</param>
        public void SyncAllDocuments(DateTime startDate)
        {
            SyncAllDocuments(startDate,
                             DateTime.Now.Date);
        }

        /// <summary>
        /// Synchronize all documents that are on site with documents in system
        /// It Inserts new documents, update existing documents, and delete deleted documents
        /// </summary>
        /// <param name="startDate">Search for documents which were published after/on startDate</param>
        /// <param name="endDate">Search for documents which were published before/on endDate</param>
        public void SyncAllDocuments(DateTime startDate,
                                     DateTime endDate)
        {
            var documentsInSystem = _documentManager.GetActiveDocumentsByDateRecentVersion(startDate,
                                                                                           endDate);
            var documentsOnSite = new List<Document>();

            var pageCount = 0;
            var i = 0;

            do
            {
                var startFrom = i * Constants.DEFAULT_ITEMS_PER_PAGE;
                var urlToSearch = _documentQueryStringBuilder.GetAllDocumentsQuery(startDate,
                                                                                   endDate,
                                                                                   startFrom);
                var siteContent = GetSiteHtmlContent(urlToSearch);
                var htmlDoc = Parser.Parse(siteContent);

                var documents = _documentParserManager.ParseCurrentPage(htmlDoc);
                documents.ForEach(d => GetDocumentContent(d));
                documentsOnSite.AddRange(documents);

                //Get total pages only once
                if (i == 0)
                {
                    pageCount = GetPageCount(htmlDoc);
                }
            } while (++i < pageCount);

            var documentsToUpdate = GetUpdatedDocuments(documentsInSystem,
                                                        documentsOnSite);
            var documentsToInsert = GetNewDocuments(documentsInSystem,
                                                    documentsToUpdate,
                                                    documentsOnSite);
            var documentsToDelete = GetDeletedDocuments(documentsInSystem, 
                                                        documentsToUpdate,
                                                        documentsOnSite);

            _logger.Info($"Found {documentsToInsert.Count} documents to Insert.");
            _logger.Info($"Found {documentsToUpdate.Count} documents to Updated.");
            if (documentsToInsert.Count != 0
                || documentsToUpdate.Count != 0)
            {
                using (var dbContext = new NewsContext())
                {
                    dbContext.Configuration.AutoDetectChangesEnabled = false;

                    documentsToInsert.ForEach(d => d.DocumentId = Guid.NewGuid());

                    dbContext.Documents.AddRange(documentsToInsert);

                    //Update is actually inserting new entity, with same ID, but other revision
                    //(yes, we have composite primary key)
                    dbContext.Documents.AddRange(documentsToUpdate);
                    dbContext.SaveChanges();
                }

                _logger.Info($"Saved successfuly.");
            }

            _logger.Info($"Found {documentsToDelete.Count} documents to Delete.");
            if (documentsToDelete.Count != 0)
            {
                _documentManager.DeleteDocuments(documentsToDelete);
                _logger.Info($"Successfuly deleted {documentsToDelete.Count} documents.");
            }          

        }

        /// <summary>
        /// Get Document content
        /// </summary>
        public string GetDocumentContent(Document document)
        {
            var urlToLoadDocument = _documentQueryStringBuilder.GetDocumentContentQuery(document);
            var siteContent = GetSiteHtmlContent(urlToLoadDocument);

            var parser = new HtmlParser(new HtmlParserOptions()
            {
                IsScripting = true
            });

            var htmlDocument = parser.Parse(siteContent);
            _documentParserManager.ParseDocument(htmlDocument, document);

            return document.TextContent;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Makes request to Url, and return response as HTML 
        /// </summary>
        /// <returns>Html response string</returns>
        private string GetSiteHtmlContent(string url)
        {
            var content = string.Empty;
            var response = MakeRequest(url);

            using (var receiveStream = response.GetResponseStream())
            {
                if (receiveStream == null)
                {
                    var exception = new Exception($"response stream is null. Url: ${url}, Response Status Code: {response.StatusCode}");
                    _logger.Error(exception);
                    throw exception;
                }

                var readStream = response.CharacterSet == null
                                    ? new StreamReader(receiveStream)
                                    : new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                content = readStream.ReadToEnd();
                readStream.Close();
            }

            return content;
        }

        /// <summary>
        /// Makes request to Url, and returns web response
        /// </summary>
        private HttpWebResponse MakeRequest(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                return (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {
                var exception = new Exception($"Failed to make request to {url}", e);
                _logger.Error(exception);
                throw exception;
            }
        }

        /// <summary>
        /// Get Count of pages. 
        /// It always 0, if totalCount label was not found
        /// </summary>
        /// <param name="htmlDoc">Html DOM object</param>
        /// <returns>Count of pages</returns>
        private int GetPageCount(IHtmlDocument htmlDoc)
        {
            var totalCountElement = htmlDoc.QuerySelector("#bgFilter p span");
            var totalCount = totalCountElement.TextContent.ToInt();

            return !totalCount.HasValue
                        ? 0
                        : ((double)totalCount.Value / Constants.DEFAULT_ITEMS_PER_PAGE).Round();
        }

        /// <summary>
        /// Compares documents which are in system, with actual ones, and return new documents
        /// Since we are saving different versions of one document, we skip Updated documents
        /// </summary>
        /// <param name="documentsInSystem">Documents that are in system</param>
        /// <param name="documentsToUpdate">Documents that were updated</param>
        /// <param name="parsedDocuments">Documents that are on site</param>
        /// <returns>New Documents, that are not exist in system</returns>
        private List<Document> GetNewDocuments(List<Document> documentsInSystem,
                                               List<Document> documentsToUpdate,
                                               List<Document> parsedDocuments)
        {
            return parsedDocuments.Where(pd => !documentsInSystem.Contains(pd)
                                               && !documentsToUpdate.Contains(pd))
                                  .ToList();
        }

        /// <summary>
        /// Compares documents which are in system, with actual ones, and return documents that were deleted from Site
        /// Since we are saving different versions of one document, we skip Updated documents
        /// </summary>
        /// <param name="documentsInSystem">Documents that are in system</param>
        /// <param name="documentsToUpdate">Documents that were updated</param>
        /// <param name="parsedDocuments">Documents that are on site</param>
        /// <returns>Documents, that were deleted from Site</returns>
        private List<Document> GetDeletedDocuments(List<Document> documentsInSystem,
                                                   List<Document> documentsToUpdate,
                                                   List<Document> parsedDocuments)
        {
            return documentsInSystem.Where(pd => !parsedDocuments.Contains(pd) 
                                                 && !documentsToUpdate.Contains(pd))
                                    .ToList();
        }

        /// <summary>
        /// Compares documents which are in system, with actual ones, and return documents that were modified
        /// </summary>
        /// <param name="documentsInSystem">Documents that are in system</param>
        /// <param name="parsedDocuments">Documents that are on site</param>
        /// <returns>Documents, that were modified</returns>
        private List<Document> GetUpdatedDocuments(List<Document> documentsInSystem,
                                                  List<Document> parsedDocuments)
        {
            var documentsToUpdate = new List<Document>();

            foreach(var document in parsedDocuments)
            {
                var documentInSystem = documentsInSystem.FirstOrDefault(d => d.Link == document.Link);
                
                if (documentInSystem == null)
                    continue;
                
                
                if (!document.Equals(documentInSystem))
                {
                    var newRevision = new Document()
                    {
                        DocumentId = documentInSystem.DocumentId,
                        Revision = ++documentInSystem.Revision,
                        DocumentIdentifier = documentInSystem.DocumentIdentifier,
                        PublishDate = documentInSystem.PublishDate,
                        Title = document.Title,
                        HtmlContent = document.HtmlContent,
                        TextContent = document.TextContent,
                        Link = document.Link,
                    };

                    documentsToUpdate.Add(newRevision);
                }
            }

            return documentsToUpdate;
        }

        #endregion
    }
}