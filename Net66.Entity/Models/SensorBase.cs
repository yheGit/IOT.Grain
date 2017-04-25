using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class SensorBase
    {
        public int ID { get; set; }
        public string SCpu { get; set; }
        public Nullable<int> SSequen { get; set; }
        public string SLineCode { get; set; }      
        public string StampTime { get; set; }
        public Nullable<int> LSequen { get; set; }

        public string GuidID { get; set; }

    }
}
