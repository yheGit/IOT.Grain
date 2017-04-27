using Net66.Comm;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;
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
                    var datenow = Utils.GetServerDateTime();
                    var datenowStr = datenow.ToString();

                    var info = db.LineBase.FirstOrDefault(f => f.HeapNumber == model.HeapNumber && f.LSequence == model.Sort);
                    if (info != null)
                    {
                        if (info.LineCode != model.LineCode)
                        {
                            info.LineCode = model.LineCode;
                            info.StampTime = datenowStr;
                            db.LineBase.Attach(info);
                            db.Entry(info).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                       
                        db.LineBase.Add(new LineBase()
                        {
                            HeapNumber = model.HeapNumber,
                            LineCode = model.LineCode,
                            LSequence = model.Sort,
                            StampTime = datenowStr
                        });
                    }
                }
                return db.SaveChanges() > 0;
            }
        }



    }
}
