using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class WareHouse
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
        public Nullable<decimal> InSideTemperature { get; set; }
        public Nullable<decimal> OutSideTemperature { get; set; }
        public Nullable<System.DateTime> StampTime { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> BadPoints { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> depth { get; set; }
        public string OrgId { get; set; }
        public string OrgCode { get; set; }
        public int? Sort { get; set; }//≈≈–Ú

    }
}
