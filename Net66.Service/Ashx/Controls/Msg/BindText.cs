using Autofac;
using KimiReader.Comm;
using KimiReader.Core.Interface;
using KimiReader.Platform.Messages;
using KimiReader.Web.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KimiReader.Web.Controls.Msg
{
    public class BindText
    {
        public static string GetBindingText(string deviceid,string openid)
        {

            var result = IocConfig.Containers.Resolve<IKR_BindingCore>().Binding(deviceid, openid);

            Utils.WriteLog("SubscribeText", "FromUserName---" + deviceid + "----result:" + result);
            var context = "";

            if (result == true)
            {
                context = "成功绑定设备！";

            }
            else
            {
                context = "绑定失败，请尝试重新扫描二维进行绑定！";
                Utils.WriteLog("SubscribeText", "FromUserName---" + deviceid + "----result:" + result + "---" + context);
            }

            return "";
        }


        public static string GetBindingText(IotEventMessage em)
        {

            var result = IocConfig.Containers.Resolve<IKR_BindingCore>().Binding(em.DeviceID, em.OpenID);

            Utils.WriteLog("SubscribeText", "FromUserName---" + em.FromUserName + "----result:" + result + "----stuName");
            var context = "";

            if (result == true)
            {
                context = "成功绑定设备！";
            
            }
            else
                context = "绑定失败，请尝试重新扫描二维进行绑定！";

            context = Utils.ToBase64String(context);
            var msg = string.Format(MsgUtils.IotBackMsg, em.FromUserName, em.ToUserName, Utils.GetNowTime(), "device_text", em.DeviceType, em.DeviceID,em.SessionID, context);

            return msg;
        }


        public static string GetUnBindingText(string openid)
        {
            var result = IocConfig.Containers.Resolve<IKR_BindingCore>().UnBinding(openid);      
            return "";
        }

        public static string GetUnBindingText(IotEventMessage em)
        {
            var result = IocConfig.Containers.Resolve<IKR_BindingCore>().UnBinding(em.FromUserName);//em.OpenID
            var context = Utils.ToBase64String("");
            var msg = string.Format(MsgUtils.IotBackMsg, em.FromUserName, em.ToUserName, Utils.GetNowTime(), "device_text", em.DeviceType, em.DeviceID, em.SessionID, context);
            return msg;
        }

    }
}