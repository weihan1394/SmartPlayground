using SmartPlaygroundWeb.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace SmartPlaygroundWeb
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Here are settings to change the default JSON.net Json formatting to indented, many other options to choose.
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            // WebApi Routing, loads up your Web API routes stored in App_Start/WebApiConfig.cs
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}