using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Collector
    {
        public int ID { get; set; }
        public string CPUId { get; set; }
        //public string H_Number { get; set; }
        public int R_Code { get; set; }
        public string InstallDate { get; set; }
        public Nullable<int> UserId { get; set; }//
        public string HeapNumber { get; set; }//
        public Nullable<decimal> Voltage { get; set; }
        public int IsActive { get; set; }
        public string SensorIdArr { get; set; }
        public int Sublayer { get; set; }
        public string GuidID { get; set; }
    }
}
