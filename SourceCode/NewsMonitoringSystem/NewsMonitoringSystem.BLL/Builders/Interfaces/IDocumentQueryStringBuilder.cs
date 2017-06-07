using System;
using NewsMonitoringSystem.Data.Entities.Generated;

namespace NewsMonitoringSystem.Builders.Interfaces
{
    /// <summary>
    /// Builder which has responsibilities to build url with query, to parse documents
    /// </summary>
    public interface IDocumentQueryStringBuilder
    {
        /// <summary>
        /// Url with queryString to search ALL documents
        /// </summary>
        /// <param name="startFrom">you should use this for paging, and it will skip first N documents</param>
        /// <returns>URL as string</returns>
        string GetAllDocumentsQuery(int? startFrom = null);

        /// <summary>
        /// Url with queryString to search documents for spicific date range
        /// </summary>
        /// <param name="startDate">Search for documents which were published after/on startDate</param>
        /// <param name="endDate">Search for documents which were published before/on endDate</param>
        /// <param name="startFrom">you should use this for paging, and it will skip first N documents</param>
        /// <returns>URL as string</returns>
        string GetAllDocumentsQuery(DateTime startDate,
                                    DateTime endDate,
                                    int? startFrom = null);

        /// <summary>
        /// Get Url for getting printable document content
        /// </summary>
        /// <returns>URL as string</returns>
        string GetDocumentContentQuery(Document document);
    }
}