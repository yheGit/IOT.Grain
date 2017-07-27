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

        public string CollectorInstall(IReceiver _entity)
        {
            #region  调试打印日志               
            if (Utils.DebugApp)
            {
                var msg = JsonConvertHelper.SerializeObject(_entity);
                Utils.PrintLog(msg, "CollectorInstall-安装分机", "DebugLog");
            }
            #endregion //调试打印日志

            var reStr = "BAP";
            var reint = 0;
            var datenow = Utils.GetServerTime();
            var c_short = TypeParse._16NAC_To_10NSC(_entity.c_short);
            var slayer = TypeParse.StrToInt(_entity.sublayer, 0);
            var column = TypeParse.StrToInt(_entity.column, 0);

            if (slayer > 0)
            {
                var rInfo = rRepository.Get(g => g.ID == c_short);
                if (rInfo == null)
                    return reStr + ",,,";//"Unfind Collecter";//找不到主机
                var rnumber = rInfo.Number;
                var heapnumber = Utils.StrSequenConcat(rnumber, endash, _entity.heap);
                //var guidKey = c_short + "_" + _entity.heap + "_" + _entity.sublayer;
                var guidKey = Utils.StrSequenConcat(heapnumber, endash, slayer.ToString(), endash, column.ToString());
                var guid = Utils.MD5(guidKey);
                var addCollecters = new List<Collector>() {new Collector(){
                    GuidID=guid,
                    CPUId = _entity.c_cpuid,
                    InstallDate = datenow,
                    HeapNumber=heapnumber,
                    R_Code=c_short,
                    Sublayer=slayer,
                    Column=column,
                    UserId=0,
                    //Voltage=null,
                    //SensorIdArr=null,
                    BadPoints=0,
                    IsActive = 1
                } };
                reint = new Data.Context.DbEntity().AddUpdateCollector(addCollecters);
                Utils.PrintLog("reint:" + reint, "CollectorCore-Install");
            }
            else
            {
                if (!string.IsNullOrEmpty(_entity.c_short) && !string.IsNullOrEmpty(_entity.c_cpuid))
                {
                    var cInfo = cRepository.Get(g => g.R_Code == c_short && g.CPUId == _entity.c_cpuid);
                    if (cInfo != null)
                    {
                        _entity.heap = cInfo.HeapNumber.Split('-').LastOrDefault();
                        _entity.sublayer = cInfo.Sublayer.ToString();
                        _entity.column = cInfo.Column.ToString();
                    }
                }
            }

            if (reint <= 0)
            {
                _entity.heap = "";
                _entity.sublayer = "";
                _entity.column = "";
            }

            return reStr + "," + _entity.heap + "," + _entity.sublayer + "," + _entity.column;

        }


        public string UploadMeasurers(List<ICollector> _list)
        {
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();
            if (_list == null || _list.Count < 0)
                return string.Empty;
            var rebit = false;
            foreach (var cmodel in _list)
            {
                var type = TypeParse.StrToInt(cmodel.type, 0);

                if (type == 1)
                {
                    #region  调试打印日志               
                    if (Utils.DebugApp)
                    {
                        var msg = JsonConvertHelper.SerializeObject(cmodel);
                        Utils.PrintLog(msg, "AddTemps-采集温度", "DebugLog");
                    }
                    #endregion //调试打印日志
                    //采集温度
                    rebit = AddTemps(cmodel);
                    //2017-06-15 11:27:18 废弃采集器的平均温度
                    ////同时更新采集器的温度
                    //var cpuIds = _list.Select(s => s.m_cpuid).ToList();
                    //tRepository.AddUpdate(CTemplist, p => p.RealHeart == 0 && cpuIds.Contains(p.PId) && p.Type == 1, "RealHeart", 1, "");
                }

                if (type == 2)
                {
                    #region  调试打印日志               
                    if (Utils.DebugApp)
                    {
                        var msg = JsonConvertHelper.SerializeObject(cmodel);
                        Utils.PrintLog(msg, "AddSensors-将传感器映射到指定位置（这里包含更新分机、给线备案）", "DebugLog");
                    }
                    #endregion //调试打印日志

                    #region 更新采集器  供后续上传温度使用
                    var sublayer = cRepository.Get(g => g.CPUId == cmodel.m_cpuid);
                    if (sublayer == null)
                        return "";
                    var updateList = new List<Collector>();
                    updateList.Add(new Collector() //一机一线和一机多线，都只会产生一条分机安装记录
                    {
                        GuidID = sublayer.GuidID,
                        //CPUId = cmodel.m_cpuid,
                        //Sublayer = sublayer.Sublayer,
                        InstallDate = datenowStr,
                        SensorIdArr = cmodel.sum > 0 ? string.Join(",", cmodel.SensorId) : null //cmodel.SensorId原ORGID
                    });

                    //批量更新采集器
                    if (new Data.Context.DbEntity().UpdateCollector(updateList) <= 0)
                        return "";
                    #endregion

                    if (AddLineCode(cmodel) == true)//给线备案
                    {
                        AddSensors(cmodel, sublayer); //将传感器映射到指定位置
                    }
                }

                if (type == 3)
                {
                    #region  调试打印日志               
                    if (Utils.DebugApp)
                    {
                        var msg = JsonConvertHelper.SerializeObject(cmodel);
                        Utils.PrintLog(msg, "AddLineCode-备案传感线", "DebugLog");
                    }
                    #endregion //调试打印日志
                    AddLineCode(cmodel);
                }
            }
            if (rebit == true)
                return datenowStr;

            return "";
        }

        /// <summary>
        /// 分机收集温度 upby2017-6-11 16:29:23
        /// </summary>
        public bool AddTemps(ICollector cmodel)
        {
            var structList = new List<string>();//chuan gan qi 对应的物理结构ID
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();
            var dicBadhots = new Dictionary<string, int>();//坏点记录
            var redhots = new List<string>();//红色报警
            var addTemp = new List<Temperature>();

            #region 收集温度
            if (string.IsNullOrEmpty(cmodel.c_short) || string.IsNullOrEmpty(cmodel.temp))
                return false;

            #region 基础信息判断
            var c_short = TypeParse._16NAC_To_10NSC(cmodel.c_short);
            var rInfo = rRepository.Get(g => g.ID == c_short);//主机
            var cinfo = cRepository.Get(g => g.CPUId == cmodel.m_cpuid);//分机

            if (rInfo == null || cinfo == null)
            {
                Utils.PrintLog("不存在该收集器/采集器的信息，Receiver:" + c_short + ",CollecterID:" + cmodel.m_cpuid, "AddTemp");
                return false;
            }

            var sensorList = cinfo.SensorIdArr.Split(',').ToList();
            sensorList = sensorList.Distinct().ToList();
            if (sensorList.Count <= 0)
            {
                Utils.PrintLog("不存在传感器的安装信息，sensorList:" + string.Join("-", sensorList), "AddTemp");
                return false;
            }

            #endregion 基础信息判断
            var wh_number = rInfo.W_Number;
            var g_number = rInfo.Number;
            var h_number = cinfo.HeapNumber;

            var sList = sRepository.GetList(g => sensorList.Contains(g.SensorId)) ?? new List<Sensor>();
            #region chuanganqi temp
            int i = 0;
            for (; i < sensorList.Count; i++)
            {
                var sf = sensorList[i];
                #region 将传感器ID转化位物理位置
                var sInfo = sList.FirstOrDefault(d => d.SensorId == sf);
                if (sInfo == null)
                    continue;
                var pid = sInfo.GuidID;
                structList.Add(pid);//将物理结构ID假如集合
                #endregion 将传感器ID转化位物理位置

                var temp = Comm.SysApi.Tools.GetTemp(cmodel.temp, i);

                #region 坏点
                if (temp == (decimal)63.75 || temp == (decimal)0)//坏点
                {
                    //badhots.Add(f);
                    dicBadhots.Add(sf, 1);
                    continue;
                }
                else
                {
                    dicBadhots.Add(sf, 0);
                }
                #endregion 坏点

                #region 高温红色报警

                if (temp >= (decimal)35 && temp < (decimal)63.75)
                {
                    redhots.Add(sf);
                }

                #endregion

                addTemp.Add(new Temperature()
                {
                    PId = pid,
                    StampTime = datenowStr,
                    UpdateTime = datenow,
                    Temp = temp,
                    RealHeart = 0,
                    Type = 0,
                    WH_Number = wh_number,
                    G_Number = g_number,
                    H_Number = h_number,
                    TimeFlag = "-"
                });

                //做传感器的折线的变化图
                new Data.Context.DbEntity().AddTempsChartData2(wh_number, g_number, h_number, pid, temp, 1);
                //Action<string, string, string, string, decimal, int> pushTemps2 = new Data.Context.DbEntity().AddTempsChartData2;
                //pushTemps2.BeginInvoke(wh_number, g_number, h_number, pid, temp, 1, ar => pushTemps2.EndInvoke(ar), null);

            }
            #endregion


            #region 批量插入温度、更新坏点、高温报警            
            var reint = tRepository.AddUpdate(addTemp, p => p.RealHeart == 0 && structList.Contains(p.PId) && p.Type == 0, "RealHeart", 1, "StampTime");
            if (reint > 0)
            {
                //做粮堆的三温图
                new Data.Context.DbEntity().AddTempsChartData(wh_number, g_number, h_number, "-", 1);
                //Action<string, string, string, string, decimal, int> pushTemps = new Data.Context.DbEntity().AddTempsChartData;
                //pushTemps.BeginInvoke(wh_number, g_number, h_number, "-", averTepms, 1, ar => pushTemps.EndInvoke(ar), null);

            }
            //更新坏点- 更新采集器的坏点数
            new Data.Context.DbEntity().UpdateBadHot(dicBadhots);
            //高温报警
            new Data.Context.DbEntity().UpdateRedHot(redhots);
            #endregion

            #endregion

            return reint > 0;
        }


        /// <summary>
        /// 添加传感器 upby2017-6-10 13:09:29
        /// </summary>
        public bool AddSensors(ICollector cmodel, Collector _collecter)
        {
            var reint = 0;
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();
            List<string> sensorList = cmodel.SensorId;
            if (sensorList == null || sensorList.Count <= 0)
                return false;
            #region  添加传感器         

            var sblist = sbRepository.GetList(g => sensorList.Contains(g.SCpuOrg));//传感器的基础信息表，用于定位
            var addSensors = new List<Sensor>();
            foreach (var fsensor in sensorList)
            {
                #region 生成传感线 2017-4-18 10:32:12
                //来自备案的信息
                var baseinfo = sblist.FirstOrDefault(d => d.SCpuOrg == fsensor);
                if (baseinfo == null)
                    continue;
                var lineCode = baseinfo.SLineCode;
                var sequen = baseinfo.SSequen ?? 0;

                #region 筒仓 2017-4-18 10:31:48
                int lSequen = -1;
                if (_collecter.HeapNumber.IndexOf('T') > -1 || _collecter.HeapNumber.IndexOf('Q') > -1)
                {
                    var line = lbRepository.Get(g => g.HeapNumber == _collecter.HeapNumber && g.LineCode == lineCode);
                    if (line != null)
                        lSequen = line.LSequence.Value;
                }
                #endregion 筒仓 

                #endregion 生成传感线

                int y = sequen, x = lSequen != -1 ? lSequen : _collecter.Column ?? 0, z = _collecter.Sublayer;
                if ((_collecter.Column ?? 0) != 0)//一机一线 Column不等于0
                    x = _collecter.Column.Value;
                else
                    x = baseinfo.SCount;

                var guidKey = _collecter.HeapNumber + "_" + x + "_" + y + "_" + z;
                var guid = Utils.MD5(guidKey);
                addSensors.Add(new Sensor()
                {
                    CRC = guidKey,
                    SensorId = fsensor,//from orgID
                    IsActive = 1,
                    Collector = cmodel.m_cpuid,
                    Label = lineCode,
                    Direction_X = x,
                    Direction_Y = y,
                    Direction_Z = z,
                    Sequen = _collecter.GuidID,
                    IsBad = 0,
                    GuidID = guid,
                    InstallDate = datenowStr
                });
            }
            reint = new Data.Context.DbEntity().AddUpdateSensor(addSensors);

            #endregion
            return reint > 0;
        }

        /// <summary>
        /// 给线备案  upby2017-6-11 16:35:13
        /// </summary>
        public bool AddLineCode(ICollector cmodel)
        {
            var reint = 0; int depth = 0;
            var datenow = Utils.GetServerDateTime();
            var datenowStr = datenow.ToString();
            List<SensorBase> sbList = new List<SensorBase>();
            var senArr = cmodel.SensorId;
            if (senArr == null || senArr.Count <= 0)
                return false;
            var label = senArr[0];
            var column = 0;
            for (int i = 0; i < senArr.Count; i++)
            {
                var fsOrg = senArr[i];
                var fsensor = fsOrg;
                if (fsensor.StartsWith("3"))//换线
                {
                    label = fsensor;
                    fsensor = "2" + fsensor.Substring(1);
                    depth = 0;
                    column += 1;
                }
                var sequen = ++depth;
                sbList.Add(new SensorBase() { SLineCode = label, SCpu = fsensor, SCpuOrg = fsOrg, SSequen = sequen, SCount = column, StampTime = datenowStr });
            }
            //reint = sbRepository.Add(sbList);
            reint = new Data.Context.DbEntity().AddSbSensor(sbList);
            return reint > 0;
        }


        #region 安装传感器，并采集温度 Test(20170615废弃)
        /// <summary>
        /// 收集温度 2017-03-12 14:07:11
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public bool AddTemp(List<ICollector> _list)
        {
            var reint = 0;

            #region Test
            //if (_list == null || _list.Count < 0)
            //    return false;

            //#region 参数定义
            //var collectid = "collectUnkwon";
            //var datenow = Utils.GetServerDateTime();
            //var datenowStr = datenow.ToString();
            //var addTemp = new List<Temperature>();
            //var add_CTemp = new List<Temperature>();
            //var updateList = new List<Collector>();
            //var addSensors = new List<Sensor>();
            //var sensorIdList = new List<string>();
            //var redhots = new List<string>();//红色报警
            //var dicBadhots = new Dictionary<string, int>();//坏点记录
            //List<SensorBase> sbAddList = new List<SensorBase>();
            //#endregion  参数定义 

            //foreach (var cmodel in _list)
            //{
            //    var type = TypeParse.StrToInt(cmodel.type, 0);
            //    if (type == 1 || type == 2)//1温度、 2ID、
            //    {
            //        //cmodel.c_short
            //        int depth = 0;
            //        List<string> sensorList = new List<string>();

            //        #region  添加传感器
            //        if (cmodel.SensorId != null && cmodel.SensorId.Count > 0 && string.IsNullOrEmpty(cmodel.temp))
            //        {
            //            var sublayer = cRepository.Get(g => g.CPUId == cmodel.m_cpuid);
            //            if (sublayer == null)
            //                continue;
            //            collectid = cmodel.m_cpuid + "No";
            //            #region 更新采集器
            //            updateList.Add(new Collector()
            //            {
            //                GuidID = sublayer.GuidID,
            //                CPUId = cmodel.m_cpuid,
            //                InstallDate = datenowStr,
            //                SensorIdArr = cmodel.sum > 0 ? string.Join(",", cmodel.SensorId) : null
            //            });
            //            #endregion

            //            sensorList = cmodel.SensorId;
            //            var sblist = sbRepository.GetList(g => sensorList.Contains(g.SCpu));//传感器的基础信息表，用于定位
            //            Dictionary<string, int> linelist = new Dictionary<string, int>();
            //            string lineCode = collectid + (linelist.Count + 1);
            //            foreach (var f in sensorList)
            //            {
            //                #region 生成传感线
            //                var sequen = ++depth;
            //                var fsensor = f;

            //                //来自备案的信息
            //                var baseinfo = sblist.FirstOrDefault(d => d.SCpu == f);
            //                if (baseinfo != null)
            //                {
            //                    lineCode = baseinfo.SLineCode;
            //                    sequen = baseinfo.SSequen ?? 0;
            //                }
            //                else
            //                {
            //                    if (f.StartsWith("3"))
            //                    {
            //                        fsensor = "2" + fsensor.Substring(1);
            //                        lineCode = collectid + (linelist.Count + 1);
            //                        depth = 0;
            //                    }
            //                }

            //                if (linelist == null || !linelist.ContainsKey(lineCode))
            //                {
            //                    var count = linelist == null ? 0 : linelist.Count;
            //                    linelist.Add(lineCode, count + 1);
            //                }

            //                int lSequen = -1;

            //                if (sublayer.HeapNumber.IndexOf('T') > -1 || sublayer.HeapNumber.IndexOf('Q') > -1)
            //                {
            //                    var line = lbRepository.Get(g => g.HeapNumber == sublayer.HeapNumber && g.LineCode == lineCode);
            //                    if (line != null)
            //                        lSequen = line.LSequence.Value;
            //                }
            //                #endregion 生成传感线

            //                int x = sequen, y = lSequen != -1 ? lSequen : (linelist == null ? 1 : linelist.FirstOrDefault(d => d.Key == lineCode).Value), z = sublayer.Sublayer;
            //                //var guidKey = cmodel.m_cpuid + "_" + lineCode + "_" + sequen;
            //                var guidKey = sublayer.HeapNumber + "_" + x + "_" + y + "_" + z;
            //                var guid = Utils.MD5(guidKey);

            //                addSensors.Add(new Sensor()
            //                {
            //                    CRC = f,
            //                    SensorId = fsensor,
            //                    IsActive = 1,
            //                    Collector = cmodel.m_cpuid,
            //                    Label = lineCode,
            //                    Direction_X = x,
            //                    Direction_Y = y,
            //                    Direction_Z = z,
            //                    Sequen = sequen,
            //                    IsBad = 0,
            //                    GuidID = guid
            //                });
            //            }

            //            //添加传感器
            //            //var sKey = new string[] { "GuidID" };
            //            //var uKey = new string[] { "Collector", "SensorId", "Label" };
            //            //reint = sRepository.AddUpdate(addSensors, sKey, uKey);
            //            reint = new Data.Context.DbEntity().AddUpdateSensor(addSensors);

            //            //批量添加采集器
            //            var selectKey = new string[] { "GuidID" };
            //            var updateKey = new string[] { "CPUId", "SensorIdArr", "InstallDate" };
            //            //reint = cRepository.AddUpdate(updateList, selectKey, updateKey);
            //            reint = new Data.Context.DbEntity().AddUpdateCollector(updateList);


            //        }
            //        #endregion

            //        #region 收集温度
            //        if (!string.IsNullOrEmpty(cmodel.c_short) && !string.IsNullOrEmpty(cmodel.temp))
            //        {
            //            var c_short = TypeParse._16NAC_To_10NSC(cmodel.c_short);
            //            var rInfo = rRepository.Get(g => g.ID == c_short);
            //            if (rInfo == null)
            //            {
            //                Utils.PrintLog("不存在该收集器的信息，Receiver:" + c_short, "AddTemp");
            //                return false;
            //            }
            //            var wh_number = rInfo.W_Number;
            //            var g_number = rInfo.Number;

            //            if (sensorList == null || sensorList.Count <= 0)
            //            {
            //                var info = cRepository.Get(g => g.CPUId == cmodel.m_cpuid);
            //                if (info != null)
            //                    sensorList = info.SensorIdArr.Split(',').ToList();
            //            }

            //            decimal cjqTemp = 0;
            //            if (sensorList.Count > 0)
            //            {
            //                #region chuanganqi temp
            //                int i = 0;
            //                for (; i < sensorList.Count; i++)
            //                {
            //                    var f = sensorList[i];
            //                    if (f.StartsWith("3"))
            //                        f = "2" + f.Substring(1);

            //                    var temp = Comm.SysApi.Tools.GetTemp(cmodel.temp, i);

            //                    #region 坏点
            //                    if (temp == (decimal)63.75 || temp == (decimal)0)//坏点
            //                    {
            //                        //badhots.Add(f);
            //                        dicBadhots.Add(f, 1);
            //                        continue;
            //                    }
            //                    else
            //                    {
            //                        dicBadhots.Add(f, 0);
            //                    }
            //                    #endregion 坏点

            //                    #region 高温红色报警

            //                    if (temp >= (decimal)35 && temp < (decimal)63.75)
            //                    {
            //                        redhots.Add(f);
            //                    }

            //                    #endregion

            //                    cjqTemp += temp;
            //                    addTemp.Add(new Temperature()
            //                    {
            //                        PId = f,
            //                        StampTime = datenowStr,
            //                        UpdateTime = datenow,
            //                        Temp = temp,
            //                        RealHeart = 0,
            //                        Type = 0,
            //                        WH_Number = wh_number,
            //                        G_Number = g_number
            //                    });
            //                }
            //                cjqTemp = cjqTemp / i;
            //                #endregion

            //                #region caijiqi temp
            //                add_CTemp.Add(new Temperature()
            //                {
            //                    PId = cmodel.m_cpuid,
            //                    StampTime = datenowStr,
            //                    UpdateTime = datenow,
            //                    Type = 1,//0传感器、1采集器、2收集器（室外）
            //                    RealHeart = 0,
            //                    Temp = cjqTemp,
            //                    WH_Number = wh_number,
            //                    G_Number = g_number
            //                });
            //                #endregion

            //            }
            //        }
            //        sensorIdList.AddRange(sensorList);

            //        #region 批量插入温度
            //        reint = tRepository.AddUpdate(addTemp, p => p.RealHeart == 0 && sensorIdList.Contains(p.PId) && p.Type == 0
            //            , "RealHeart", 1, "StampTime");

            //        //同时更新采集器的温度
            //        var cpuIds = _list.Select(s => s.m_cpuid).ToList();
            //        reint = tRepository.AddUpdate(add_CTemp, p => p.RealHeart == 0 && cpuIds.Contains(p.PId) && p.Type == 1
            //            , "RealHeart", 1, "");

            //        #endregion

            //        #endregion

            //        #region 更新坏点

            //        //更新采集器的坏点数
            //        new Data.Context.DbEntity().UpdateBadHot(dicBadhots);
            //        //Action<Dictionary<string, int>> pushMsg = new Data.Context.DbEntity().UpdateBadHot;
            //        //pushMsg.BeginInvoke(dicBadhots, ar => pushMsg.EndInvoke(ar), null);
            //        #endregion

            //        #region 高温报警

            //        new Data.Context.DbEntity().UpdateRedHot(redhots);
            //        //Action<List<string>> pushRedHotMsg = new Data.Context.DbEntity().UpdateRedHot;
            //        //pushRedHotMsg.BeginInvoke(redhots, ar => pushRedHotMsg.EndInvoke(ar), null);

            //        #endregion
            //    }
            //    else if (type == 3)//3线编号,
            //    {
            //        List<SensorBase> sbList = new List<SensorBase>();
            //        var senArr = cmodel.SensorId;
            //        for (int i = 0; i < senArr.Count; i++)
            //        {
            //            sbList.Add(new SensorBase() { SLineCode = cmodel.label, SCpu = senArr[i], SSequen = ++i, StampTime = datenowStr });
            //        }
            //        sbAddList.AddRange(sbList);
            //    }

            //}

            //#region  线备案
            //sbRepository.Add(sbAddList);
            ////Action<List<SensorBase>> actionSbList = AddSensorBaseList;
            ////actionSbList.BeginInvoke(sbAddList, ar => actionSbList.EndInvoke(ar), null);

            //#endregion

            #endregion Test

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
