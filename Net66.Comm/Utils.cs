using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;


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


        #region  配置信息AppSettings


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

        public static string GetServiceAppKeyToken
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["KWatchAppKey"].ToString();
                }
                catch
                {
                    return "";
                }
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


        #endregion

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
                using (StreamWriter streamWriter = new StreamWriter(file, true))
                {
                    streamWriter.WriteLine("记录时间：" + DateTime.Now.ToString());
                    streamWriter.Write(msg);
                    streamWriter.WriteLine();
                    streamWriter.WriteLine("*****************************************************");
                    streamWriter.Close();
                }
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

    }
}
