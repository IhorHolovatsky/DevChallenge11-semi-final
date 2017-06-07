using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NewsMonitoringSystem.API.Factory;
using NewsMonitoringSystem.Data;
using NewsMonitoringSystem.IoC;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;

namespace NewsMonitoringSystem.API
{
    public class Global : System.Web.HttpApplication
    {
        internal static WindsorContainer Container { get; private set; }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            InstallIoC();
            InitLogger();

            //Bootstrap Custom Controller factory (Windsor Implementation)
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), 
                                                               Container.Resolve<IHttpControllerActivator>());

            //Send responses as JSON
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }

        private void InstallIoC()
        {
            Container = new WindsorContainer();
            Container.Install(new BLLInstaller());

            Container.Register(Component.For<IHttpControllerActivator>().Instance(new WindsorCompositionRoot(Container)));
            Container.Register(Classes.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient());
        }

        private static void InitLogger()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("logging.config"));
        }
    }
}
