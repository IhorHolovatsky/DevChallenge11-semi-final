using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NewsMonitoringSystem.UTIL.Extensions
{
    public static class StringExtensions
    {
        private static readonly CultureInfo UkrainianCulture = new CultureInfo("uk-UA");

        /// <summary>
        /// Parses date time, for given dateTime formats
        /// </summary>
        /// <param name="dateString">date to parse</param>
        /// <param name="dateFormat">expected date time format</param>
        /// <returns>NULL if date was not parsed</returns>
        public static DateTime? ParseUkrainianCultureDate(this string dateString, string dateFormat)
        {
            return ParseUkrainianCultureDate(dateString,
                                             new[] { dateFormat });
        }

        /// <summary>
        /// Parses date time, for given dateTime formats
        /// </summary>
        /// <param name="dateString">date to parse</param>
        /// <param name="dateFormats">expected date time formats</param>
        /// <returns>NULL if date was not parsed</returns>
        public static DateTime? ParseUkrainianCultureDate(this string dateString, string[] dateFormats)
        {
            DateTime dateTime;
            if (DateTime.TryParseExact(dateString,
                                       dateFormats,
                                       UkrainianCulture,
                                       DateTimeStyles.None,
                                       out dateTime))
            {
                return dateTime;
            }

            return null;
        }

        /// <summary>
        /// Parse int value from string
        /// </summary>
        public static int? ToInt(this string intString)
        {
            int returnValue;

            if (int.TryParse(intString, out returnValue))
            {
                return returnValue;
            }

            return null;
        }

        /// <summary>
        /// Find Document Identifier in string via static Regex
        /// </summary>
        public static string ParseDocumentIdentifier(this string value)
        {
            var regex = new Regex("\\(№ (.*)\\)");
            var a = regex.Match(value);

            return a.Groups.Count < 2 
                    ? string.Empty 
                    : a.Groups[1].Value;
        }
    }
}