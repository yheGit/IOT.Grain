
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT.RightsSys.Entity
{
	/// <summary>
	/// Sys_MenuRoleOperation:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    [Table("Sys_MenuRoleOperation")]
    public class Sys_MenuRoleOperation
	{
		public Sys_MenuRoleOperation()
		{}
		#region Model
		private string _guidid;
		private string _menuid;
		private string _operationid;
		private string _roleid;

        [Display(Name = "主键", Order = 1)]
        public string GuidID
		{
			set{ _guidid=value;}
			get{return _guidid;}
		}

        [Display(Name = "模块", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(36, ErrorMessage = "长度不可超过36")]
        public string MenuId
		{
			set{ _menuid=value;}
			get{return _menuid;}
		}

        [Display(Name = "操作", Order = 4)]
        [StringLength(36, ErrorMessage = "长度不可超过36")]
        public string OperationId
		{
			set{ _operationid=value;}
			get{return _operationid;}
		}

        [Display(Name = "角色", Order = 3)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(36, ErrorMessage = "长度不可超过36")]
        public string RoleId
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		#endregion Model

	}
}

