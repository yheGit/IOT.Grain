﻿using Net66.Entity.Models;
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

        public OGrainsReport(List<Temperature> temps, List<Humidity> humtys, List<Collector> collectors = null)
        {
            //0传感器、1采集器、2收集器（室内）、3收集器（室外）
            var clist = temps.Where(w => w.Type == 0).ToList();
            Maximumemperature = clist.Max(m => m.Temp);//最高温度（整个粮仓）
            MinimumTemperature = clist.Min(m => m.Temp);//最低温度（整个粮仓）
            //AverageTemperature = Math.Round(clist.Average(a => a.Temp)??0,2);//pingjunwendu
            //var snmodel = temps.Where(w => w.Type == 2).Average(a => a.Temp);
            //InSideTemperature = Math.Round(snmodel ?? 0,2);
            InSideHumidity = humtys.Where(w => w.Type == 0).Max(m=>m.Humility);////0仓内湿度，1仓外湿度
            OutSideHumidity = humtys.FirstOrDefault(w => w.Type == 1).Humility;
            var swmodel = temps.Where(w => w.Type == 2).FirstOrDefault() ?? new Temperature();
            OutSideTemperature = swmodel.Temp;
            if (collectors != null)
                BadPoints = collectors.Sum(s => s.BadPoints);
        }
        /// <summary>
        /// 仓号
        /// </summary>
        public string Number { get; set; }
        //public string Location { get; set; }
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
        /// 粮仓类型 1楼房仓，2平方仓，3立筒仓
        /// </summary>
        public int Type { get; set; }

    }
}
