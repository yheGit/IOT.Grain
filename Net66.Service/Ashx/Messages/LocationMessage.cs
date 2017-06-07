using Net66.Comm;
using Net66.Comm.vxin;
using Net66.Service.Ashx.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace KimiReader.Platform.Messages
{
    public class LocationMessage : Message
    {

        /// <summary>
        /// 坐标x
        /// </summary>
        public double Location_X { get; set; }
        /// <summary>
        /// 坐标y
        /// </summary>
        public double Location_Y { get; set; }

        /// <summary>
        /// 偏移量
        /// </summary>
        public double Scale { get; set; }

        /// <summary>
        /// 地址内容
        /// </summary>
        public string Label { get; set; }


        /// <summary>
        /// 消息ID
        /// </summary>
        public string MsgId { get; set; }


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public LocationMessage()
        {
        }


        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static LocationMessage LoadFromXml(string xml)
        {
            LocationMessage tm = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    tm = new LocationMessage();
                    tm.FromUserName = element.Element(Helper.FROM_USERNAME).Value;
                    tm.ToUserName = element.Element(Helper.TO_USERNAME).Value;
                    tm.CreateTime = element.Element(Helper.CREATE_TIME).Value;
                    tm.Location_X = TypeParse.StrToDouble(element.Element("Location_X").Value,0);
                    tm.Location_Y = TypeParse.StrToDouble(element.Element("Location_Y").Value, 0);
                    tm.Scale = TypeParse.StrToDouble(element.Element("Scale").Value, 0);
                    tm.Label = element.Element("Label").Value;
                    tm.MsgId = element.Element(Helper.MSG_ID).Value;
                }
            }

            return tm;
        }

    }
}
