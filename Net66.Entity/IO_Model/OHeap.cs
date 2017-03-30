using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class OHeap
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> PID { get; set; }
        public Nullable<int> WH_ID { get; set; }
        public string WH_Number { get; set; }
        public Nullable<int> BadPoints { get; set; }
        public Nullable<decimal> AverageTemperature { get; set; }
        public Nullable<decimal> AverageHumidity { get; set; }
        public Nullable<decimal> MaxiTemperature { get; set; }
        public Nullable<decimal> MinTemperature { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsActive { get; set; }
        public List<OSensor> SensorList { get; set; }

    }
}
