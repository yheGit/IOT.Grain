using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Net66.Comm
{
    /// <summary>
    /// 系统类型扩展
    /// </summary>
    public static class SystemTypeExtension
    {
        /// <summary>
        /// 将字符串转换成Int类型
        /// </summary>
        /// <param name="stringIn"></param>
        /// <returns></returns>
        public static int ToInt(this string stringIn)
        {
            int outInt = 0;
            int.TryParse(stringIn, out outInt);
            return outInt;
        }

        /// <summary>
        /// 将字符串转换成Int类型
        /// </summary>
        /// <param name="stringIn"></param>
        /// <returns></returns>
        public static long ToLong(this string stringIn)
        {
            long outLong = 0;
            long.TryParse(stringIn, out outLong);
            return outLong;
        }

        /// <summary>
        /// 将字符串转换成MD5值
        /// </summary>
        /// <param name="stringIn"></param>
        /// <returns></returns>
        public static string ToMd5(this string stringIn)
        {
            return Utils.MD5(stringIn);
        }

        /// <summary>
        /// 从请求中获取相应参数名的参数值
        /// </summary>
        /// <param name="req"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static int ToInt(this HttpRequest req, string paramName)
        {
            if (req.QueryString[paramName] != null)
            {
                return req.QueryString[paramName].ToInt();
            }
            return 0;
        }
        /// <summary>
        /// 从请求中获取相应参数名的参数值
        /// </summary>
        /// <param name="req"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static int ToInt(this HttpRequest req, int index)
        {
            if (req.QueryString[index] != null)
            {
                return req.QueryString[index].ToInt();
            }
            return 0;
        }
        /// <summary>
        /// 从请求中获取相应参数名的参数值
        /// </summary>
        /// <param name="req"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string ToString(this HttpRequest req, int index)
        {
            if (req.QueryString[index] != null)
            {
                return req.QueryString[index];
            }
            return "";
        }
        /// <summary>
        /// 从请求中获取相应参数名的参数值
        /// </summary>
        /// <param name="req"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string ToString(this HttpRequest req, string paramName)
        {
            if (req.QueryString[paramName] != null)
            {
                return req.QueryString[paramName];
            }
            return "";
        }

        /// <summary>
        /// 将字符串转换成Float
        /// </summary>
        /// <param name="In"></param>
        /// <returns></returns>
        public static float ToFloat(this string In)
        {
            float f = 0.0f;
            float.TryParse(In, out f);
            return f;
        }

        /// <summary>
        /// 将字符串转换成日期
        /// </summary>
        /// <param name="In"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string In)
        {
            DateTime dt = new DateTime();
            DateTime.TryParse(In, out dt);
            return dt;
        }

        /// <summary>
        /// 将对象转换成Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 将对象转换成JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerialiableJson(this object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return "";
            }
        }
    }
}
