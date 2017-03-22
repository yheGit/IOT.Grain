using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Granary
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public string F_Number { get; set; }
        public Nullable<decimal> AverageTemperature { get; set; }
        public Nullable<decimal> AverageHumidity { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<byte> IsActive { get; set; }
    }
}
