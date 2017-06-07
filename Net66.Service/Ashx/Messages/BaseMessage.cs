using Net66.Comm.vxin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Net66.Service.Ashx.Messages
{
    public class BaseMessage : Message
    {
        /// <summary>
        /// 媒体体id
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaseMessage()
        {

        }

        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static BaseMessage LoadFromXml(string xml)
        {
            BaseMessage tm = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    tm = new BaseMessage();
                    tm.FromUserName = element.Element(Helper.FROM_USERNAME).Value;
                    tm.ToUserName = element.Element(Helper.TO_USERNAME).Value;
                    tm.CreateTime = element.Element(Helper.CREATE_TIME).Value;
                    tm.EventKey = element.Element("EventKey").Value ;
                }
            }

            return tm;
        }


        private static string GetEle(XElement ele,string name)
        {
            try
            {
                return ele.Element(name).Value;
            }
            catch
            {
                return "";
            }
        }



    }
}
