using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
   public class IWareHouse
    {
        /// <summary>
        /// 更新时用
        /// </summary>
        public int ID { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Nullable<int> Type { get; set; }
        public string UserId { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> depth { get; set; }
        public Nullable<int> IsActive { get; set; }
        public int Sort { get; set; }//排序

    }
}
