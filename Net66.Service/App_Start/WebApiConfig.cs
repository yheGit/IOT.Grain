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
               routeTemplate: "api/Granary/GetSensorsChart/{sensorId}/{type}",
               defaults: new { controller = "Granary", actton = "GetSensorsChart", sensorId = RouteParameter.Optional, type = RouteParameter.Optional }
           );

            config.Routes.MapHttpRoute(
              name: "DefaultApi2",
              routeTemplate: "api/Granary/IsExist2/{number}/{type}",
              defaults: new { controller = "Granary", actton = "IsExist2", number = RouteParameter.Optional, type = RouteParameter.Optional }
          );

           config.Routes.MapHttpRoute(
               name: "DefaultApi3",
               routeTemplate: "api/Granary/GetHeapTempChart/{number}/{type}",
               defaults: new { controller = "Granary", actton = "GetHeapTempChart", number = RouteParameter.Optional, type = RouteParameter.Optional }
            );

          config.Routes.MapHttpRoute(
              name: "DefaultApi4",
              routeTemplate: "api/Grain/HeapsTemp_GetList/{number}",
              defaults: new { controller = "Grain", actton = "HeapsTemp_GetList", number = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
           name: "DefaultApi5",
           routeTemplate: "api/MenuRoleOperation/GetOperationData/{RoleID}/{MenuID}",
           defaults: new { controller = "MenuRoleOperation", actton = "GetOperationData", RoleID = RouteParameter.Optional, MenuID = RouteParameter.Optional }
         );




        }
    }
}
