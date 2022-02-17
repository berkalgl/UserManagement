using log4net;
using log4net.Config;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using UserManagement.Domain;

namespace UserManagement.WebApi.Extensions
{
    public class Bootstrapper
    {
        private readonly IUnityContainer _container;
        public Bootstrapper()
        {
            _container = BuildUnityContainer();
        }
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            ConfigureLog4net(container);
            return container;
        }
        public void Initialize(HttpConfiguration config)
        {
            config.DependencyResolver = new UnityDependencyResolver(_container);

            #region Security Protocol settings
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                //Add SecurityProtocolToEnable and SecurityProtocolToDisable to AppSettings to change protocol configuration on runtime
                string SecurityProtocolToEnable = ConfigurationManager.AppSettings["SecurityProtocolToEnable"] ?? "";
                string SecurityProtocolToDisable = ConfigurationManager.AppSettings["SecurityProtocolToDisable"] ?? "";

                foreach (var protocol in SecurityProtocolToDisable.Split(';').Where(w => w != null && w.Length > 0).ToList())
                {
                    int pi = -1;
                    if (int.TryParse(protocol, out pi) || (pi = Convert.ToInt32(protocol.Replace("0x", ""), 16)) > 1)
                    {
                        SecurityProtocolType p = (SecurityProtocolType)pi;
                        ServicePointManager.SecurityProtocol &= ~p;
                    }
                }

                foreach (var protocol in SecurityProtocolToEnable.Replace("0x", "").Split(';').Where(w => w != null && w.Length > 0).ToList())
                {
                    int pi = -1;
                    if (int.TryParse(protocol, out pi) || (pi = Convert.ToInt32(protocol.Replace("0x", ""), 16)) > 1)
                    {
                        SecurityProtocolType p = (SecurityProtocolType)pi;
                        ServicePointManager.SecurityProtocol |= p;
                    }

                }
            }
            catch (Exception ex)
            {
                var unhandledExceptionLog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
                unhandledExceptionLog.Error(string.Format(
                                            "UnHandled Exception for Url {0}",
                                             string.Empty),
                                            new Exception(
                                                $"Solution.WebApi.Bootstraper.Initialize, could not change ServicePointManager.SecurityProtocol with values from config file. Current value : {ServicePointManager.SecurityProtocol.ToString()}", ex)
                                            );
            }
            #endregion
        }
        public static void RegisterTypes(IUnityContainer container)
        {
            //container
            //    .RegisterDomainTypes()
            //    ;

        }
        private static void ConfigureLog4net(IUnityContainer container)
        {
            XmlConfigurator.Configure();
            var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            container.RegisterInstance(logger);
        }
    }
}
