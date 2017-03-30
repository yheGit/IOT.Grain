using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class OBBD
    {
        public string Type { get; set; }

        //public int Fcount { get; set; }

        //public int Gcount { get; set; }

        public Dictionary<string, object[]> Dic { get; set; }

    }
}
