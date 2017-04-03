using Net66.Comm;
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
        /// 测试用
        /// </summary>
        [HttpPost]
        public string PostPackTest(IPacks _pack)
        {
            if (_pack == null)
                return "";
            bool rebit = false;
            var datenow = Utils.GetServerDateTime();

            #region 批量插入温度
            var clist = _pack.Measurers;
            if (clist != null && clist.Count > 0)
                rebit = collectorCore.AddTemp(clist);
            #endregion

            #region 收集器/采集器安装
            var rmodel = _pack.Collector;
            var c_short = string.Empty;
            if (rmodel != null)
            {
                var type = TypeParse.StrToInt(rmodel.type, 0);
                if (type == 2)
                    rebit = receiverCore.Install(rmodel, out c_short);//安装收集器
                else if (type == 1)
                    rebit = collectorCore.Install(rmodel);//安装采集器
                else if (type == 0)//更新湿度
                {
                    rebit = receiverCore.AddHum(rmodel);
                }
                else // 外网安装用，或者以备后续推行消息指令
                { }
            }
            #endregion

            #region 报警
            var alist = _pack.Alarms;
            if (alist != null && alist.Count > 0)
                rebit = alarmCore.Add(alist);
            #endregion

            if (rebit == false)
                return string.Empty;
            else
                return !string.IsNullOrEmpty(c_short) ? c_short.PadLeft(4, '0') : string.Empty; ;
        }


        /// <summary>
        /// 200成功，203失败
        /// </summary>
        [HttpGet]
        public string PostPack([FromUri]IPacks _pack)
        {
            if (_pack == null)
                return "203";
            bool rebit = false;
            var datenow = Utils.GetServerDateTime();
            #region 采集温度
            var clist = _pack.Measurers;
            if (clist != null && clist.Count > 0)
                rebit = collectorCore.AddTemp(clist);
            #endregion

            #region 安装
            var rmodel = _pack.Collector;
            var c_short = string.Empty;
            if (rmodel != null)
            {
                var type = TypeParse.StrToInt(rmodel.type, 0);
                if (type == 2)
                    rebit = receiverCore.Install(rmodel, out c_short);
                else if (type == 1)
                    rebit = collectorCore.Install(rmodel);
                else if (type == 0)//更新湿度
                {
                    rebit = receiverCore.AddHum(rmodel);
                }
                else // 外网安装用，或者以备后续推行消息指令
                { }
            }
            #endregion

            #region 报警
            var alist = _pack.Alarms;
            rebit = alarmCore.Add(alist);
            #endregion

            if (rebit == false)
                return "203";
            else
                return "200+" + c_short;
        }

        /// <summary>
        /// 200成功，203失败
        /// pangdong diaoyong
        /// </summary>
        [HttpGet]
        public string PostPack2(string _pack)
        {
            if (string.IsNullOrEmpty(_pack))
                return "203";
            var pkentity = JsonConvertHelper.DeserializeJsonToObject<IPacks>(_pack);

            bool rebit = false;
            var datenow = Utils.GetServerDateTime();
            #region 采集器
            var clist = pkentity.Measurers;
            if (clist != null && clist.Count > 0)
                rebit = collectorCore.AddTemp(clist);
            #endregion

            #region 收集器/采集器安装
            var rmodel = pkentity.Collector;
            var c_short = string.Empty;
            if (rmodel != null)
            {
                var type = TypeParse.StrToInt(rmodel.type, 0);
                if (type == 2)
                    rebit = receiverCore.Install(rmodel, out c_short);
                else if (type == 1)
                    rebit = collectorCore.Install(rmodel);
                else if (type == 0)//更新湿度
                {
                    rebit = receiverCore.AddHum(rmodel);
                }
                else // 外网安装用，或者以备后续推行消息指令
                { }
            }
            #endregion

            #region 报警
            var alist = pkentity.Alarms;
            if (alist != null && alist.Count > 0)
                rebit = alarmCore.Add(alist);
            #endregion
            if (rebit == false)
                return string.Empty;
            else
                return !string.IsNullOrEmpty(c_short) ? c_short.PadLeft(4, '0') : string.Empty; 

        }

        /// <summary>
        /// huoquBBDshuju
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