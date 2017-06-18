using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class SensorBase
    {
        public int ID { get; set; }
        /// <summary>
        /// 传感器的真实ID
        /// </summary>
        public string SCpu { get; set; }
        public Nullable<int> SSequen { get; set; }
        public string SLineCode { get; set; }
        public string StampTime { get; set; }
        /// <summary>
        /// 多线的排号
        /// </summary>
        public int SCount { get; set; }
        /// <summary>
        /// 传进来的原始传感器ID
        /// </summary>
        public string SCpuOrg { get; set; }
    }
}
