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
    }
}
