using System.Collections.Generic;
using System.Linq;

namespace NewsMonitoringSystem.UTIL.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Get SingleOrDefault item from comparable IEnumerable
        /// </summary>
        public static T SingleOrDefault<T>(this IEnumerable<T> source,
                                           T value) where T: IEqualityComparer<T>
        {
            return source.SingleOrDefault((IEqualityComparer<T>)value, 
                                          value);
        }

        public static T SingleOrDefault<T>(this IEnumerable<T> source, 
                                           IEqualityComparer<T> comparer, 
                                           T value)
        {
            return source.SingleOrDefault(item => comparer.Equals(item, value));
        }
    }
}
