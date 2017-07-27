using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Humidity
    {
        public int ID { get; set; }
        public Nullable<decimal> Humility { get; set; }
        public Nullable<decimal> Temp { get; set; }
        public string StampTime { get; set; }
        public int ReceiverId { get; set; }
        public Nullable<int> Type { get; set; }
        public string WH_Number { get; set; }
        public string G_Number { get; set; }
        public Nullable<int> RealHeart { get; set; }

        /// <summary>
        /// PID
        /// </summary>
        public string H_Number { get; set; }

        public string TimeFlag { get; set; }
    }
}
