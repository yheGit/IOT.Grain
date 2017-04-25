using Net66.Core.Interface;
using Net66.Data.Interface;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/******************************************
*Creater:yhw[]
*CreatTime:
*Description:
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:
*******************************************/
namespace Net66.Core
{
    public class SensorCore : ISensorCore
    {
        private static IGrainRepository<Collector> cRepository;
        private static IGrainRepository<Temperature> tRepository;
        private static IGrainRepository<Sensor> sRepository;
        private static IGrainRepository<SensorBase> sbRepository;
        private static IGrainRepository<HeapLine> hlRepository;

        public SensorCore(IGrainRepository<Sensor> _sRepository, IGrainRepository<Collector> _cRepository
            , IGrainRepository<Temperature> _tRepository, IGrainRepository<SensorBase> _sbRepository, IGrainRepository<HeapLine> _hlRepository)
        {
            sRepository = _sRepository;
            cRepository = _cRepository;
            tRepository = _tRepository;
            sbRepository = _sbRepository;
            hlRepository = _hlRepository;
        }

        public List<Net66.Entity.IO_Model.OSensor> GetSensorList(string id)
        {
            var cList = cRepository.GetList(g => g.HeapNumber == id);
            if (cList == null)
                return null;
            var cpuIdList = cList.Select(s => s.CPUId).ToList();
            var reList = sRepository.GetList(g => cpuIdList.Contains(g.Collector) && g.IsActive == 1);
            if (reList == null)
                return null;
            var IdList = reList.Select(s => s.SensorId);
            //var ids = string.Empty;
            //foreach (var sId in IdList)
            //    ids += "'" + sId + "',";
            //string sql = "SELECT max(StampTime),[Temp] FROM [Grain_IOT].[dbo].[Temperature] where [SensorId] in ("
            //+ ids.TrimEnd(',')
            //+ ") group by [Temp] ";

            //var dt = sRepository.QueryByTablde(sql);
            //if (dt != null)
            //{
            //     dt.Rows.Find("").
            //}
            var tList=tRepository.GetList(g => IdList.Contains(g.PId) && g.RealHeart == 1&&g.Type==0);

            return reList.Select(s => new Net66.Entity.IO_Model.OSensor(tList.FirstOrDefault(f => f.PId == s.SensorId))
            {
                ID = s.ID,
                Collector = s.Collector,
                CRC = s.CRC,
                Direction_X = s.Direction_X,
                Direction_Y = s.Direction_Y,
                Direction_Z = s.Direction_Z,
                GuidID = s.GuidID,
                IsActive = s.IsActive,
                Label = s.Label,
                MaxTemp = s.MaxTemp,
                MinTemp = s.MinTemp,
                //RealTemp = tList.FirstOrDefault(f=>f.SensorId==s.SensorId),
                SensorId = s.SensorId,
                Sequen = s.Sequen,
                UserId = s.UserId
            }).ToList();
        }

        /// <summary>
        /// genju tongdui bianhao huoqu ,chuanguanxian shuliang(anzhuang zhiqian)
        /// </summary>
        public List<int> GetHeapLineCount(string heapNumber)
        {
            var hllist = hlRepository.GetList(g => heapNumber.Equals(g.HeapNumber)) ?? new List<HeapLine>();
            return hllist.OrderBy(o => o.Sort).Select(s => s.Counts.Value).ToList();

        }

        /// <summary>
        /// 更新传感线的基础信息
        /// </summary>
        public bool UpdateSensorBaseList(List<ISensorBase> list)
        {
            bool rebit = false;

            rebit= new Data.Context.DbEntity().UpdateSensorBase(list);

            return rebit;
        }


    }
}
