using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT.RightsSys.Entity
{
    [Table("PushAlarmMsg")]
    public class PushAlarmMsg
    {

        [Key]
        [Display(Name = "主键", Order = 1)]
        public int ID { get; set; }
        public string GuidID { get; set; }
        public string LoginID { get; set; }
        public DateTime? SendTime { get; set; }
        public string MsgConn { get; set; }
        public int? IsSend { get; set; }
        public int? Type { get; set; }

    }
}
