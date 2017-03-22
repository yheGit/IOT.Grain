using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class UserInfo
    {
        public int ID { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string LoginPwd { get; set; }
        public string OrgId { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string mobile { get; set; }
        public string tel { get; set; }
        public Nullable<int> RoleId { get; set; }
    }
}
