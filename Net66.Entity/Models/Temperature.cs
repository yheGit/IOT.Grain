using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Temperature
    {
        public int ID { get; set; }
        public Nullable<decimal> Temp { get; set; }
        public string StampTime { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        /// <summary>
        /// 1：传感器ID、分机ID、主机ID；2：DAY(最近24小时)、MONTH(最近一个月)、YEAR(最近一年)
        /// </summary>
        public string PId { get; set; }
        public Nullable<int> RealHeart { get; set; }
        /// <summary>
        /// 0传感器（单点温度）、1采集器（堆位平均温）、2收集器（仓内温度）、3收集器（室外）、-4删除
        /// </summary>
        public Nullable<int> Type { get; set; }
        public string WH_Number { get; set; }
        public string G_Number { get; set; }
        public string H_Number { get; set; }

        public string TimeFlag { get; set; }

    }
}
