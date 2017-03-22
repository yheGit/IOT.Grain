﻿using Net66.Comm;
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
        private static IGrainRepository<Temperature> tRepository;
        private static IGrainRepository<Sensor> sRepository;
        private static IGrainRepository<SensorBase> sbRepository;

        public CollectorCore(IGrainRepository<Collector> _cRepository, IGrainRepository<Temperature> _tRepository
            , IGrainRepository<Sensor> _sRepository, IGrainRepository<SensorBase> _sbRepository)
        {
            cRepository = _cRepository;
            tRepository = _tRepository;
            sRepository = _sRepository;
            sbRepository = _sbRepository;
        }

        public bool Install(IReceiver _entity)
        {
            var reint = 0;
            var datenow = Utils.GetServerTime();
            var c_short = TypeParse._16NAC_To_10NSC(_entity.c_short);
            var guidKey = c_short + "_" + _entity.heap + "_" + _entity.sublayer;
            var guid = Utils.MD5(guidKey);
            var addCollecters = new List<Collector>() {new Collector(){
                    GuidID=guid,
                    CPUId = _entity.c_cpuid,
                    InstallDate = datenow,               
                    H_Number=_entity.heap,
                    R_Code=c_short,
                    Sublayer=_entity.sublayer,
                    //UserId=null,
                    //Voltage=null,
                    //SensorIdArr=null,
                    IsActive = 1
                } };

            var selectKey = new string[] { "GuidID" };
            var updateKey = new string[] { "CPUId", "IsActive" };
            reint = cRepository.AddUpdate(addCollecters, selectKey, updateKey, "InstallDate");

            return reint > 0;

        }

        public bool AddTemp(List<ICollector> _list)
        {
            if (_list == null || _list.Count < 0)
                return false;
            var datenow = Utils.GetServerTime();
            var addTemp = new List<Temperature>();
            var updateList = new List<Collector>();
            var addSensors = new List<Sensor>();
            foreach (var cmodel in _list)
            {
                #region 更新采集器
                if (cmodel.SensorId != null && cmodel.SensorId.Count > 0)
                {
                    updateList.Add(new Collector()
                    {
                        CPUId = cmodel.m_cpuid,
                        InstallDate = datenow,
                        SensorIdArr = cmodel.sum > 0 ? string.Join(",", cmodel.SensorId) : null
                    });
                }
                #endregion

                List<string> sensorList = new List<string>();

                #region  添加传感器
                if (cmodel.SensorId != null && cmodel.SensorId.Count > 0)
                {
                    sensorList = cmodel.SensorId;
                    var sublayer = cRepository.Get(g=>g.CPUId==cmodel.m_cpuid);
                    if (sublayer == null)
                        continue;
                    var sblist = sbRepository.GetList(g => sensorList.Contains(g.SCpu));
                    Dictionary<string, int> linelist = new Dictionary<string, int>();
                    foreach (var f in sensorList)
                    {
                        var baseinfo = sblist.FirstOrDefault(d => d.SCpu == f);
                        if (baseinfo == null)
                            continue;
                        var lineCode = baseinfo.SLineCode;
                        if (linelist == null || !linelist.ContainsKey(lineCode))
                        {
                            var count = linelist == null ? 0 : linelist.Count;
                            linelist.Add(lineCode, count + 1);
                        }                       
                        
                        var sequen = baseinfo.SSequen;
                        var guidKey = cmodel.m_cpuid + "_" + lineCode + "_" + sequen;
                        var guid = Utils.MD5(guidKey);
                        addSensors.Add(new Sensor()
                        {
                            SensorId = f,
                            IsActive = 1,
                            Collector = cmodel.m_cpuid,
                            Label = lineCode,
                            Direction_X = sequen,
                            Direction_Y = linelist.FirstOrDefault(d => d.Key == lineCode).Value,
                            Direction_Z = sublayer.Sublayer,
                            Sequen=sequen,
                            GuidID = guid 
                        });
                    }
                }
                #endregion

                #region 温度
                if (sensorList == null || sensorList.Count <= 0)
                {
                    var info = cRepository.Get(g => g.CPUId == cmodel.m_cpuid);
                    if (info != null)
                        sensorList = info.SensorIdArr.Split(',').ToList();
                }
               
                if (sensorList.Count > 0)
                {
                    int i = 0;
                    sensorList.ForEach(f =>
                    {
                        addTemp.Add(new Temperature()
                        {
                            SensorId = f,
                            StampTime = datenow,
                            Temp = Comm.SysApi.Tools.GetTemp(cmodel.temp, i)
                        }); i++;
                    });
                }
                #endregion
            }

            var selectKey = new string[] { "CPUId" };
            var updateKey = new string[] { "SensorIdArr", "InstallDate" };
            var reint = cRepository.AddUpdate(updateList, selectKey, updateKey);//批量添加采集器
            reint = tRepository.Add(addTemp);//批量添加温度
            var sKey=new string[] { "GuidID" };
            var uKey = new string[] { "Collector", "GuidID" };
            reint = sRepository.AddUpdate(addSensors, sKey, uKey);//添加传感器
            return reint > 0;
        }
    }
}
