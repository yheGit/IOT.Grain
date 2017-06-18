using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net66.Comm.vxin
{
   public class VxinMsg
    {
        public string touser { get; set; }

        public string msgtype { get; set; }

        public MsgText text { get; set; }

    }

    public class MsgText
    {
        public MsgText() { }
        public MsgText(string conn)
        {
            content = conn;
        }
        public string content { get; set; }
    }


    //{
    //    "touser": "oCSYtvwSMD5vntlVy3LyKSyAMLgs", 
    //    "msgtype": "text", 
    //    "text": {
    //        "content": "Hello World666666"
    //    }
    //}


}
