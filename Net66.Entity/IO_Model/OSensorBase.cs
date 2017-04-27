using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class OSensorBase
    {
        public List<int> LineCount { get; set; }

        public List<ISensorBase> LineList { get; set; }
    }
}
