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

        public OGrainsReport(List<Temperature> temps, List<Humidity> humtys, int _type, List<string> sidlist = null)
        {
            //0传感器、1采集器、2收集器（室内）、3收集器（室外）
            var clist = temps.Where(w => w.Type == 0 && w.RealHeart == 0).ToList();
            if (sidlist != null)
                clist = clist.Where(w => sidlist.Contains(w.PId)).ToList();
            Maximumemperature = Math.Round(clist.Where(w => w.Type == 0).Max(m => m.Temp) ?? 0, 2);//最高温度（整个粮仓）
            MinimumTemperature = Math.Round(clist.Where(w => w.Type == 0).Min(m => m.Temp) ?? 0, 2);//最低温度（整个粮仓）
            AverageTemperature = Math.Round((decimal)((Maximumemperature + MinimumTemperature) / 2), 2);
            var insidehum = humtys.Where(w => w.Type == 0 && w.RealHeart == 0).Max(m => m.Humility);
            if (_type == 2)
            {
                if (insidehum != null)
                    InSideHumidity = Math.Round(humtys.Where(w => w.Type == 0 && w.RealHeart == 0).Max(m => m.Humility) ?? 0, 2);////0仓内湿度，1仓外湿度
                InSideTemperature = 0;
            }
            OutSideHumidity = Math.Round(humtys.FirstOrDefault(w => w.Type == 1 && w.RealHeart == 0) == null ? 0 : humtys.FirstOrDefault(w => w.Type == 1 && w.RealHeart == 0).Humility ?? 0, 2);
            var swmodel = temps.Where(w => w.Type == 3).FirstOrDefault() ?? new Temperature();
            OutSideTemperature = Math.Round(swmodel.Temp ?? 0, 2);
        }
        /// <summary>
        /// 仓号
        /// </summary>
        public string Number { get; set; }
        public string Name { get; set; }//
        public string UserId { get; set; }
        /// <summary>
        /// 最高温度
        /// </summary>
        public Nullable<decimal> Maximumemperature { get; set; }
        /// <summary>
        /// 最低温度
        /// </summary>
        public Nullable<decimal> MinimumTemperature { get; set; }
        /// <summary>
        /// 平均温度
        /// </summary>
        public Nullable<decimal> AverageTemperature { get; set; }
        /// <summary>
        /// 仓内温度
        /// </summary>
        public Nullable<decimal> InSideTemperature { get; set; }
        /// <summary>
        /// 仓外温度
        /// </summary>
        public Nullable<decimal> OutSideTemperature { get; set; }
        /// <summary>
        /// 仓内湿度
        /// </summary>
        public Nullable<decimal> InSideHumidity { get; set; }
        /// <summary>
        /// 仓外湿度
        /// </summary>
        public Nullable<decimal> OutSideHumidity { get; set; }
        //public Nullable<System.DateTime> StampTime { get; set; }
        /// <summary>
        /// 坏点数
        /// </summary>
        public Nullable<int> BadPoints { get; set; }
        /// <summary>
        /// 粮仓类型 1楼房仓，2平方仓(气膜仓)，3立筒仓（浅圆仓）
        /// </summary>
        public int Type { get; set; }
        public int Sort { get; set; }//排序

    }
}
