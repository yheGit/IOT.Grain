using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Net66.Service.Base;

namespace Net66.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Filters.Add(new BaseAuthentication());
            // Web API 路由
            //config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
           
            config.Routes.MapHttpRoute(
               name: "DefaultApi1",
               routeTemplate: "api/Grain/IsExist/{_code}",
               defaults: new { controller = "Grain", actton = "IsExist", _code=RouteParameter.Optional }
           );
            config.Routes.MapHttpRoute(
            name: "DefaultApi2",
            routeTemplate: "api/Info/AuthDevice/{machineId}",
            defaults: new { controller = "Info", actton = "AuthDevice", machineId = RouteParameter.Optional }
        );



        }
    }
}
