using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Floor
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public Nullable<int> UserId { get; set; }
        public string WH_Number { get; set; }
        public Nullable<byte> IsActive { get; set; }
    }
}
