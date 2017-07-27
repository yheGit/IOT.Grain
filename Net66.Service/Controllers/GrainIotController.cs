﻿using Net66.Comm;
using Net66.Core.Interface;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers
{
    public class GrainIotController : ApiController
    {
        public static ICollectorCore collectorCore;
        public static IReceiverCore receiverCore;
        public static IAlarmCore alarmCore;

        public static IWareHouseCore wareHouseCore;

        public GrainIotController(ICollectorCore _collectorCore, IReceiverCore _receiverCore, IAlarmCore _alarmCore
            , IWareHouseCore _wareHouseCore)
        {
            collectorCore = _collectorCore;
            receiverCore = _receiverCore;
            alarmCore = _alarmCore;
            wareHouseCore = _wareHouseCore;
        }

        /// <summary>
        /// 200成功，203失败
        /// Post测试用
        /// </summary>
        [HttpPost]
        public string PostPackTest(IPacks _pack)
        {
            string reStr = string.Empty;
            var pkentity = JsonConvertHelper.SerializeObjectNo(_pack);
            Utils.PrintLog("pkentity" + pkentity, "PostPack2");
            if (_pack == null)
                return string.Empty;

            bool rebit = false;

            #region 批量采集温度

            var clist = _pack.Measurers;
            if (clist != null && clist.Count > 0)
            {
                //rebit = collectorCore.AddTemp(clist);
                reStr = collectorCore.UploadMeasurers(clist);
                return reStr;
            }

            #endregion

            #region 安装收集器/采集器

            var rmodel = _pack.Collector;
            var c_short = string.Empty;
            if (rmodel != null)
            {
                var type = TypeParse.StrToInt(rmodel.type, 0);
                if (type == 2)
                    rebit = receiverCore.InstallReceiver(rmodel, out c_short);//安装收集器
                else if (type == 1)
                {
                    reStr = collectorCore.CollectorInstall(rmodel);//安装采集器
                    return reStr;
                }
                else if (type == 0)//更新温、湿度（包含室内外）
                {
                    reStr = receiverCore.AddTemAndHum(rmodel);
                    return reStr;
                }
                else // 外网安装用，或者以备后续推行消息指令
                { }
                if (rebit == false)
                    return string.Empty;
                else
                    //return !string.IsNullOrEmpty(c_short) ? c_short.PadLeft(4, '0') : string.Empty; ;
                    return !string.IsNullOrEmpty(c_short) ? ("BSN," + c_short.PadLeft(4, '0') + ",") : string.Empty;
            }

            #endregion

            #region 报警(电池没电)
            var alist = _pack.Alarms;
            if (alist != null && alist.Count > 0)
            {
                rebit = alarmCore.Add(alist);
                return string.Empty;
            }

            #endregion
            return string.Empty;
        }

        /// <summary>
        /// 200成功，203失败(IOT设备调用2017-03-10 17:12:46)
        /// 正式
        /// </summary>
        [HttpGet]
        public string PostPack2(string _pack)
        {
            if (string.IsNullOrEmpty(_pack))
                return string.Empty;
            string reStr = string.Empty;
            //Utils.PrintLog("pkentity" + _pack, "PostPack2");
            #region  调试打印日志               
            if (Utils.DebugApp)
            {
                var msg = _pack;//JsonConvertHelper.SerializeObject(_entity);
                Utils.PrintLog(msg, "PostPack2-IOT交互信息", "DebugLog");
            }
            #endregion //调试打印日志
            var pkentity = JsonConvertHelper.DeserializeJsonToObject<IPacks>(_pack);
            bool rebit = false;

            #region 批量采集温度
            var clist = pkentity.Measurers;
            if (clist != null && clist.Count > 0)
            {
                //rebit = collectorCore.AddTemp(clist);
                reStr = collectorCore.UploadMeasurers(clist);
                return reStr;
            }
            #endregion

            #region 安装收集器/采集器
            var rmodel = pkentity.Collector;
            var c_short = string.Empty;
            if (rmodel != null)
            {
                var type = TypeParse.StrToInt(rmodel.type, 0);
                if (type == 2)
                    rebit = receiverCore.InstallReceiver(rmodel, out c_short);
                else if (type == 1)
                {
                    reStr = collectorCore.CollectorInstall(rmodel);
                    return reStr;
                }
                else if (type == 0)//更新湿度
                {
                    reStr = receiverCore.AddTemAndHum(rmodel);
                    return reStr;
                }
                else // 外网安装用，或者以备后续推行消息指令
                { }
                if (rebit == false)
                    return string.Empty;
                else
                    return !string.IsNullOrEmpty(c_short) ? ("BSN," + c_short.PadLeft(4, '0') + ",") : string.Empty;
            }
            #endregion

            #region 报警
            var alist = pkentity.Alarms;
            if (alist != null && alist.Count > 0)
            {
                rebit = alarmCore.Add(alist);
                return string.Empty;
            }
            #endregion

            return string.Empty;
        }

        #region Test
        /// <summary>
        /// 200成功，203失败(old) 2017-03-12 17:18:09
        /// </summary>
        [HttpGet]
        public string PostPack([FromUri]IPacks _pack)
        {
            #region 测试URi
            //if (_pack == null)
            //    return "203";
            //bool rebit = false;
            ////var datenow = Utils.GetServerDateTime();
            //#region 采集温度
            //var clist = _pack.Measurers;
            //if (clist != null && clist.Count > 0)
            //    rebit = collectorCore.AddTemp(clist);
            //#endregion

            //#region 安装
            //var rmodel = _pack.Collector;
            //var c_short = string.Empty;
            //if (rmodel != null)
            //{
            //    var type = TypeParse.StrToInt(rmodel.type, 0);
            //    if (type == 2)
            //        rebit = receiverCore.Install(rmodel, out c_short);
            //    else if (type == 1)
            //        rebit = collectorCore.Install(rmodel);
            //    else if (type == 0)//更新湿度
            //    {
            //        rebit = receiverCore.AddHum(rmodel);
            //    }
            //    else // 外网安装用，或者以备后续推行消息指令
            //    { }
            //}
            //#endregion

            //#region 报警
            //var alist = _pack.Alarms;
            //rebit = alarmCore.Add(alist);
            //#endregion

            //if (rebit == false)
            //    return "203";
            //else
            //    return "200+" + c_short;
            #endregion
            return "";
        }

        #endregion

        /// <summary>
        /// IOT获取BBD数据 2017-03-15 17:17:51
        /// </summary>
        public Dictionary<string, object[]> GetBBDList()
        {
            return wareHouseCore.GetBBDList();
        }

        //public string GetBBDList2()
        //{
        //    return "BBD"+JsonConvertHelper.SerializeObjectNo(wareHouseCore.GetBBDList());
        //}


    }
}