using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Receiver
    {
        public int ID { get; set; }
        public string CPUId { get; set; }
        public string Number { get; set; }
        public string G_Number { get; set; }
        public string F_Number { get; set; }
        public string W_Number { get; set; }
        public string GuidID { get; set; }
        public string IPAddress { get; set; }
        public string InstallDate { get; set; }
        public Nullable<decimal> Temperature { get; set; }
        public Nullable<decimal> Humidity { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsActive { get; set; }
        //public Nullable<int> RandKey { get; set; }
    }
}
