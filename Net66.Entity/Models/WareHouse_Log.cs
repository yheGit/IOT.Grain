using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class WareHouse_Log
    {
        public int ID { get; set; }
        public Nullable<int> WH_ID { get; set; }
        public string WH_Content { get; set; }
        public string ProducePlace { get; set; }
        public Nullable<System.DateTime> ReceiptDate { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public Nullable<decimal> Moisture { get; set; }
        public Nullable<byte> StoreType { get; set; }
        public Nullable<decimal> Incomplete { get; set; }
        public Nullable<byte> StoreLevel { get; set; }
        public Nullable<decimal> Capacity { get; set; }
        public Nullable<decimal> Impurity { get; set; }
        public string Mgr { get; set; }
    }
}
