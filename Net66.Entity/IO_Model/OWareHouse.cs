using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
   public class OWareHouse
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Nullable<int> Type { get; set; }
        public string UserId { get; set; }
        public Nullable<decimal> AverageTemperature { get; set; }
        public Nullable<decimal> Maximumemperature { get; set; }
        public Nullable<decimal> MinimumTemperature { get; set; }
        public Nullable<DateTime> StampTime { get; set; }
        public Nullable<int> IsActive { get; set; }
        public List<OFloor> Floors { get; set; }

    }
}
