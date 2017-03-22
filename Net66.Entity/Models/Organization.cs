using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Organization
    {
        public int ID { get; set; }
        public string OrgCode { get; set; }
        public string OrgName { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<byte> IsActive { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
    }
}
