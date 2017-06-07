namespace NewsMonitoringSystem.UTIL.Extensions
{
    public static class NullableExtensions
    {
        /// <summary>
        /// Return empty string for NULL value
        /// and real string value for not NULL value
        /// </summary>
        public static string ToString<T>(this T? value) where T : struct
        {
            return value.HasValue 
                     ? value.Value.ToString() 
                     : string.Empty;
        }
    }
}