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
        public bool UpdateCollectorBadHot(Dictionary<string,int> dic)
        {
            if (!dic.Any())
                return false;
            var cpulist = dic.Keys;
            using (var db = new GrainContext())
            {
                var collectors= db.Collectors.Where(w => cpulist.Contains(w.CPUId) && w.IsActive == 1).ToList()??new List<Entity.Models.Collector>();
                foreach (var model in collectors)
                {
                    model.BadPoints += dic[model.CPUId];
                    db.Collectors.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                }
                return db.SaveChanges() > 0;
            }
        }

    }
}
