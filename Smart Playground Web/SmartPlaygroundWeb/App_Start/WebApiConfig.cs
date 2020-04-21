using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SmartPlaygroundWeb.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Attribute Routing
            // Allows for custom routing to be set at the method, example [Route("api/warehouse/scheduling/all")]
            config.MapHttpAttributeRoutes();

            // Convention-Based Routing
            // Allows for standard controller based routing, example api/warehouse, api/warehouse/1
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}