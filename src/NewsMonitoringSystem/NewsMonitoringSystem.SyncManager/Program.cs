using System;
using Castle.Windsor;
using NewsMonitoringSystem.IoC;
using NewsMonitoringSystem.Managers.Intefaces;
using NewsMonitoringSystem.UTIL.Extensions;
using NewsMonitoringSystem.Builders;
using log4net;
using System.IO;
using NewsMonitoringSystem.UTIL;

namespace NewsMonitoringSystem.SyncManager
{
    public class Program
    {
        public static WindsorContainer Container { get; set; }

        private static IDocumentImportManager _importManager;
        private static ILog _logger = null;

        public static void Main(string[] args)
        {
            InitLogger();

            try
            {
                InstallIoC();
                _importManager = Container.Resolve<IDocumentImportManager>();

                if (args.Length > 0)
                {
                    if (args[0].Equals(ConfigUtils.GetAppSetting<string>(AppSettings.SearchNewDocumentsForLastWeek_cmdLine.ToString()), StringComparison.OrdinalIgnoreCase))
                    {
                        var startDate = DateTime.Now.AddDays(-7).Date;
                        var endDate = DateTime.Now.Date;

                        _logger.Info($"Starting Importing documents, Start Date: {startDate}, End Date: {endDate}");
                        _importManager.SyncAllDocuments(startDate, endDate);
                        _logger.Info($"Imported successfuly");
                    }
                    else if (args[0].Equals(ConfigUtils.GetAppSetting<string>(AppSettings.SyncAllDocuments_cmdLine.ToString()), StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.Info($"Starting Importing all documents");
                        _importManager.SyncAllDocuments();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error("Critical Error during synchronization!", e);
            }
        }

        private static void InstallIoC()
        {
            Container = new WindsorContainer();
            Container.Install(new BLLInstaller());
        }

        private static void InitLogger()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("logging.config"));
            _logger = LogManager.GetLogger(typeof(Program));
        }

        private enum AppSettings
        {
            SearchNewDocumentsForLastWeek_cmdLine,
            SyncAllDocuments_cmdLine
        }
    }
}