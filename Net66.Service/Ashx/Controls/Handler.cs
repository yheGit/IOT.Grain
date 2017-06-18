using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Comm.vxin;
using Net66.Core.SysSecCore;
using Net66.Service.Ashx.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Net66.Service.Ashx.Controls
{
    public class Handler
    {
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <returns></returns>
        public static string CreateHandler(HttpRequest request)
        {
            //var userip= request.UserHostAddress;
            //Utils.WriteLog("EventHandler-ip", userip);
            var requestStr = Utils.ReadRequest(request);
            if (string.IsNullOrEmpty(requestStr))
                return "";
            Utils.WriteLog("EventHandler-vx", requestStr);

            #region //解析数据
            var msgType = string.Empty;
            var handler = "";

            //strXml
            if (requestStr.Contains("</xml>"))
            {              
                var doc = new XmlDocument();
                doc.LoadXml(requestStr);
                var node = doc.SelectSingleNode("/xml/MsgType");      
                if (node == null)
                    return "";                
                var section = node.FirstChild as XmlCDataSection;
                if (section == null)
                    return "";
                msgType = section.Value;
              var bm= BaseMessage.LoadFromXml(requestStr);
              var eventkey=bm.EventKey;

            }
            else//strJson
            {
                var jsonModel = JsonMessage.GetJsonValue(requestStr, "msg_type");
                if (jsonModel == null)
                    return "";
                msgType = jsonModel.ToString();
            }
            #endregion

            switch (msgType)
            {               
                case "bind":
                    handler = EventHandler1(requestStr);
                    break;
                case "voice":
                    handler = EventHandler2(requestStr);
                    break;
            }

            return handler;
        }


      

        /// <summary>
        /// 处理绑定事件
        /// </summary>
        /// <param name="requestJson"></param>
        /// <returns></returns>
        private static string EventHandler1(string requestJson)
        {
            ////{"device_id":"devid123","device_type":"gh_804b4166f015","msg_id":73314806,
            ////"msg_type":"bind","create_time":1465717137,"open_id":"oCSYtv2gY-fMXu3GMAZ50wVMSzkE",
            ////"session_id":0,"content":"","qrcode_suffix_data":""}
            //var eventToken = JsonMessage.GetJsonValue(requestJson, "msg_type").ToString();
            //var deviceId = JsonMessage.GetJsonValue(requestJson, "device_id").ToString();
            //var openId = JsonMessage.GetJsonValue(requestJson, "open_id").ToString();
            var response = "";

            ////硬件绑定
            //if (eventToken.ToLower().Equals("bind"))
            //{
            //    BindText.GetBindingText(deviceId, openId);
            //}
            if (true)
            {
               var rebit= new UserVxinInfoCore().AddVxin(new UserVxinInfo() {
                    UserLoginID = "13488091880",
                    XvinID = "openid",
                    SendTime=DateTime.Now
                });
            }

           
            return response;
        }

        /// <summary>
        /// 处理多媒体事件
        /// </summary>
        /// <param name="requestXml"></param>
        /// <returns></returns>
        private static string EventHandler2(string requestXml)
        {
            var response = "";
            //var mm = MultimediaMessage.LoadFromXml(requestXml);

           
            //var meadiaId = mm.MediaId;
            //var vxUserId = mm.FromUserName;
            //try
            //{                
            //    BusinessProcess.SendMessage(meadiaId, vxUserId);                
            //}
            //catch (Exception ex)
            //{
            //    Utils.WriteLog("EX2016", "ex.Message:" + ex.Message + "---ex.InnerException" + ex.InnerException.Message);
            //    return response;
            //}

            return response;
        }


    }
}