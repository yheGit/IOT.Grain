using Autofac;
using KimiReader.Comm;
using KimiReader.Core.Interface;
using KimiReader.Platform;
using KimiReader.Web.Pages;
using NSS.SocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KimiReader.Web.Controls.BLL
{
    public class BusinessProcess
    {
        private static IIM_AgentRelationCore IM_AgentRelationCore;

        public BusinessProcess(IIM_AgentRelationCore _IM_AgentRelationCore)
        {
            IM_AgentRelationCore = _IM_AgentRelationCore;
        }

        /// <summary>
        /// 发送消息给该用户下的R1设备
        /// </summary>
        /// <param name="meadiaId">媒体Id</param>
        /// <param name="vxUserId">微信用户</param>
        public static void SendMessage(string meadiaId, string vxUserId)
        {
            //139.219.128.86:8864问吧
            //string host = System.IO.File.ReadAllText(@"E:\wwwroot\KimiReader.UdpWinStart\bin\NLog\krNlog.txt");
            string mid = GetMachineId(vxUserId);
            if (string.IsNullOrEmpty(mid))
            {
                CreateLog("GetMachineId", "---[微信ID:" + vxUserId + "||提示：该用户未绑定任何机器，或者已经与机器解绑！]", true);
            }

            var info = IocConfig.Containers.Resolve<IIM_AgentRelationCore>().UserAgent_R(mid);// IM_AgentRelationCore.UserAgent_R(mid);
            if (info != null)
            {
                var host = info.MachineIp;
                var remote = host.Split(':');
                var ip = remote[0];
                var port =8810;
                // 测试用，正式发布，则删除
                if (AppConfig.PortDebug == true)
                    port = Utils.StrToInt(remote[1], 0);
                var atoken = VxinUtils.GetAccess_token(false);
                var msgId = Utils.GetNewId(1);
                var msg = "{\"id\":\"" + msgId + "\",\"msgConn\":\"" + meadiaId + "\",\"msgType\":1,\"timeStamp\":\"" + DateTime.Now.ToString() + "\",\"access_token\":\"" + atoken + "\"}";

                SendMessageToClient.SendToByIp(ip, port, msg);
                CreateLog("UserAgent_R", "[微信ID:" + vxUserId + "||机器ID:" + mid + "||IP:" + info.MachineIp + "]", true);
            }
            else
            { //R1不在线

                //这里可以异步处理
                var reInt = IocConfig.Containers.Resolve<IIM_MachineMsgCore>().MachineMsg_C(meadiaId, mid);
                if (reInt <= 0)
                    CreateLog("UserAgent_R", "[消息ID:" + meadiaId + "||机器ID:" + mid + "||消息存储失败]", true);
            }



        }

        /// <summary>
        /// 通过微信账号获取对应的机器ID
        /// </summary>
        /// <param name="vxUserId">微信账号</param>
        /// <returns></returns>
        public static string GetMachineId(string vxUserId)
        {
            //通过openid到库里匹配用户
            var model = IocConfig.Containers.Resolve<IKR_BindingCore>().Get(p => p.OpenId.Equals(vxUserId));
            if (model == null)
                return "";
            return model.MachineId;

        }



        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="_filename"></param>
        /// <param name="_msg"></param>
        /// <param name="_append">false 覆盖</param>
        public static void CreateLog(string _filename, string _msg, bool _append)
        {
            Utils.WriteLog(_filename, _msg, _append);
        }

        /// <summary>
        /// 打印异常日志
        /// </summary>
        /// <param name="exStr"></param>
        /// <param name="method">方法名</param>
        public static void CreateExLog(Exception exStr, string method)
        {
            Utils.ExceptionLog(exStr, method);
        }


    }
}