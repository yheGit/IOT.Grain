using Net66.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers
{
    public class CommonController : ApiController
    {
        // GET: Common
        [HttpGet]
        public string Index()
        {
            return "hello developer";
        }

        [HttpGet]
        public void SendMsg()
        {
            CommonCore.DelayedSendingMsg();
        }

    }
}