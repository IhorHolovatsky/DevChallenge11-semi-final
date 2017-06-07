using System;
using System.Configuration;

namespace NewsMonitoringSystem.UTIL
{
    public static class ConfigUtils
    {
        public enum AppSettingKeys
        {
            AdminToken,
            UrlToMonitor,
            MinimumSearchDateTime
        }

        #region Public Methods
        /// <summary>
        /// Gets the value from the applications appSettings. This will error if the appSetting is not defined.
        /// </summary>
        /// <param name="appSettingName">Name of the appSettings key.</param>
        /// <returns></returns>
        public static T GetAppSetting<T>(string appSettingName) where T : IConvertible
        {
            return GetAppSettingInternal(appSettingName, false, default(T));
        }

        public static T GetAppSetting<T>(AppSettingKeys appSettingName) where T : IConvertible
        {
            return GetAppSettingInternal(appSettingName.ToString(), false, default(T));
        }

        public static T GetAppSetting<T>(AppSettingKeys appSettingName, T defaultValue) where T : IConvertible
        {
            return GetAppSettingInternal(appSettingName.ToString(), true, defaultValue);
        }

        /// <summary>
        /// Gets the value from the applications appSettings.
        /// </summary>
        /// <param name="appSettingName">Name of the appSettings key.</param>
        /// <param name="defaultValue">The default value returned if the appSetting has not been defined.</param>
        /// <returns></returns>
        public static T GetAppSetting<T>(string appSettingName, T defaultValue) where T : IConvertible
        {
            return GetAppSettingInternal(appSettingName, true, defaultValue);
        }

        #endregion

        #region Private Methods

        private static T GetAppSettingInternal<T>(string appSettingName, bool useDefaultOnUndefined, T defaultValue)
            where T : IConvertible
        {
            var value = ConfigurationManager.AppSettings[appSettingName];

            // require that all appSettings are either defined or have explicitly been given a default value
            if (value == null)
            {
                if (useDefaultOnUndefined)
                {
                    return defaultValue;
                }
                throw new Exception($"{appSettingName} not defined in appSettings.");
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }

        #endregion
    }
}