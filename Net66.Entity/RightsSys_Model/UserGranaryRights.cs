using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOT.RightsSys.Entity
{
    [Table("UserGranaryRights")]
    public  class UserGranaryRights
    {
        [Key]
        [Display(Name = "主键", Order = 1)]
        public int ID { get; set; }
        public string UserId { get; set; }

        public string GranaryNumber { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
