using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net66.Comm.vxin
{
    public class JsonMessage
    {
        /// <summary>
        /// 通过key在json中获取对value值
        /// </summary>
        /// <param name="jsonStr">json串</param>
        /// <param name="jsonKey">key值</param>
        /// <returns>返回value值</returns>
        public static JToken GetJsonValue(string jsonStr, string jsonKey)
        {
            return JObject.Parse(jsonStr).SelectToken(jsonKey);
        }

    }
}
