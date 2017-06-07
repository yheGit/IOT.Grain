using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net66.Comm.vxin
{
    public class Signature
    {
        public static bool ToCheckSignature(string signature, string timestamp, string nonce, string token)
        {
            List<string> list = new List<string>();
            list.Add(token);
            list.Add(timestamp);
            list.Add(nonce);
            //排序
            list.Sort();
            //拼串
            string input = string.Empty;
            foreach (var item in list)
            {
                input += item;
            }
            //加密
            string new_signature = SecurityUtility.SHA1Encrypt(input);
            //验证
            if (new_signature == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
