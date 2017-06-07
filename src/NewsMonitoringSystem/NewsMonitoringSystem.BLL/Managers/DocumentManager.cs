using System.Collections.Generic;
using System.Linq;
using NewsMonitoringSystem.Data.Entities.Generated;
using NewsMonitoringSystem.Managers.Intefaces;
using NewsMonitoringSystem.Data;
using System;
using System.Data.Entity;

namespace NewsMonitoringSystem.Managers
{
    public class DocumentManager : IDocumentManager
    {
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Public Methods

        /// <summary>
        /// Get the most recent revision of document
        /// </summary>
        public Document GetDocumentByIdRecentRevision(Guid documentId)
        {
            using (var dbContext = new NewsContext())
            {
                var documents = (from x in dbContext.Documents
                                 where x.DocumentId == documentId
                                 orderby x.Revision descending
                                 select x).FirstOrDefault();

                return documents;
            }
        }

        /// <summary>
        /// Search documents with similar identifier.
        /// Actually it should return only one document for given Identifier, but really there are documents with the same identifier
        /// So here could be more than one document
        /// </summary>
        /// <param name="documentIdentifier"></param>
        public List<Document> GetDocumentsByIdentifier(string documentIdentifier)
        {
            using (var dbContext = new NewsContext())
            {
                var documents = (from x in dbContext.Documents
                                 where x.DocumentIdentifier.Contains(documentIdentifier)
                                 select x).ToList();

                return documents;
            }
        }

        /// <summary>
        /// Get all Documents (including deleted documents)
        /// </summary>
        public List<Document> GetAllDocuments()
        {
            using (var dbContext = new NewsContext())
            {
                var documents = (from x in dbContext.Documents
                                 select x).ToList();

                return documents;
            }
        }

        /// <summary>
        /// Get All Active documents (documents which exists on site)
        /// </summary>
        public List<Document> GetAllActiveDocuments()
        {
            using (var dbContext = new NewsContext())
            {
                var documents = (from x in dbContext.Documents
                                 where !x.IsDeleted
                                 select x).ToList();

                return documents;
            }
        }

        /// <summary>
        /// Get All Active documents, only recent revision 
        /// </summary>
        public List<Document> GetActiveDocumentsByDateRecentVersion(DateTime startDate,
                                                                    DateTime endDate)
        {
            var documents = GetActiveDocumentsByDate(startDate,
                                                     endDate);

            return documents.GroupBy(d => d.DocumentId)
                            .ToDictionary(k => k.Key, v => v.ToList())
                            .Select(k => k.Value.OrderByDescending(d => d.Revision).First())
                            .ToList();
        }

        /// <summary>
        /// Get All Active documents, 
        /// which were published in specific date range
        /// </summary>
        public List<Document> GetActiveDocumentsByDate(DateTime startDate,
                                                       DateTime endDate)
        {
            using (var dbContext = new NewsContext())
            {
                var documents = (from x in dbContext.Documents
                                 where x.PublishDate >= startDate
                                       && x.PublishDate <= endDate
                                       && !x.IsDeleted
                                 select x).ToList();

                return documents;
            }
        }

        /// <summary>
        /// Get All documents, 
        /// which were published in specific date range
        /// </summary>
        public List<Document> GetDocumentsByDate(DateTime startDate,
                                                 DateTime endDate)
        {
            using (var dbContext = new NewsContext())
            {
                var documents = (from x in dbContext.Documents
                                 where x.PublishDate >= startDate
                                       && x.PublishDate <= endDate
                                 select x).ToList();

                return documents;
            }
        }


        /// <summary>
        /// Get Documents by Id
        /// </summary>
        public List<Document> GetDocumentsById(Guid documentId)
        {
            using (var dbContext = new NewsContext())
            {
                var documents = (from x in dbContext.Documents
                                 where x.DocumentId == documentId
                                 select x).ToList();

                return documents;
            }
        }


        /// <summary>
        /// Get Document by Id and concrete revision
        /// </summary>
        public Document GetDocumentByIdAndRevision(Guid documentId, int revision)
        {
            using (var dbContext = new NewsContext())
            {
                var document = (from x in dbContext.Documents
                                 where x.DocumentId == documentId
                                       && x.Revision == revision
                                 select x).FirstOrDefault();

                return document;
            }
        }

        /// <summary>
        /// Mark documents (including whole history of document changes) as deleted (since we want to save deleted documents in our system)
        /// </summary>
        public void DeleteDocuments(List<Document> documentsToDelete)
        {
            var documentGuids = documentsToDelete.Select(d => d.DocumentId)
                                                 .Distinct();

            using (var dbContext = new NewsContext())
            {
                foreach (var documentId in documentGuids)
                {
                    var documentHistory = GetDocumentsById(documentId);
                    documentHistory.ForEach(b =>
                                            {
                                                b.IsDeleted = true;
                                                dbContext.Documents.Attach(b);
                                                dbContext.Entry(b).State = EntityState.Modified;
                                            });


                }

                dbContext.SaveChanges();
            }
        }

        #endregion

        #region Private Methods
        #endregion
    }
}