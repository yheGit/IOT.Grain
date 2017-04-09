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
        public string PId { get; set; }
        public Nullable<int> RealHeart { get; set; }
        public Nullable<int> Type { get; set; }
        public string WH_Number { get; set; }
        public string G_Number { get; set; }

    }
}
