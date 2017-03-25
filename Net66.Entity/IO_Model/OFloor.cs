using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class OFloor
    {
        public int ID { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public string WH_Number { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsActive { get; set; }

        public List<Granary> GranaryList { get; set; }

    }
}
