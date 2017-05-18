using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT.RightsSys.Entity
{
    [Table("SendMsg")]
    public class SendMsg
    {
        [Display(Name = "编号")]
        [Key]
        public string GUID { get; set; }
        [Display(Name = "手机号")]
        public string Tel { get; set; }
        [Display(Name = "发送时间")]
        public DateTime? SendTime { get; set; }
        [Display(Name = "内容")]
        public string Msg { get; set; }
        /// <summary>
        /// 1报警，2短信验证
        /// </summary>
        [Display(Name = "类型")]
        public Nullable<int> Type { get; set; }

    }
}
