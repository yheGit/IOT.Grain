using Net66.Comm.vxin;
using Net66.Service.Ashx.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KimiReader.Platform.Messages
{
    public class IotEventMessage : Message
    {
        private const string EVENT = "Event";
        private const string EVENT_KEY = "EventKey";
        private static string mTemplate;

        /// <summary>
        /// 模板
        /// </summary>
        public override string Template
        {
            get
            {
                if (string.IsNullOrEmpty(mTemplate))
                {
                    mTemplate = @"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[event]]></MsgType>
                                <DeviceType><![CDATA[{3}]]></DeviceType>
                                <DeviceID><![CDATA[{4}]]></DeviceID>                                
                                <SessionID>{5}</SessionID>
                                <Content><![CDATA[{6}]]></Content>
                            </xml>";
                }
                return mTemplate;
            }
        }

        public string Event { get; set; }


        /// <summary>     
        /// </summary>
        public string DeviceType { get; set; }

        /// <summary>     
        /// </summary>
        public string DeviceID { get; set; }

        public string Content { get; set; }

        public string SessionID { get; set; }

        public string MsgID { get; set; }

        public string OpenID { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public IotEventMessage()
        {
            this.MsgType = "event";
        }
        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static IotEventMessage LoadFromXml(string xml)
        {
            IotEventMessage em = null;

            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    em = new IotEventMessage();
                    em.FromUserName = element.Element(Helper.FROM_USERNAME).Value;
                    em.ToUserName = element.Element(Helper.TO_USERNAME).Value;
                    em.CreateTime = element.Element(Helper.CREATE_TIME).Value;
                    em.MsgType = element.Element("MsgType").Value;
                    em.Event = element.Element(EVENT).Value;
                    //em.DeviceType = element.Element("DeviceType").Value;

                    //em.DeviceID = element.Element("DeviceID").Value;
                    //em.Content = element.Element("Content").Value;
                    //em.SessionID = element.Element("SessionID").Value;
                    //em.MsgID = element.Element("MsgID").Value;
                    //em.OpenID = element.Element("OpenID").Value;

                }
            }

            return em;
        }
    }
}
