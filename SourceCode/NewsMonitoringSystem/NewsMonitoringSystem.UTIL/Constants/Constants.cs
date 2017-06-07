namespace NewsMonitoringSystem.UTIL.Constants
{
    public static class Constants
    {
        public const string DEFAULT_DATE_TIME_FORMAT = "d MMMM yyyy";
        public const string ADMIN_TOKEN_HEADER_NAME = "Admin-Token";

        public const int DEFAULT_ITEMS_PER_PAGE = 10;

        public static class UrlPaths
        {
            public const string GET_DOCUMENTS = "/documents";
        }

        public static class Sort
        {
            public const string DESCENDING = "DESC";
            public const string ASCENDING = "ASC";
        }
    }
}