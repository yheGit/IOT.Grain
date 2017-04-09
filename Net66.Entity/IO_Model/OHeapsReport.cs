﻿using Net66.Entity.Models;
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

        public OHeapReport(List<string> ckfenji, List<Sensor> sensors, List<Temperature> temps)
        {
            var curSensors = sensors.Where(w => ckfenji.Contains(w.Collector)).Select(s => s.SensorId).ToList();
            var headTemps = temps.Where(a => curSensors.Contains(a.PId) && a.Type == 0).ToList();
            //Type 0传感器、1采集器、2收集器（仓外）
            var intemp = headTemps.Average(v => v.Temp);//0、堆位的平均温度， 1、堆位的仓内温度取，所在廒间的所有温度的平均值            
            var maxTemp = headTemps.Max(m => m.Temp);//堆位的最高温度，也就是传感器的最高温度
            var minTemp = headTemps.Min(m => m.Temp);//最低温度
            InSideTemperature = intemp;
            Maximumemperature = maxTemp;
            MinimumTemperature = minTemp;

        }
        /// <summary>
        /// 堆位编号
        /// </summary>
        public string Number { get; set; }
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

    }
}
