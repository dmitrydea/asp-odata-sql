using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net.Http.Formatting;
namespace Service_API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.AddQueryStringMapping("$format", "json", "application/json");
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.AddQueryStringMapping("$format", "xml", "application/xml");
            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
