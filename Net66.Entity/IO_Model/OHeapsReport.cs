using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class OHeapReport
    {
        public OHeapReport() { }

        public OHeapReport(List<string> ckfenji, List<Sensor> sensors, List<Temperature> temps, decimal outtemp)
        {
            var curSensors = sensors.Where(w => ckfenji.Contains(w.Collector)).Select(s => s.SensorId).ToList();
            var headTemps = temps.Where(a => curSensors.Contains(a.PId) && a.Type == 0).ToList();
            //Type 0传感器、1采集器、2收集器（仓外）
            //var intemp = Math.Round(headTemps.Average(v => v.Temp) ?? 0, 2);//0、堆位的平均温度， 1、堆位的仓内温度取，所在廒间的所有温度的平均值            
            var intemp = Math.Round(temps.Where(w=>w.RealHeart==0&&w.Type==2).Average(v => v.Temp) ?? 0, 2);
            var maxTemp = Math.Round(headTemps.Max(m => m.Temp) ?? 0, 2);//堆位的最高温度，也就是传感器的最高温度
            var minTemp = Math.Round(headTemps.Min(m => m.Temp) ?? 0, 2);//最低温度
            AverageTemperature = Math.Round((decimal)((maxTemp + minTemp) / 2), 2);
            InSideTemperature = intemp;
            Maximumemperature = maxTemp;
            MinimumTemperature = minTemp;
            if (temps == null || temps.Count <= 0)
                OutSideTemperature = 0;
            else
                OutSideTemperature = Math.Round(outtemp, 2);

        }
        /// <summary>
        /// 堆位编号
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
        /// 坏点数
        /// </summary>
        public Nullable<int> BadPoints { get; set; }
        /// <summary>
        /// 仓内温度
        /// </summary>
        public Nullable<decimal> InSideTemperature { get; set; }
        /// <summary>
        /// 仓外温度
        /// </summary>
        public Nullable<decimal> OutSideTemperature { get; set; }

        public int Sort { get; set; }//排序
    }
}
