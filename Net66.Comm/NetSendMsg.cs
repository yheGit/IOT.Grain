using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Net66.Comm
{
    public class NetSendMsg
    {
        private static string MsgApiUsl = Utils.GetMsgApiUrl;
        private static HttpWebRequest httpWReq;
        private static HttpWebResponse httpWResp;
        private static CookieContainer cookiecn = null;

        public static CookieContainer Cookiecon
        {
            get
            {
                if (cookiecn == null)
                {
                    cookiecn = new CookieContainer();
                }
                return cookiecn;
            }
        }

        //发送短信
        public static string DirectSend(string Phones, string Content, int SendType = 1, string SendTime = "", string PostFixNumber = "")
        {
            var userID = Utils.GetSendUserID;
            var account = Utils.GetSendAccount;
            var password = Utils.GetSendPassword;
            var signature = Utils.GetSignature;
            if (string.IsNullOrEmpty(MsgApiUsl) || string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
                return "1009";
            Content += signature;
            string rtustr = "";
            string posurl = "DirectSend?UserID=" + userID + "&Account=" + account + "&Password=" + password + "&Phones=" + Phones + "&Content=" + Content + "&SendTime=" + SendTime + "&SendType=" + SendType + "&PostFixNumber=" + PostFixNumber;
            MsgApiUsl += posurl;
            string RtuContent = WebHelper.Get(MsgApiUsl, ""); //GetWebResponse(MsgApiUsl);
            if (RtuContent != "")
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(RtuContent);
                XmlElement root = doc.DocumentElement;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("sms", "JobSendedDescription");
                XmlNode ErrorMessage = doc.SelectSingleNode("//sms:Message", nsmgr);
                XmlNode SendFlag = doc.SelectSingleNode("//sms:RetCode", nsmgr);
                if (SendFlag.InnerText == "Sucess")
                {
                    
                    rtustr = "发送成功!";
                }
                else
                {
                    rtustr = "发送失败!" + ErrorMessage.InnerText;
                }
            }
            else
            {
                rtustr = "Fail，ServerErr";
            }
            return rtustr;
        }


        /// <summary>
        /// 提交一个URL至服务器上运行
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string GetWebResponse(string uri)
        {
            string strResp = "";
            var httpWReq = (HttpWebRequest)WebRequest.Create(uri);
            httpWReq.CookieContainer = Cookiecon;
            var httpWResp = (HttpWebResponse)httpWReq.GetResponse();
            using (StreamReader reader = new StreamReader(httpWResp.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")))
            {
                strResp = reader.ReadToEnd();
            }
            httpWResp.Close();
            return strResp;
        }
    }

}
