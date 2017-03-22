using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net66.Comm
{
    public class Verification
    {
        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="serKey"></param>
        /// <param name="mobiKey"></param>
        /// <returns></returns>
        public static bool Authorization(string serKey, string mobiKey)
        {
            if (string.IsNullOrEmpty(serKey) || string.IsNullOrEmpty(mobiKey))
            {
                return false;
            }
            bool result;
            try
            {
                string[] array = mobiKey.Split(new char[]
                {
                    '-'
                });
                if (array.Length != 2)
                {
                    result = false;
                }
                else
                {
                    string text = array[0];
                    if (string.IsNullOrEmpty(text))
                    {
                        result = false;
                    }
                    else
                    {
                        string text2 = array[1].ToLower();
                        if (string.IsNullOrEmpty(text2))
                        {
                            result = false;
                        }
                        else
                        {
                            string str = Utils.MD5(Utils.MD5(serKey + "-" + text));
                            string value = Utils.MD5(serKey + "-" + str).ToLower();
                            if (!text2.Equals(value))
                            {
                                result = false;
                            }
                            else
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 用默认的SerKey验证token
        /// </summary>
        /// <param name="mobiKey"></param>
        /// <returns></returns>
        public static bool Authorization(string mobiKey)
        {
            return Authorization(Utils.GetMobiAppKey, mobiKey);
        }

        /// <summary>
        /// 生成一个token
        /// </summary>
        /// <param name="serKey"></param>
        /// <param name="clientKey"></param>
        /// <returns></returns>
        public static string EnAuthorization(string serKey, string clientKey)
        {
            if (string.IsNullOrEmpty(serKey) || string.IsNullOrEmpty(clientKey))
            {
                return "";
            }
            string str = Utils.MD5(Utils.MD5(serKey + "-" + clientKey));
            string str2 = Utils.MD5(serKey + "-" + str);
            return clientKey + "-" + str2;
        }
        
    }
}
