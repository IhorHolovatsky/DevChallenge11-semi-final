using NewsMonitoringSystem.Data.Entities.Generated;
using System;

namespace NewsMonitoringSystem.Managers.Intefaces
{
    /// <summary>
    /// Manager which has responsibility to find documents and import them to our system
    /// </summary>
    public interface IDocumentImportManager
    {
        /// <summary>
        /// Synchronize all documents that are on site with documents in system
        /// It Inserts new documents, update existing documents, and delete deleted documents
        /// </summary>
        void SyncAllDocuments();


        /// <summary>
        /// Synchronize all documents that are on site with documents in system
        /// It Inserts new documents, update existing documents, and delete deleted documents
        /// </summary>
        /// <param name="startDate">Search for documents which were published after/on startDate and to current time</param>
        void SyncAllDocuments(DateTime startDate);

        /// <summary>
        /// Synchronize all documents that are on site with documents in system
        /// It Inserts new documents, update existing documents, and delete deleted documents
        /// </summary>
        /// <param name="startDate">Search for documents which were published after/on startDate</param>
        /// <param name="endDate">Search for documents which were published before/on endDate</param>
        void SyncAllDocuments(DateTime startDate,
                              DateTime endDate);

        /// <summary>
        /// Get Document content
        /// </summary>
        string GetDocumentContent(Document document);
    }
}