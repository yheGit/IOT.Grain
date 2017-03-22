using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Alarm
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public string ShortCode { get; set; }
        public string CpuId { get; set; }
        public string SensorId { get; set; }
        public string DateValue { get; set; }
        public Nullable<System.DateTime> StampTime { get; set; }
    }
}
