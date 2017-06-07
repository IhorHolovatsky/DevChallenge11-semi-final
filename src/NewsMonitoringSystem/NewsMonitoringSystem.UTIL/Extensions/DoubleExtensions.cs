using System;

namespace NewsMonitoringSystem.UTIL.Extensions
{
    public static class DoubleExtensions
    {
        /// <summary>
        /// Rounds double value to nearest bigger int
        /// </summary>
        public static int Round(this double value)
        {
            if ((value % 1) == 0)
            {
                return (int) value;
            }

            return (int) Math.Truncate(value) + 1;
        }
    }
}