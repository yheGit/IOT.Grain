using Net66.Comm;
using Net66.Core.Interface;
using Net66.Data.Interface;
using Net66.Entity.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Net66.Core
{
    public class CollectorCore : ICollectorCore
    {
        private static IGrainRepository<Collector> cRepository;
        private static IGrainRepository<Granary> gRepository;
        private static IGrainRepository<Receiver> rRepository;
        private static IGrainRepository<Temperature> tRepository;
        private static IGrainRepository<Sensor> sRepository;
        private static IGrainRepository<SensorBase> sbRepository;
        private static IGrainRepository<LineBase> lbRepository;
        private static string endash = StaticClass.Endash;

        public CollectorCore(IGrainRepository<Collector> _cRepository, IGrainRepository<Temperature> _tRepository
            , IGrainRepository<Sensor> _sRepository, IGrainRepository<SensorBase> _sbRepository
            , IGrainRepository<Receiver> _rRepository, IGrainRepository<Granary> _gRepository, IGrainRepository<LineBase> _lbRepository)
        {
            cRepository = _cRepository;
            tRepository = _tRepository;
            sRepository = _sRepository;
            sbRepository = _sbRepository;
            rRepository = _rRepository;
            gRepository = _gRepository;
            lbRepository = _lbRepository;
        }

        public bool Install(IReceiver _entity)
        {
            var reint = 0;
            var datenow = Utils.GetServerTime();
            var c_short = TypeParse._16NAC_To_10NSC(_entity.c_short);
            var slayer = TypeParse.StrToInt(_entity.sublayer, 0);
            if (slayer > 0)
            {               
                var rInfo = rRepository.Get(g => g.ID == c_short);
                if (rInfo == null)
                    return false;
                var rnumber = rInfo.Number;
                var heapnumber = Utils.StrSequenConcat(rnumber, endash, _entity.heap);
                //var guidKey = c_short + "_" + _entity.heap + "_" + _entity.sublayer;
                var guid = Utils.MD5(heapnumber);
                var addCollecters = new List<Collector>() {new Collector(){
                    GuidID=guid,
                    CPUId = _entity.c_cpuid,
                    InstallDate = datenow,
                    HeapNumber=heapnumber,
                    R_Code=c_short,
                    Sublayer=slayer,
                    UserId=0,
                    //Voltage=null,
                    //SensorIdArr=null,
                    BadPoints=0,
                    IsActive = 1
                } };
                //var selectKey = new string[] { "GuidID" };
                ////var selectKey = new string[] { "CPUId" };
                //var updateKey = new string[] { "CPUId", "IsActive", "Sublayer" };
                //reint = cRepository.AddUpdate(addCollecters, selectKey, updateKey, "InstallDate");
                //reint = cRepository.AddDelete(addCollecters, selectKey, "InstallDate");
                reint = new Data.Context.DbEntity().AddUpdateCollector(addCollecters);
            }

            return reint > 0;

        }

        #region 安装传感器，并采集温度
        /// <summary>
        /// 收集温度 2017-03-12 14:07:11
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public bool AddTemp(List<ICollector> _list)
        {
            if (_list == null || _list.Count < 0)
                return false;

            #region 参数定义
            var reint = 0;
            var collectid = "collectUnkwon";
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();
            var addTemp = new List<Temperature>();
            var add_CTemp = new List<Temperature>();
            var updateList = new List<Collector>();
            var addSensors = new List<Sensor>();
            var sensorIdList = new List<string>();
            var redhots = new List<string>();//红色报警
            var dicBadhots = new Dictionary<string, int>();//坏点记录
            List<SensorBase> sbAddList = new List<SensorBase>();
            #endregion  参数定义 

            foreach (var cmodel in _list)
            {
                var type = TypeParse.StrToInt(cmodel.type, 0);
                if (type == 1 || type == 2)//1温度、 2ID、
                {
                    //cmodel.c_short
                    int depth = 0;
                    List<string> sensorList = new List<string>();

                    #region  添加传感器
                    if (cmodel.SensorId != null && cmodel.SensorId.Count > 0 && string.IsNullOrEmpty(cmodel.temp))
                    {
                        var sublayer = cRepository.Get(g => g.CPUId == cmodel.m_cpuid);
                        if (sublayer == null)
                            continue;
                        collectid = cmodel.m_cpuid + "No";
                        #region 更新采集器
                        updateList.Add(new Collector()
                        {
                            GuidID=sublayer.GuidID,
                            CPUId = cmodel.m_cpuid,
                            InstallDate = datenowStr,
                            SensorIdArr = cmodel.sum > 0 ? string.Join(",", cmodel.SensorId) : null
                        });
                        #endregion

                        sensorList = cmodel.SensorId;                       
                        var sblist = sbRepository.GetList(g => sensorList.Contains(g.SCpu));//传感器的基础信息表，用于定位
                        Dictionary<string, int> linelist = new Dictionary<string, int>();
                        string lineCode = collectid + (linelist.Count + 1);
                        foreach (var f in sensorList)
                        {
                            #region 生成传感线
                            var sequen = ++depth;
                            var fsensor = f;                           

                            //来自备案的信息
                            var baseinfo = sblist.FirstOrDefault(d => d.SCpu == f);
                            if (baseinfo != null)
                            {
                                lineCode = baseinfo.SLineCode;
                                sequen = baseinfo.SSequen ?? 0;
                            }
                            else
                            {
                                if (f.StartsWith("3"))
                                {
                                    fsensor = "2" + fsensor.Substring(1);
                                    lineCode = collectid + (linelist.Count + 1);
                                    depth = 0;
                                }
                            }

                            if (linelist == null || !linelist.ContainsKey(lineCode))
                            {
                                var count = linelist == null ? 0 : linelist.Count;
                                linelist.Add(lineCode, count + 1);
                            }                           

                            int lSequen = -1;

                            if (sublayer.HeapNumber.IndexOf('T') > -1 || sublayer.HeapNumber.IndexOf('Q') > -1)
                            {
                                var line = lbRepository.Get(g => g.HeapNumber == sublayer.HeapNumber && g.LineCode == lineCode);
                                if (line != null)
                                    lSequen = line.LSequence.Value;
                            }
                            #endregion 生成传感线

                            int x = sequen, y = lSequen != -1 ? lSequen : (linelist == null ? 1 : linelist.FirstOrDefault(d => d.Key == lineCode).Value), z = sublayer.Sublayer;
                            //var guidKey = cmodel.m_cpuid + "_" + lineCode + "_" + sequen;
                            var guidKey = sublayer.HeapNumber + "_" + x + "_" + y + "_" + z;
                            var guid = Utils.MD5(guidKey);

                            addSensors.Add(new Sensor()
                            {
                                CRC=f,
                                SensorId = fsensor,
                                IsActive = 1,
                                Collector = cmodel.m_cpuid,
                                Label = lineCode,
                                Direction_X = x,
                                Direction_Y = y,
                                Direction_Z = z,
                                Sequen = sequen,
                                IsBad = 0,
                                GuidID = guid
                            });
                        }

                        //添加传感器
                        //var sKey = new string[] { "GuidID" };
                        //var uKey = new string[] { "Collector", "SensorId", "Label" };
                        //reint = sRepository.AddUpdate(addSensors, sKey, uKey);
                        reint = new Data.Context.DbEntity().AddUpdateSensor(addSensors);

                        //批量添加采集器
                        var selectKey = new string[] { "GuidID" };
                        var updateKey = new string[] { "CPUId","SensorIdArr", "InstallDate" };
                        //reint = cRepository.AddUpdate(updateList, selectKey, updateKey);
                        reint=new Data.Context.DbEntity().AddUpdateCollector(updateList);


                    }
                    #endregion

                    #region 收集温度
                    if (!string.IsNullOrEmpty(cmodel.c_short) && !string.IsNullOrEmpty(cmodel.temp))
                    {
                        var c_short = TypeParse._16NAC_To_10NSC(cmodel.c_short);
                        var rInfo = rRepository.Get(g => g.ID == c_short);
                        if (rInfo == null)
                        {
                            Utils.PrintLog("不存在该收集器的信息，Receiver:" + c_short, "AddTemp");
                            return false;
                        }
                        var wh_number = rInfo.W_Number;
                        var g_number = rInfo.Number;

                        if (sensorList == null || sensorList.Count <= 0)
                        {
                            var info = cRepository.Get(g => g.CPUId == cmodel.m_cpuid);
                            if (info != null)
                                sensorList = info.SensorIdArr.Split(',').ToList();
                        }

                        decimal cjqTemp = 0;
                        if (sensorList.Count > 0)
                        {
                            #region chuanganqi temp
                            int i = 0;
                            for (; i < sensorList.Count; i++)
                            {
                                var f = sensorList[i];
                                if (f.StartsWith("3"))
                                    f = "2" + f.Substring(1);

                                var temp = Comm.SysApi.Tools.GetTemp(cmodel.temp, i);

                                #region 坏点
                                if (temp == (decimal)63.75 || temp == (decimal)0)//坏点
                                {
                                    //badhots.Add(f);
                                    dicBadhots.Add(f, 1);
                                    continue;
                                }
                                else
                                {
                                    dicBadhots.Add(f, 0);
                                }
                                #endregion 坏点

                                #region 高温红色报警

                                if (temp >= (decimal)35 && temp < (decimal)63.75)
                                {
                                    redhots.Add(f);
                                }

                                #endregion

                                cjqTemp += temp;
                                addTemp.Add(new Temperature()
                                {
                                    PId = f,
                                    StampTime = datenowStr,
                                    UpdateTime = datenow,
                                    Temp = temp,
                                    RealHeart = 0,
                                    Type = 0,
                                    WH_Number = wh_number,
                                    G_Number = g_number
                                });
                            }
                            cjqTemp = cjqTemp / i;
                            #endregion

                            #region caijiqi temp
                            add_CTemp.Add(new Temperature()
                            {
                                PId = cmodel.m_cpuid,
                                StampTime = datenowStr,
                                UpdateTime = datenow,
                                Type = 1,//0传感器、1采集器、2收集器（室外）
                                RealHeart = 0,
                                Temp = cjqTemp,
                                WH_Number = wh_number,
                                G_Number = g_number
                            });
                            #endregion

                        }
                    }
                    sensorIdList.AddRange(sensorList);

                    #region 批量插入温度
                    reint = tRepository.AddUpdate(addTemp, p => p.RealHeart == 0 && sensorIdList.Contains(p.PId) && p.Type == 0
                        , "RealHeart", 1, "StampTime");

                    //同时更新采集器的温度
                    var cpuIds = _list.Select(s => s.m_cpuid).ToList();
                    reint = tRepository.AddUpdate(add_CTemp, p => p.RealHeart == 0 && cpuIds.Contains(p.PId) && p.Type == 1
                        , "RealHeart", 1, "");

                    #endregion

                    #endregion

                    #region 更新坏点

                    //更新采集器的坏点数
                    new Data.Context.DbEntity().UpdateBadHot(dicBadhots);
                    //Action<Dictionary<string, int>> pushMsg = new Data.Context.DbEntity().UpdateBadHot;
                    //pushMsg.BeginInvoke(dicBadhots, ar => pushMsg.EndInvoke(ar), null);
                    #endregion

                    #region 高温报警

                    new Data.Context.DbEntity().UpdateRedHot(redhots);
                    //Action<List<string>> pushRedHotMsg = new Data.Context.DbEntity().UpdateRedHot;
                    //pushRedHotMsg.BeginInvoke(redhots, ar => pushRedHotMsg.EndInvoke(ar), null);

                    #endregion
                }
                else if (type == 3)//3线编号,
                {
                    List<SensorBase> sbList = new List<SensorBase>();
                    var senArr = cmodel.SensorId;
                    for (int i = 0; i < senArr.Count; i++)
                    {
                        sbList.Add(new SensorBase() { SLineCode = cmodel.label, SCpu = senArr[i], SSequen = ++i, StampTime = datenowStr });
                    }
                    sbAddList.AddRange(sbList);
                }

            }

            #region  线备案
            sbRepository.Add(sbAddList);
            //Action<List<SensorBase>> actionSbList = AddSensorBaseList;
            //actionSbList.BeginInvoke(sbAddList, ar => actionSbList.EndInvoke(ar), null);

            #endregion
            return reint > 0;
        }

        /// <summary>
        /// 线备案 2017-6-8 16:38:36
        /// </summary>
        private void AddSensorBaseList(List<SensorBase> sblist)
        {
            try
            {
                sbRepository.Add(sblist);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "AddSensorBaseList");
            }
        }

        #endregion 安装传感器，并采集温度

    }
}
