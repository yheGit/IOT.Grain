using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Temperature
    {
        public int ID { get; set; }
        public Nullable<decimal> Temp { get; set; }
        public string StampTime { get; set; }
        public string SensorId { get; set; }
        public Nullable<int> RealHeart { get; set; }

    }
}
