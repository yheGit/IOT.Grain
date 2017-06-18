using Net66.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Net66.Service.Controllers
{
    public class CommonController : ApiController
    {
        // GET: Common
        public string Index()
        {
            return "hello developer";
        }

        public void SendMsg()
        {
            CommonCore.DelayedSendingMsg();
        }

    }
}