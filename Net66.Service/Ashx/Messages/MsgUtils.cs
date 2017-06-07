using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net66.Service.Ashx.Messages
{
    public class MsgUtils
    {
        public static string IotBackMsg = @"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[{3}]]></MsgType>
                                <DeviceType><![CDATA[{4}]]></DeviceType>
                                <DeviceID><![CDATA[{5}]]></DeviceID>                                
                                <SessionID>{6}</SessionID>
                                <Content><![CDATA[{7}]]></Content>
                            </xml>";

        public static string IotBackCommMsg = @"<xml>
                                <ToUserName><![CDATA[{0}]]></ToUserName>
                                <FromUserName><![CDATA[{1}]]></FromUserName>
                                <CreateTime>{2}</CreateTime>
                                <MsgType><![CDATA[{3}]]></MsgType>
                                <MediaId><![CDATA[{4}]]></MediaId>
                                <Format><![CDATA[{5}]]></Format>
                                <Recognition><![CDATA[腾讯微信团队]]></Recognition>
                                <MsgId>{6}</MsgId>
                           </xml>";
        

    }
}
