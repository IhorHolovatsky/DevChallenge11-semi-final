using System;
using System.Collections.Specialized;
using System.Web;
using NewsMonitoringSystem.Builders.Interfaces;
using NewsMonitoringSystem.Data.Entities.Generated;
using NewsMonitoringSystem.UTIL;
using NewsMonitoringSystem.UTIL.Constants;
using NewsMonitoringSystem.UTIL.Enums;
using NewsMonitoringSystem.UTIL.Extensions;

namespace NewsMonitoringSystem.Builders
{
    /// <summary>
    /// Builder which has responsibilities to build url with query, to parse documents
    /// </summary>
    public class DocumentQueryStringBuilder : IDocumentQueryStringBuilder
    {
        #region Properties

        /// <summary>
        /// Base Url of website, which we need to Monitor
        /// </summary>
        public static string BaseUrlToMonitor
        {
            get
            {
                return _baseUrlToMonitor;
            }
            set
            {
                //For Unit Tests
                _baseUrlToMonitor = value;
            }
        }

        /// <summary>
        /// Url For getting documents
        /// </summary>
        public static string DocumentsUrl => BaseUrlToMonitor + Constants.UrlPaths.GET_DOCUMENTS;

        /// <summary>
        /// DateTime of first published document
        /// </summary>
        public static DateTime MinDateTime => DateTime.Parse(ConfigUtils.GetAppSetting<string>(ConfigUtils.AppSettingKeys.MinimumSearchDateTime, string.Empty));


        private static string _baseUrlToMonitor = ConfigUtils.GetAppSetting<string>(ConfigUtils.AppSettingKeys.UrlToMonitor, string.Empty);
        #endregion

        #region Constructors
        #endregion

        #region Public Methods

        /// <summary>
        /// Url with queryString to search ALL documents
        /// </summary>
        /// <param name="startFrom">you should use this for paging, and it will skip first N documents</param>
        /// <returns>URL as string</returns>
        public string GetAllDocumentsQuery(int? startFrom = null)
        {
            var builder = new UriBuilder(DocumentsUrl)
            {
                Query = GetAllDocumentsQueryString(startFrom).ToString()
            };
            
            return builder.ToString();
        }

        /// <summary>
        /// Url with queryString to search documents for spicific date range
        /// </summary>
        /// <param name="startDate">Search for documents which were published after/on startDate</param>
        /// <param name="endDate">Search for documents which were published before/on endDate</param>
        /// <param name="startFrom">you should use this for paging, and it will skip first N documents</param>
        /// <returns>URL as string</returns>
        public string GetAllDocumentsQuery(DateTime startDate, 
                                           DateTime endDate, 
                                           int? startFrom = null)
        {
            var builder = new UriBuilder(DocumentsUrl)
            {
                Query = GetAllDocumentsQueryString(startDate, 
                                                   endDate, 
                                                   startFrom).ToString()
            };

            return builder.ToString();
        }


        /// <summary>
        /// Get Url for getting printable document content
        /// </summary>
        /// <returns>URL as string</returns>
        public string GetDocumentContentQuery(Document document)
        {
            if (document == null
                || string.IsNullOrEmpty(document.Link))
            {
                return string.Empty;
            }

            return $"{BaseUrlToMonitor}/{document.Link}?=print";
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get Query string to find ALL documents
        /// </summary>
        private NameValueCollection GetAllDocumentsQueryString(int? startFrom = null)
        {
            return GetQueryStringCollection(startFrom,
                                            DocumentCategory.All,
                                            DocumentDestination.All,
                                            null,
                                            MinDateTime,
                                            DateTime.Now,
                                            Constants.Sort.DESCENDING,
                                            string.Empty);
        }

        /// <summary>
        /// Get Query string to find ALL documents
        /// </summary>
        private NameValueCollection GetAllDocumentsQueryString(DateTime startDate, 
                                                               DateTime endDate, 
                                                               int? startFrom = null)
        {
            return GetQueryStringCollection(startFrom,
                                            DocumentCategory.All,
                                            DocumentDestination.All,
                                            null,
                                            startDate,
                                            endDate,
                                            Constants.Sort.DESCENDING,
                                            string.Empty);
        }


        /// <summary>
        /// Get name value collection of required query string parameters for searching documents
        /// </summary>
        /// <returns>Name-Value Collection with filled query string parameters</returns>
        private NameValueCollection GetQueryStringCollection(int? startFrom,
                                                             DocumentCategory documentCategory,
                                                             DocumentDestination documentDestination,
                                                             int? documentNumber,
                                                             DateTime startDate,
                                                             DateTime endDate,
                                                             string sort,
                                                             string containsString)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            if (startFrom.HasValue)
            {
                queryString[QueryString.START_FROM] = startFrom.Value.ToString();
            }

            queryString[QueryString.CATEGORY] = ((int)documentCategory).ToString();
            queryString[QueryString.DESTINATION] = ((int)documentDestination).ToString();
            queryString[QueryString.DOCUMENT_NUMBER] = documentNumber.ToString<int>();
            queryString[QueryString.FROM_DAY] = startDate.Day.ToString();
            queryString[QueryString.FROM_MONTH] = startDate.Month.ToString();
            queryString[QueryString.FROM_YEAR] = startDate.Year.ToString();
            queryString[QueryString.TO_DAY] = endDate.Day.ToString();
            queryString[QueryString.TO_MONTH] = endDate.Month.ToString();
            queryString[QueryString.TO_YEAR] = endDate.Year.ToString();
            queryString[QueryString.SORT] = sort;
            queryString[QueryString.STRING_TO_FIND] = containsString;

            return queryString;
        }

        #endregion
    }
}