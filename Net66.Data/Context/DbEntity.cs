using Net66.Comm;
using Net66.Entity.IO_Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Data.Context
{
    public class DbEntity
    {
        /// <summary>
        /// 更新收集器坏点 2017-04-12 21:17:05
        /// </summary>
        public bool UpdateCollectorBadHot(Dictionary<string, int> dic)
        {
            if (!dic.Any())
                return false;
            var cpulist = dic.Keys;
            using (var db = new GrainContext())
            {
                var collectors = db.Collectors.Where(w => cpulist.Contains(w.CPUId) && w.IsActive == 1).ToList() ?? new List<Entity.Models.Collector>();
                foreach (var model in collectors)
                {
                    model.BadPoints += dic[model.CPUId];
                    db.Collectors.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                }
                return db.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 更新基础表中线的序号，
        /// 更新已知筒堆序号下的 传感线编号
        /// </summary>
        public bool UpdateSensorBase(List<ISensorBase> list)
        {
            using (var db = new GrainContext())
            {

                foreach (var model in list)
                {
                    var guidKey = model.HeapNumber + "_" + model.Sort;
                    var guid = Utils.MD5(guidKey);

                    var info = db.SensorBase.FirstOrDefault(f => f.GuidID == guid);
                    if (info != null)
                    {
                        if (info.SLineCode != model.LineCode)
                            info.SLineCode = model.LineCode;
                        db.SensorBase.Add(info);
                    }
                    else
                    {
                        var info2 = db.SensorBase.FirstOrDefault(f => f.SLineCode == model.LineCode);
                        if (info2 == null)
                            continue;
                        info2.GuidID = guid;
                        info2.LSequen = model.Sort;
                        db.SensorBase.Attach(info2);
                        db.Entry(info2).State = EntityState.Modified;
                    }
                }
                return db.SaveChanges() > 0;
            }
        }



    }
}
