using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT.RightsSys.Entity
{
    [Table("UserVxinInfo")]
    public class UserVxinInfo
    {
        [Key]
        [Display(Name = "主键", Order = 1)]
        public int ID { get; set; }
        public string XvinID { get; set; }
        public string UserLoginID { get; set; }
        public DateTime? SendTime { get; set; }

    }
}
