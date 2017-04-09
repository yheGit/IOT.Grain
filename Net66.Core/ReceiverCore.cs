﻿using Net66.Comm;
using Net66.Core.Interface;
using Net66.Data.Interface;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core
{
    public class ReceiverCore : IReceiverCore
    {
        private static IGrainRepository<Receiver> rRepository;
        private static IGrainRepository<Collector> cRepository;
        private static IGrainRepository<Humidity> hRepository;
        private static IGrainRepository<Temperature> tRepository;
        private static string endash = StaticClass.Endash;

        public ReceiverCore(IGrainRepository<Receiver> _rRepository, IGrainRepository<Collector> _cRepository
            , IGrainRepository<Humidity> _hRepository, IGrainRepository<Temperature> _tRepository)
        {
            rRepository = _rRepository;
            cRepository = _cRepository;
            hRepository = _hRepository;
            tRepository = _tRepository;
        }

        public bool Install(IReceiver _entity, out string c_short)
        {
            var reint = 0;
            c_short = "ff";
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();
            var guidKey = _entity.building + "_" + _entity.layer + "_" + _entity.room;
            var guid = Utils.MD5(guidKey);
            var temp = Comm.SysApi.Tools.GetTemp(_entity.temp, 0);
            var addList = new List<Receiver>() { new Receiver()
            {
                 GuidID=guid,
                CPUId = _entity.c_cpuid,
                InstallDate = datenowStr,
                IsActive = 1,
                W_Number=_entity.building,
                F_Number = _entity.layer.ToString(),
                G_Number=_entity.room.ToString(),
                Number=Utils.StrSequenConcat(_entity.building,endash,_entity.layer,endash,_entity.room),
                //IPAddress=null,
                //UserId = null,
                 Humidity = Comm.SysApi.Tools.GetTemp(_entity.hum, 0),//Convert.ToDecimal(_entity.hum),
                Temperature =temp// Convert.ToDecimal(_entity.temp)
            } };
            var selectKey = new string[] { "GuidID" };
            var updateKey = new string[] { "CPUId", "Humidity", "IsActive", "Temperature" };
            reint = rRepository.AddUpdate(addList, selectKey, updateKey, "InstallDate");

            if (reint > 0)
            {
                //if (_entity.type == 0)
                //{
                //    var ttype = 2;
                //    if (string.IsNullOrEmpty(_entity.layer) && string.IsNullOrEmpty(_entity.room))
                //        ttype = 3;//louceng aojian weikongshi weiliangcangshiwaiwendu
                //    reint = tRepository.AddUpdate(new List<Temperature>() { new Temperature()
                //    {
                //        PId = _entity.c_cpuid,
                //        StampTime = datenowStr,
                //        UpdateTime=datenow, 
                //        Type = ttype,//0chuanganqi、1caijiqi、2shoujiqi nei、3shoujiqi wai
                //        RealHeart = 0,
                //        Temp = temp
                //    } }, p => (p.RealHeart == 0 && p.Type == ttype&&p.PId==_entity.c_cpuid), "RealHeart", 1, "StampTime");
                //}
                var reId = addList.FirstOrDefault().ID;
                if (reId == 0)
                {
                    var rInfo = rRepository.Get(g => g.GuidID == guid);
                    if (rInfo != null)
                        reId = rInfo.ID;
                }
                c_short = TypeParse._10NSC_To_16NAC(reId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新收集器自身的温湿度
        /// 温度为粮仓仓外温度
        /// </summary>
        /// <param name="_entity"></param>
        /// <returns></returns>
        public bool AddHum(IReceiver _entity)
        {
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();
            var temp = Convert.ToDecimal(_entity.temp);//Comm.SysApi.Tools.GetTemp(_entity.temp, 0);
            var reint = 0;
            var c_short = TypeParse._16NAC_To_10NSC(_entity.c_short);
            var rInfo = rRepository.Get(g => g.ID == c_short);
            if (rInfo != null)
            {
                var wh_number = rInfo.W_Number;
                var g_number = rInfo.Number;
                #region  仓内、外湿度
                var ttype = 0;//仓内湿度
                if (string.IsNullOrEmpty(_entity.layer)
                    || (!string.IsNullOrEmpty(_entity.layer) && _entity.layer.Equals("0")))//约定楼层为零时，为仓外湿度
                    ttype = 1;// 仓外湿度
                              //0仓内湿度，1仓外湿度
                var addEntity = new Humidity()
                {
                    Humility = Convert.ToDecimal(_entity.hum),//Comm.SysApi.Tools.GetTemp(_entity.hum, 0),
                    Temp = temp,//Comm.SysApi.Tools.GetTemp(_entity.temp, 0),
                    ReceiverId = TypeParse._16NAC_To_10NSC(_entity.c_short),
                    StampTime = datenowStr,
                    Type = ttype,
                    G_Number = "0",
                    WH_Number = wh_number
                };
                reint = hRepository.Add(addEntity);
                #endregion

                #region 仓外温度
                reint = tRepository.AddUpdate(new List<Temperature>() { new Temperature()
                    {
                        PId = string.IsNullOrEmpty(_entity.c_cpuid)?rInfo.CPUId:_entity.c_cpuid,
                        StampTime = datenowStr,
                        UpdateTime=datenow,
                        Type = 2,//0传感器、1采集器、2收集器（室外）
                        RealHeart = 0,
                        Temp = temp,
                        G_Number="0",
                        WH_Number=wh_number//
                    } }, p => (p.RealHeart == 0 && p.Type == ttype && p.PId == _entity.c_cpuid), "RealHeart", 1, "StampTime");

                #endregion

                return 1 > 0;
            }

            return 0 > 0;
        }


    }
}
