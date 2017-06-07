using Net66.Comm;
using Net66.Comm.vxin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Net66.Service.Ashx
{
    /// <summary>
    /// WebInit 的摘要说明
    /// </summary>
    public class WebInit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var msg = GetRequest(context);
            //Utils.SetGzip(context);
            //context.Response.Clear();
            //context.Response.Charset = "UTF-8";
            context.Response.Write(msg);
            //context.Response.End();
            //http://kimireader-web.kyd2002.com/Ashx/IotInit.ashx
        }

        private string GetRequest(HttpContext context)
        {
            var method = context.Request.HttpMethod.ToUpper();
            ////验证签名
            //if (method == "GET")
            //{
            //    var signature = Utils.GetString("signature");
            //    var timestamp = Utils.GetString("timestamp");
            //    var nonce = Utils.GetString("nonce");
            //    var token = Helper.WeiXinToken;
            //    if (Signature.ToCheckSignature(signature, timestamp, nonce, token))
            //        return Utils.GetString("echostr");
            //    else
            //        return "error";
            //}
            //处理消息
            if (method == "POST")
                return Controls.Handler.CreateHandler(context.Request);
            else
                return "无法处理";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}