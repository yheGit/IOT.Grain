using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class HeapLine
    {
        public int ID { get; set; }
        public string HeapNumber { get; set; }
        public Nullable<int> Counts { get; set; }
        public Nullable<int> Sort { get; set; }

    }
}
