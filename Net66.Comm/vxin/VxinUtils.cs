
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Net66.Comm.vxin
{
    public class VxinUtils
    {
        //发消息给微信用户
        public static void SendMsgToVxUser(string _json)
        {
            var access_token = GetAccess_token(false);
            var url = Helper.SendMsg_Url + access_token;
            var result = WebHelper.Post(url, _json);
            var isCreateToken = Utils.GetWxinQue(result);
            if (isCreateToken == true)
            {
                GetAccess_token(true);
                SendMsgToVxUser(_json);

                //VxinMsg =>  _json
            }
        }

        public static string GetAccess_token()
        {
            string access_token = "";
            int tokenTime = 0;
            string strjson = WebHelper.Get(Helper.Access_token_URL, "");
            if (!string.IsNullOrEmpty(strjson))
            {
                var token = JObject.Parse(strjson).SelectToken("access_token");
                tokenTime = TypeParse.StrToInt(JObject.Parse(strjson).SelectToken("expires_in"), 0);//7200秒
                if (token != null)
                    access_token = token.ToString();
            }
            tokenTime = tokenTime - 360;
            if (string.IsNullOrEmpty(access_token) || tokenTime <= 0)
                return "";
            return access_token;

        }

        /// <summary>
        /// 获取AcessToken
        /// </summary>
        /// <param name="isNewCreate">false只获取；true获取不到，重现创建</param>
        /// <returns></returns>
        public static string GetAccess_token(bool isNewCreate)
        {
            var access_token = "";
            if (isNewCreate == false)
            {
                if (!string.IsNullOrEmpty(access_token))
                    return access_token;
            }

            using (var wl = new WebClient())
            {
                wl.Headers.Add(HttpRequestHeader.Accept, "json");
                wl.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
                wl.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0");
                wl.Encoding = Encoding.UTF8;
                access_token = wl.DownloadString(Helper.Access_token_URL);
            }
            var tokenTime = 0;
            if (!string.IsNullOrEmpty(access_token))
            {
                var token = JObject.Parse(access_token).SelectToken("access_token");
                tokenTime = TypeParse.StrToInt(JObject.Parse(access_token).SelectToken("expires_in"), 0);

                if (token != null)
                    access_token = token.ToString();
            }
            tokenTime = tokenTime - 360;
            if (string.IsNullOrEmpty(access_token) || tokenTime <= 0)
                return "";
            return access_token;
        }

        public static bool GetMenuToken()
        {
            try
            {
                var access_token = GetAccess_token(false);
                var url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=";
                var result = "";
                using (var wl = new WebClient())
                {
                    wl.Headers.Add(HttpRequestHeader.Accept, "json");
                    wl.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
                    wl.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0");
                    wl.Encoding = Encoding.UTF8;
                    result = wl.DownloadString(url + access_token);
                }
                if (string.IsNullOrEmpty(result))
                {
                    return false;
                }
                var error = JObject.Parse(result).SelectToken("errcode").ToString();
                if (error.Equals("40001") || error.Equals("42001"))
                    GetAccess_token(true);
                return true;
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "GetMenuToken获取二维码失败:" + ex.Message.ToString());
                return false;
            }
        }

        public static string GetOpenID(string code)
        {
            var openid = "";
            using (var wl = new WebClient())
            {
                wl.Headers.Add(HttpRequestHeader.Accept, "json");
                wl.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
                wl.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0");
                wl.Encoding = Encoding.UTF8;
                openid = wl.DownloadString(Helper.Web_Access_token_URL + code);
            }
            if (!string.IsNullOrEmpty(openid))
            {
                var token = JObject.Parse(openid).SelectToken("openid");
                if (token != null)
                    openid = token.ToString();
            }
            return openid;
        }


        public static string GetQrcode(string ele, out int expire_seconds)
        {
            try
            {
                expire_seconds = 0;
                var json = "{\"expire_seconds\": 604800, \"action_name\": \"QR_SCENE\", \"action_info\":{\"scene\": {\"scene_id\": " + ele + "}}}";
                var access_token = GetAccess_token(false);
                var result = "";
                using (var wl = new WebClient())
                {
                    wl.Headers.Add(HttpRequestHeader.Accept, "json");
                    wl.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
                    wl.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0");
                    wl.Encoding = Encoding.UTF8;
                    result = wl.UploadString(Helper.CreateQrcode_URL + access_token, "POST", json);
                }
                if (string.IsNullOrEmpty(result))
                {
                    return "";
                }

                var url = JObject.Parse(result).SelectToken("url");
                expire_seconds = TypeParse.StrToInt(JObject.Parse(result).SelectToken("expire_seconds"), 0);
                if (url == null)
                {
                    //如果是access_token超时，则重新去获取一次
                    var error = JObject.Parse(result).SelectToken("errcode").ToString();
                    if (error.Equals("40001") || error.Equals("42001"))
                        GetAccess_token(true);

                    Utils.PrintLog("VxinUtils" + ele + "获取二维码失败,GetQrcode方法内的ticket为空,result：" + result + ",access_token：" + access_token);
                    return "";
                }
                return url.ToString();
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "VxinUtils" + ele + "获取二维码失败:" + ex.Message.ToString());
                expire_seconds = 0;
                return "";
            }
        }

        public static string SendMsg(string type, string openId, string context)
        {
            var contextKey = "media_id";
            if (string.IsNullOrEmpty(type))
            {
                type = "text";
                contextKey = "content";
            }

            var json = "{\"touser\":\"" + openId + "\",\"msgtype\":\"" + type + "\",\"" + type + "\":{\"" + contextKey + "\":\"" + context + "\"}}";
            var access_token = GetAccess_token(false);
            using (var wl = new WebClient())
            {
                wl.Headers.Add(HttpRequestHeader.Accept, "json");
                wl.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
                wl.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0");
                wl.Encoding = Encoding.UTF8;
                var result = wl.UploadString(Helper.SendMsg_Url + access_token, json);

                var isCreateToken = Utils.GetWxinQue(result);
                if (isCreateToken == true)
                    GetAccess_token(true);
            }
            return "ok";
        }


        public static string GetUserInfo(string[] openIds)
        {
            var access_token = GetAccess_token(false);
            var strBuilder = new StringBuilder();
            strBuilder.Append("{\"user_list\": [");
            foreach (var m in openIds)
            {
                strBuilder.Append("{\"openid\": \"" + m + "\",\"lang\": \"zh-CN\"},");
            }
            strBuilder.Append("]}");
            var json = strBuilder.ToString().Replace("},]", "}]");
            using (var wl = new WebClient())
            {
                wl.Headers.Add(HttpRequestHeader.Accept, "json");
                wl.Headers.Add(HttpRequestHeader.ContentType, "application/json;charset=UTF-8");
                wl.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0");
                wl.Encoding = Encoding.UTF8;
                var result = wl.UploadString(Helper.GetUserInfo_Url + access_token, json);

                var isCreateToken = Utils.GetWxinQue(result);
                if (isCreateToken == true)
                    GetAccess_token(true);
                return result;
            }
        }

    }
}
