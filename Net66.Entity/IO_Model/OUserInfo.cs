using IOT.RightsSys.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class OUserInfo
    {

        public OUserInfo()
		{}

        public OUserInfo(Sys_Role role, Sys_Department org)
        {
            if (role != null)
                RoleName = role.Name;
            if (org != null)
                RoleName = org.Name;
        }

        public string Id
		{
			set;get;
		}

        public string LoginID
		{
			set;get;
		}

        public string NickName
		{
			set;get;
		}

        public string Password
		{
			set;get;
		}

        public string Sex
		{
			set;get;
		}

        public string DepartmentId
		{
			set;get;
		}

        public string RoleId
		{
			set;get;
		}

       
        public string TelPhone
		{
			set;get;
		}

        public string PhoneNumber
		{
		set;get;
		}

        public string EmailAddress
		{
			set;get;
		}

        public string State
		{
			set;get;
		}

        public string Address
		{
			set;get;
		}

        public string Remark
		{
			set;get;
		}

        public int? IsShow
        {
            set;get;
        }

        public string OrgId {
           set;get;
        }
        public string OrgCode {
            set;get;
        }

        public string PhoneInfo
        {
           set;get;
        }

        public string RoleName { get; set; }

        public string OrgName { get; set; }

    }
}
