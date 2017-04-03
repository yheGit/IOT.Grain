using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class OGrainsReport
    {
        public OGrainsReport() { }

        public OGrainsReport(List<Temperature> temps, List<Collector> collectors = null)
        {
            var clist = temps.Where(w => w.Type == 0).ToList();
            Maximumemperature = clist.Max(m => m.Temp);//zuigaowendu
            MinimumTemperature = clist.Min(m => m.Temp);//zuidiwendu
            AverageTemperature = Math.Round(clist.Average(a => a.Temp)??0,2);//pingjunwendu
            var snmodel = temps.Where(w => w.Type == 2).Average(a => a.Temp);
            InSideTemperature = Math.Round(snmodel ?? 0,2);
            var swmodel = temps.Where(w => w.Type == 3).FirstOrDefault() ?? new Temperature();
            OutSideTemperature = swmodel.Temp;
            if (collectors != null)
                BadPoints = collectors.Sum(s => s.BadPoints);
        }
        public string Number { get; set; }
        //public string Location { get; set; }
        public string UserId { get; set; }
        public Nullable<decimal> AverageTemperature { get; set; }
        public Nullable<decimal> Maximumemperature { get; set; }
        public Nullable<decimal> MinimumTemperature { get; set; }
        public Nullable<decimal> InSideTemperature { get; set; }
        public Nullable<decimal> OutSideTemperature { get; set; }
        //public Nullable<System.DateTime> StampTime { get; set; }
        public Nullable<int> BadPoints { get; set; }
        /// <summary>
        /// 粮仓类型 1楼房仓，2平方仓，3立筒仓
        /// </summary>
        public int Type { get; set; }

    }
}
