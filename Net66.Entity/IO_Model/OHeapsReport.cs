using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class OHeapReport
    {
        public string Number { get; set; }
        public string UserId { get; set; }
        public Nullable<decimal> AverageTemperature { get; set; }
        public Nullable<decimal> Maximumemperature { get; set; }
        public Nullable<decimal> MinimumTemperature { get; set; }
        public Nullable<int> BadPoints { get; set; }

    }
}
