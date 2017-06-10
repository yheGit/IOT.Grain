using Net66.Comm;
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
            var cupid = _entity.c_cpuid;
            var number = Utils.StrSequenConcat(_entity.building, endash, _entity.layer, endash, _entity.room);
            var datenowStr = datenow.ToString();
            //var guidKey = _entity.building + "_" + _entity.layer + "_" + _entity.room;
            var guidKey = number;
            var guid = Utils.MD5(guidKey);
            var temp = Comm.SysApi.Tools.GetTemp(_entity.temp, 0);
            var model = new Receiver()
            {
                GuidID = guid,
                CPUId = cupid,
                InstallDate = datenowStr,
                IsActive = 1,
                W_Number = _entity.building,
                F_Number = _entity.layer.ToString(),
                G_Number = _entity.room.ToString(),
                Number = number,
                //IPAddress=null,
                //UserId = null,
                Humidity = Comm.SysApi.Tools.GetTemp(_entity.hum, 0),//Convert.ToDecimal(_entity.hum),
                Temperature = temp// Convert.ToDecimal(_entity.temp)
            };
            ////var addList = new List<Receiver>() { model };
            ////var selectKey = new string[] { "GuidID" };
            //var selectKey = new string[] { "CPUId" };
            //var updateKey = new string[] { "CPUId", "Humidity", "IsActive", "Temperature" };
            ////reint = rRepository.AddUpdate(addList, selectKey, updateKey, "InstallDate");
            //reint = rRepository.AddDelete(addList, selectKey, "InstallDate");
            try
            {
                reint = new Data.Context.DbEntity().AddOrUpdateReceiver(model);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "AddOrUpdateReceiver");
            }

            if (reint > 0)
            {
                //var reId = addList.FirstOrDefault().ID;
                //if (reId == 0)
                //{
                //    //var rInfo = rRepository.Get(g => g.GuidID == guid);
                //    var rInfo = rRepository.Get(g => g.CPUId == cupid);
                //    if (rInfo != null)
                //        reId = rInfo.ID;
                //}
                var reId = reint;
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
            var temp = Convert.ToDecimal(_entity.temp);//Convert.ToDecimal(Convert.ToDouble(_entity.temp)*175.72/0x10000-46.85);
            var hum = Convert.ToDecimal(_entity.hum);//Convert.ToDecimal(Convert.ToDouble(_entity.hum)*125.00/0x10000-6);
            var reint = 0;
            var c_short = TypeParse._16NAC_To_10NSC(_entity.c_short);
            var rInfo = rRepository.Get(g => g.ID == c_short);
            if (rInfo != null)
            {
                var wh_number = rInfo.W_Number;
                var f_number = rInfo.F_Number;
                var g_number = rInfo.Number;
                #region  仓内、外湿度              
                var ttype = 0;//仓内湿度
                var temptype = 2;//仓内温度
                if (!string.IsNullOrEmpty(f_number) && f_number.Equals("0"))//约定楼层为零时，为仓外湿度
                {
                    ttype = 1;// 仓外湿度
                    temptype = 3;//仓外温度
                    g_number = "0";
                }

                var receierid = TypeParse._16NAC_To_10NSC(_entity.c_short);
                var addEntity = new Humidity()
                {
                    Humility = hum,//Comm.SysApi.Tools.GetTemp(_entity.hum, 0),
                    Temp = hum,//Comm.SysApi.Tools.GetTemp(_entity.temp, 0),
                    ReceiverId = receierid,
                    StampTime = datenowStr,
                    Type = ttype,  //0仓内湿度，1仓外湿度
                    G_Number = g_number,
                    RealHeart = 0,
                    WH_Number = wh_number
                };
                reint = hRepository.AddUpdate(new List<Humidity>() { addEntity },p=>p.RealHeart==0&&p.Type==ttype&&p.ReceiverId== receierid
                , "RealHeart", 1, "StampTime");
                #endregion

                #region 仓内、外温度
                reint = tRepository.AddUpdate(new List<Temperature>() { new Temperature()
                    {
                        PId = receierid.ToString(),
                        StampTime = datenowStr,
                        UpdateTime=datenow,
                        Type = temptype,//0传感器、1采集器、2收集器（仓内温度）、3收集器（室外）
                        RealHeart = 0,
                        Temp = temp,
                        G_Number=g_number,
                        WH_Number=wh_number//
                    } }, p => (p.RealHeart == 0 && p.Type == temptype && p.PId.Equals(receierid.ToString())), "RealHeart", 1, "StampTime");

                #endregion

                return 1 > 0;
            }

            return 0 > 0;
        }


    }
}
