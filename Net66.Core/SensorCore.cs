using Net66.Core.Interface;
using Net66.Data.Interface;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/******************************************
*Creater:yhw[96160]
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

        public SensorCore(IGrainRepository<Sensor> _sRepository, IGrainRepository<Collector> _cRepository, IGrainRepository<Temperature> _tRepository)
        {
            sRepository = _sRepository;
            cRepository = _cRepository;
            tRepository = _tRepository;
        }

        public List<Net66.Entity.IO_Model.OSensor> GetSensorList(int id)
        {
            var cList = cRepository.GetList(g => g.PID == id);
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
            var tList=tRepository.GetList(g => IdList.Contains(g.SensorId) && g.RealHeart == 1);

            return reList.Select(s => new Net66.Entity.IO_Model.OSensor(tList.FirstOrDefault(f => f.SensorId == s.SensorId))
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


    }
}
