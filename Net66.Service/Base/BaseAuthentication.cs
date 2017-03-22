using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Configuration;
using System.Net.Http;
using System.Net;
using Net66.Comm;
using Net66.Entity.IO_Model;

namespace Net66.Service.Base
{
    public class BaseAuthentication : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                string text = ConfigurationManager.AppSettings["MobiAppKey"];
                if (!string.IsNullOrEmpty(text))
                {
                    string text2 = actionContext.Request.Headers.GetValues("AppKey").ToArray<string>()[0].ToString();
                    var flg = Verification.Authorization(text, text2);
                    if (flg == true)
                        return;
                    
                    string text3 = new MobiResult(1014, "", null, "").ToJson();
                    HttpResponseMessage expr_75 = new HttpResponseMessage(HttpStatusCode.NotFound);
                    expr_75.Content = new StringContent(text3);
                    actionContext.Response = expr_75;

                }
            }
            catch
            {
                string text3 = new MobiResult(1014, "", null, "").ToJson();
                HttpResponseMessage expr_75 = new HttpResponseMessage(HttpStatusCode.NotFound);
                expr_75.Content = new StringContent(text3);
                actionContext.Response = expr_75;
            }
        }
    }
}