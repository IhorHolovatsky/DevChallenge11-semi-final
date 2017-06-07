using NewsMonitoringSystem.Data.Entities.Generated;
using System;
using System.Collections.Generic;

namespace NewsMonitoringSystem.Managers.Intefaces
{
    public interface IDocumentManager
    {
        /// <summary>
        /// Get the most recent revision of document
        /// </summary>
        Document GetDocumentByIdRecentRevision(Guid documentId);

        /// <summary>
        /// Get Document by Id and concrete revision
        /// </summary>
        Document GetDocumentByIdAndRevision(Guid documentId, int revision);

        /// <summary>
        /// Search documents with similar identifier.
        /// Actually it should return only one document for given Identifier, but really there are documents with the same identifier
        /// So here could be more than one document
        /// </summary>
        /// <param name="documentIdentifier"></param>
        List<Document> GetDocumentsByIdentifier(string documentIdentifier);

        /// <summary>
        /// Get all Documents (including deleted documents)
        /// </summary>
        List<Document> GetAllDocuments();

        /// <summary>
        /// Get All Active documents (documents which exists on site)
        /// </summary>
        List<Document> GetAllActiveDocuments();

        /// <summary>
        /// Get All Active documents, 
        /// which were published in specific date range.
        /// Only recent revision 
        /// </summary>
        List<Document> GetActiveDocumentsByDateRecentVersion(DateTime startDate,
                                                             DateTime endDate);

        /// <summary>
        /// Get All Active documents, 
        /// which were published in specific date range
        /// </summary>
        List<Document> GetActiveDocumentsByDate(DateTime startDate,
                                                DateTime endDate);

        /// <summary>
        /// Get All documents, 
        /// which were published in specific date range
        /// </summary>
        List<Document> GetDocumentsByDate(DateTime startDate,
                                          DateTime endDate);

        /// <summary>
        /// Get Documents by Id
        /// </summary>
        List<Document> GetDocumentsById(Guid documentId);

        /// <summary>
        /// Mark documents (including whole history of document changes) as deleted (since we want to save deleted documents in our system)
        /// </summary>
        void DeleteDocuments(List<Document> documentsToDelete);
    }
}