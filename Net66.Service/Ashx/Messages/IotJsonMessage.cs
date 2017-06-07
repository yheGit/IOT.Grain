using Net66.Comm;
using Net66.Service.Ashx.Messages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KimiReader.Platform.Messages
{
    public class IotJsonMessage : Message
    {

        public string DeviceID { get; set; }
        public string DeviceType { get; set; }
        public int MsgID { get; set; }
        public string msg_type { get; set; }
        public long create_time { get; set; }
        public string OpenID { get; set; }
        public int SessionID { get; set; }
        public string Content { get; set; }
        public string qrcode_suffix_data { get; set; }


        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static IotJsonMessage LoadFromJson(string json)
        {
            IotJsonMessage jm = null;
            if (!string.IsNullOrEmpty(json))
            {

                jm = new IotJsonMessage();
                //jm.FromUserName = element.Element(Helper.FROM_USERNAME).Value;
                //jm.ToUserName = element.Element(Helper.TO_USERNAME).Value;
                //jm.CreateTime = element.Element(Helper.CREATE_TIME).Value;               
                //jm.Event = element.Element(EVENT).Value;
                jm.DeviceType = JObject.Parse(json).SelectToken("device_type").ToString(); 
                jm.MsgType = JObject.Parse(json).SelectToken("MsgType").ToString();
                jm.DeviceID = JObject.Parse(json).SelectToken("device_id").ToString();
                jm.Content = JObject.Parse(json).SelectToken("content").ToString();
                jm.SessionID = TypeParse.StrToInt(JObject.Parse(json).SelectToken("session_id").ToString(), 0);
                jm.MsgID = TypeParse.StrToInt(JObject.Parse(json).SelectToken("msg_id").ToString(), 0);
                jm.OpenID = JObject.Parse(json).SelectToken("open_id").ToString();

            }

            return jm;
        }

    }
}
