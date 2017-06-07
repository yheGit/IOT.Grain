using Net66.Comm.vxin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Net66.Service.Ashx.Messages
{
    public class MultimediaMessage : Message
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 媒体体id
        /// </summary>
        public string MediaId { get; set; }


        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format { get; set; }


        /// <summary>
        /// 消息ID
        /// </summary>
        public string MsgId { get; set; }



        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MultimediaMessage()
        {
        }


        /// <summary>
        /// 从xml数据加载文本消息
        /// </summary>
        /// <param name="xml"></param>
        public static MultimediaMessage LoadFromXml(string xml)
        {
            MultimediaMessage tm = null;
            if (!string.IsNullOrEmpty(xml))
            {
                XElement element = XElement.Parse(xml);
                if (element != null)
                {
                    tm = new MultimediaMessage();
                    tm.FromUserName = element.Element(Helper.FROM_USERNAME).Value;
                    tm.ToUserName = element.Element(Helper.TO_USERNAME).Value;
                    tm.CreateTime = element.Element(Helper.CREATE_TIME).Value;                   
                    tm.MsgId = element.Element(Helper.MSG_ID).Value;
                    tm.PicUrl = GetEle(element, "PicUrl");
                    tm.MediaId=GetEle(element, "MediaId");
                    tm.Format = GetEle(element, "Format");
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
