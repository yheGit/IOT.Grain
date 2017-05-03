
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT.RightsSys.Entity
{
	/// <summary>
	/// Sys_RoleUser:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    [Table("Sys_RoleUser")]
    public class Sys_RoleUser
	{
		public Sys_RoleUser()
		{}
		#region Model
		private string _guidid;
		private string _role_id;
		private string _person_id;

        [Key]
        [Display(Name = "主键", Order = 1)]
        public string GuidID
		{
			set{ _guidid=value;}
			get{return _guidid;}
		}

        [Display(Name = "角色", Order = 3)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(36, ErrorMessage = "长度不可超过36")]
        public string Role_Id
		{
			set{ _role_id=value;}
			get{return _role_id;}
		}

        [Display(Name = "用户", Order = 3)]
        public string Person_Id
		{
			set{ _person_id=value;}
			get{return _person_id;}
		}
		#endregion Model

	}
}

