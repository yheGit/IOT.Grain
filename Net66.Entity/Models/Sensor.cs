using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Sensor
    {
        public int ID { get; set; }
        public string SensorId { get; set; }
        public string CRC { get; set; }
        public string Label { get; set; }
        /// <summary>
        /// Collector 的GUID
        /// </summary>
        public string Sequen { get; set; }
        public string Collector { get; set; }
        /// <summary>
        /// 传感器编号
        /// </summary>
        public Nullable<int> Direction_X { get; set; }
        /// <summary>
        /// 线号
        /// </summary>
        public Nullable<int> Direction_Y { get; set; }
        /// <summary>
        /// 码层
        /// </summary>
        public Nullable<int> Direction_Z { get; set; }
        public Nullable<decimal> MaxTemp { get; set; }
        public Nullable<decimal> MinTemp { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> IsBad { get; set; }
        public string GuidID { get; set; }
        public string InstallDate { get; set; }

    }
}
