using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO.Compression;


/******************************************
*Creater:yhw[]
*CreatTime:2016-11-09 14:47:37
*Description:
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:
*******************************************/
namespace Net66.Comm
{
    public class Utils
    {

        /// <summary>
        /// 有序连接字符串 by yhw 2017-3-27 11:46:58
        /// </summary>
        public static string StrSequenConcat(params string[] args)
        {
            return string.Concat(args);
        }

        /// <summary>
        /// 获取集合中指定Key至的Value by 2017-3-13 14:56:25
        /// </summary>
        /// <returns></returns>
        public static string GetValue(List<string> _params, string _key)
        {
            if (!_params.Exists(e=>e.Contains(_key))) return string.Empty;
            var objStr = _params.Find(f => f.Contains(_key));
            if (string.IsNullOrEmpty(objStr)) return string.Empty;
            return objStr.Split('^')[1];
        }

        public static void Test(Dictionary<string, string> select)
        {
            string str4 = !select.ContainsKey("Style") || string.IsNullOrEmpty(select["Style"].ToString()) ? "2" : select["Style"].ToString();
        }

        /// <summary>
        /// 获取字符串类型的主键
        /// </summary>
        /// <returns></returns>
        public static string GetNewId(int type = 0)
        {
            if (type == 1)
                return CreateNewId();
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 创建不重复的Id
        /// </summary>
        /// <returns></returns>
        private static string CreateNewId()
        {
            string id = DateTime.Now.ToString("yyyyMMddHHmmssfffffff");
            string guid = Guid.NewGuid().ToString().Replace("-", "");

            id += guid.Substring(0, 10);
            return id;
        }

        /// <summary>
        /// 订单号 by yhw 2016-11-16 15:15:33
        /// </summary>
        /// <param name="_pre">订单前缀</param>
        /// <returns></returns>
        public static string GetOrderNumber(string _pre = "")
        {
            //if(!string.IsNullOrEmpty(_pre))
            var order = _pre + TypeParse.GetDateString() + Rndstring(6, DateTime.Now.Millisecond);

            return order;
        }

        /// <summary>
        /// 生成n位随机数 by yhw 2016-11-16 15:13:59
        /// </summary>
        /// <param name="n">n不大于9</param>
        /// <returns></returns>
        public static int GetRandNum(int n)
        {
            Random rd = new Random();
            var min = NumemberMin(n);
            int max = NumemberMax(n);
            int randkey = rd.Next(min, min);//rd.Next(100000, 999999);
            return randkey;

            //Math.Pow(4,5).ToString()
        }

        /// <summary>
        /// 取n位数的最小值（n不大于9） by yhw 2016-11-16 15:23:10
        /// </summary>
        /// <param name="n">n>=1</param>
        /// <returns></returns>
        public static int NumemberMin(int n = 1)
        {
            return TypeParse.StrToInt(Math.Pow(10, n - 1).ToString(), 0);//2147483647
        }

        /// <summary>
        /// 取n位数的最大值（n不大于9） by yhw 2016-11-16 15:25:39
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int NumemberMax(int n = 1)
        {
            var strnum = "";
            for (int s = 0; s < n; s++)
                strnum += "9";
            return TypeParse.StrToInt(strnum, 9);

        }

        /// <summary>
        /// 生成随机数字 by yhw 2016-11-16 15:01:38
        /// </summary>
        /// <param name="strlength"></param>
        /// <param name="seed"></param>
        /// <returns></returns>
        public static string Rndstring(int strlength, int seed)
        {
            string strchar = "0,1,2,3,4,5,6,7,8,9";
            string[] arychar = strchar.Split(',');
            string strrandom = "";
            Random rnd = new Random(seed);
            //生成随机字符串 
            for (int i = 0; i < strlength; i++)
            {
                strrandom += arychar[rnd.Next(arychar.Length)];
            }
            return strrandom;
        }

        /// <summary>
        /// 取当前时分 eg:14:24
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentTime()
        {
            return DateTime.Now.ToString("t");
        }

        /// <summary>
        /// 获取服务器时间
        /// 格式：yyyy-MM-dd HH:mm:ss
        /// sql：CONVERT(CHAR(20),GetServerTime(),120)
        /// </summary>
        /// <returns></returns>
        public static string GetServerTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取服务器时间 by yhw96106 2016-11-14 19:29:46
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServerDateTime()
        {
            return Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }


        /// <summary>
        ///  //yyyy/MM/dd,HH:mm:ss	format by yhw 2016-11-14 19:30:28
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string GetServerStrTime(string format)
        {
            return DateTime.Now.ToString(format);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <param name="_type">1一天开始,2一天结束,0默认获取当前时间</param>
        /// <returns></returns>
        public static string GetServerTime(int _type = 0)
        {
            if (_type == 1)
                return DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
            else if (_type == 2)
                return DateTime.Now.ToString("yyyy-MM-dd 23:59:59");
            else
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ConvertToHHmm(string date)
        {
            try
            {
                if (string.IsNullOrEmpty(date))
                    return "";
                var d = DateTime.Parse(date);
                return d.ToString("HH:mm");
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// MD5函数 by yhw 2016-11-10 15:57:52
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// 获取服务器根目录 by yhw 2016-11-10 10:19:23
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string GetMapPath(string strPath)
        {
            if (string.IsNullOrEmpty(strPath))
                return "";
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            strPath = strPath.Replace("/", @"\");
            if (strPath.StartsWith(@"\"))
            {
                strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart(new char[] { '\\' });
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        }

        /// <summary>
        /// 获取当前站点地址(包括端口号) by yhw 2016-11-10 10:28:45
        /// </summary>
        public static string SiteUrl
        {
            set { value = SiteUrl; }
            get
            {
                return HttpContext.Current.Request.Url.Authority;
            }
        }

        /// <summary>
        /// 获取文件保存的路径 by yhw 2016-11-10 15:54:36
        /// </summary>
        public static string GetFileSaveUrl
        {
            get
            {
                string result;
                try
                {
                    result = ConfigurationManager.AppSettings["FileSaveUrl"].ToString();
                }
                catch
                {
                    result = "";
                }
                return result;
            }
        }


        /// <summary>
        /// 获取文件保存的路径 by yhw 2016-11-10 14:11:17
        /// </summary>
        public static string GetFileWebUrl
        {
            get
            {
                string result;
                try
                {
                    result = ConfigurationManager.AppSettings["FileWebUrl"].ToString();
                }
                catch
                {
                    result = "";
                }
                return result;
            }
        }


        /// <summary>
        /// 得到当前时间（整型）（考虑时区）
        /// </summary>
        /// <returns></returns>
        public static string GetNowTime()
        {
            DateTime timeStamp = new DateTime(1970, 1, 1);  //得到1970年的时间戳
            long a = (DateTime.UtcNow.Ticks - timeStamp.Ticks) / 10000000;
            return a.ToString();
        }

        public static void ResponseWrite(string strHtml, bool isEnd)
        {
            HttpContext.Current.Response.Write(strHtml);
            if (isEnd)
            {
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 读取请求对象的内容
        /// 只能读一次
        /// </summary>
        /// <param name="request">HttpRequest对象</param>
        /// <returns>string</returns>
        public static string ReadRequest(HttpRequest request)
        {
            string reqStr = string.Empty;
            using (Stream s = request.InputStream)
            {
                using (StreamReader reader = new StreamReader(s, Encoding.UTF8))
                {
                    reqStr = reader.ReadToEnd();
                }
            }

            return reqStr;
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName);
            }
            else
            {
                return GetQueryString(strName);
            }
        }


        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Form[strName].Trim().Replace("'", "");
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName].Trim().Replace("'", "");
        }


        public static void SetGzip(HttpContext context)
        {
            try
            {
                var acceptEncoding = context.Request.Headers["Accept-Encoding"].ToString().ToUpperInvariant();
                if (!String.IsNullOrEmpty(acceptEncoding))
                {
                    //如果头部里有包含"GZIP”,"DEFLATE",表示你浏览器支持GZIP,DEFLATE压缩
                    if (acceptEncoding.Contains("GZIP"))
                    {
                        //向输出流头部添加压缩信息
                        context.Response.AppendHeader("Content-encoding", "gzip");
                        context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                    }
                    else if (acceptEncoding.Contains("DEFLATE"))
                    {
                        //向输出流头部添加压缩信息
                        context.Response.AppendHeader("Content-encoding", "deflate");
                        context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
                    }
                }
            }
            catch
            {
            }
        }


        #region  配置信息AppSettings

        #region mxt短信

        public static string GetMsgApiUrl
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["MsgApiUrl"].ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        public static string GetSendUserID
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SendUserID"].ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        public static string GetSendAccount
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SendAccount"].ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        public static string GetSendPassword
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SendPassword"].ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        public static string GetSignature
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["Signature"].ToString();
                }
                catch
                {
                    return "";
                }
            }
        }

        #endregion

        /// <summary>
        /// 调试App
        /// </summary>
        public static bool DebugApp
        {
            get
            {
                try
                {
                    var val = ConfigurationManager.AppSettings["Debug"].ToString();
                    if (!string.IsNullOrEmpty(val) && val.ToUpper().Equals("TRUE"))
                        return true;
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static string GetMobiAppKey
        {
            get
            {
                string result;
                try
                {
                    result = ConfigurationManager.AppSettings["MobiAppKey"].ToString();
                }
                catch
                {
                    result = "";
                }
                return result;
            }
        }


        /// <summary>
        /// 项目绝对地址 by yhw 2016-11-10 16:39:55
        /// </summary>
        public static string LocalAddress
        {
            set { value = LocalAddress; }
            get
            {
                return ConfigurationManager.AppSettings["LocalAddress"].ToString();
            }
        }

        public static string ServerAddress
        {
            set { value = ServerAddress; }
            get
            {
                return ConfigurationManager.AppSettings["ServerAddress"].ToString();
            }
        }

        public static string GetMemCachedUrl
        {
            get
            {
                string result;
                try
                {
                    result = ConfigurationManager.AppSettings["MemCachedUrl"].ToString();
                }
                catch
                {
                    result = "";
                }
                return result;
            }
        }

        /// <summary>
        /// 定时
        /// </summary>
        public static int TimingClock
        {
            get
            {
                try
                {
                    var val = ConfigurationManager.AppSettings["TimingClock"].ToString();
                    if (!string.IsNullOrEmpty(val))
                        return TypeParse.StrToInt(val, 3600 * 1000);
                    return 3600*1000;
                }
                catch
                {
                    return 3600 * 1000;
                }
            }
        }


        #endregion


        #region vxin配置


        public static bool GetWxinQue(string result)
        {
            try
            {
                var error = JObject.Parse(result).SelectToken("errcode").ToString();
                if (error.Equals("40001") || error.Equals("42001"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion


        #region 写日志

        public enum LogOpration
        {
            /// <summary>
            /// 根据Web.config中的配置
            /// </summary>
            Default,
            /// <summary>
            /// 开启记录日志
            /// </summary>
            Start,
            /// <summary>
            /// 禁止记录日志
            /// </summary>
            Fobid
        }

        /// <summary>
        /// 写日志 by yhw 2016-11-10 15:58:34
        /// </summary>
        /// <param name="logtype"></param>
        /// <param name="path"></param>
        /// <param name="msg"></param>
        /// <param name="logOpration"></param>
        public static void WriteLog(string logtype, string path, string msg, LogOpration logOpration = LogOpration.Default)
        {
            bool logEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["LogEnabled"]);
            if (logOpration == LogOpration.Fobid || (logOpration == LogOpration.Default && !logEnabled))
                return;
            else if (logOpration == LogOpration.Start || (logOpration == LogOpration.Default && logEnabled))
            {
                path += "\\" + DateTime.Now.ToString("yyyy-MM-dd");
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                string str = logtype + DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + ".txt";
                string file = path + "\\" + str;
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter(file, true))
                    {
                        streamWriter.WriteLine("记录时间：" + DateTime.Now.ToString());
                        streamWriter.Write(msg);
                        streamWriter.WriteLine();
                        streamWriter.WriteLine("*****************************************************");
                        streamWriter.Close();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 随手打印消息 by yhw 2017-3-18 09:11:14
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="methodName"></param>
        public static void PrintLog(string message, string methodName = "未知方法名", string logType = "Log")
        {
            var msg = string.Format("\r\n====================【{0}】\r\n", methodName);
            msg += string.Format("\r\n---msg:{0}", message);
            WriteLog("Exception", LocalAddress + logType, msg);
        }

        /// <summary>
        /// 打印异常消息 by yhw 2016-11-10 16:39:05
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="methodName"></param>
        public static void ExceptionLog(Exception ex, string methodName = "未知方法名")
        {
            var msg = string.Format("\r\n====================【{0}】\r\n", methodName);
            msg += string.Format("异常消息\r\n Message：{0}，\r\n Source：{1}，\r\n TargetSite：{2}，\r\n InnerException：{3}，\r\n StackTrace：{4}",
         ex.Message, ex.Source, ex.TargetSite.ToString(), ex.InnerException, ex.StackTrace);
            if (ex.InnerException != null)
            {
                msg += "\r\n\r\n InnerException.Message：" + ex.InnerException.Message + "\r\n InnerException.StackTrace：" + ex.InnerException.StackTrace;
                if (ex.InnerException.InnerException != null)
                {
                    msg += "\r\n\r\n InnerException.InnerException.Message：" + ex.InnerException.InnerException.Message
                        + "\r\n InnerException.InnerException.StackTrace" + ex.InnerException.InnerException.StackTrace;
                }
            }
            WriteLog("Exception", LocalAddress + "Log", msg);

            //WriteLog("--------------------------------------[本次异常开始]--------------------------------------");  
            //WriteLog("Message : " + e.Message);  
            //WriteLog("Source : " + e.Source);  
            //WriteLog("StackTrace : " + e.StackTrace);  
            //WriteLog("TargetSite : " + e.TargetSite);  
            //WriteLog("--------------------------------------[本次异常结束]--------------------------------------\r\n");  
        }

        public static void WriteLog(string fileName, string msg)
        {
          
            string text = LocalAddress + "Log\\";
            text = text  + DateTime.Now.ToString("yyyy-MM-dd");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "error" + DateTime.Now.Year.ToString().Trim() + DateTime.Now.Month.ToString().Trim() + DateTime.Now.Day.ToString().Trim();
            }
            fileName += ".txt";
            string path = text + "\\" + fileName;
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(path, true))
                {
                    streamWriter.WriteLine("出错时间：" + DateTime.Now.ToString());
                    streamWriter.Write(msg);
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("*****************************************************");
                    streamWriter.Close();
                }
            }
            catch
            {
            }
        }

        #endregion

    }
}
