using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LMSAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);

            //TEST
            //GlobalFilters.Filters.Add(new ThrowOnErrorActionFilter());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //FilterConfig.RegisterGlobalFilters();

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            //Json formatter            
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter
                        .SerializerSettings
                        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //config.Filters.Add(new ThrowOnErrorActionFilter());



            //Load nHibernate Factory
            Adevico.WebAPI.Helper.ContextHelper.FactoryGet();
        }
    }
}
