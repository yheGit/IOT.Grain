using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Comm
{
    /// <summary>
    /// web请求类 by yhw96160 2017-1-3 10:35:54
    /// </summary>
    public class WebHelper
    {
        public static string Get(string method, string ele)
        {
            try
            {
                var url = string.Format("{0}{1}", method, ele);
                using (var wl = new WebClient())
                {
                    wl.Headers.Add(HttpRequestHeader.Accept, "json");
                    wl.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
                    wl.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0");
                    wl.Encoding = Encoding.UTF8;
                    return wl.DownloadString(url);
                }
            }
            catch (Exception ex)
            {     
                var msg = string.Format("WebHelp---Get异常消息：{0}，Source：{1}，TargetSite：{2}", ex.Message, ex.Source, ex.TargetSite.ToString());
                //Utils.WriteLog("EXCE", GetMapPath("/Log/"), msg, LogOpration.Default);
                return "";
            }
        }

        public static string Post(string method, string ele)
        {
            try
            {
                var url = method;
                using (var wl = new WebClient())
                {
                    wl.Headers.Add(HttpRequestHeader.Accept, "json");
                    wl.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
                    wl.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0");
                    wl.Encoding = Encoding.UTF8;
                    return wl.UploadString(url, "POST", ele);
                }
            }
            catch (Exception ex)
            {
                var msg = string.Format("WebHelp---Post异常消息：{0}。Source：{1}。TargetSite：{2}。InnerException：{3}", ex.Message, ex.Source, ex.TargetSite.ToString(), ex.InnerException);
                //Utils.WriteLog(GetMapPath("/Log/"), msg + "");
                return msg;
                // return "";
            }
        }
    }
}
