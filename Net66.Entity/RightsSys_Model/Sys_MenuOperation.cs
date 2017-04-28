
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT.RightsSys.Entity
{
	/// <summary>
	/// Sys_MenuOperation:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    [Table("Sys_MenuOperation")]
    public class Sys_MenuOperation
	{
		public Sys_MenuOperation()
		{}
		#region Model
		private string _guidid;
		private string _operation_id;
		private string _menu_id;

        [Display(Name = "主键", Order = 1)]
        public string GuidID
		{
			set{ _guidid=value;}
			get{return _guidid;}
		}

        [Display(Name = "操作", Order = 4)]
        [StringLength(36, ErrorMessage = "长度不可超过36")]
        public string Operation_Id
		{
			set{ _operation_id=value;}
			get{return _operation_id;}
		}

        [Display(Name = "模块", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(36, ErrorMessage = "长度不可超过36")]
        public string Menu_Id
		{
			set{ _menu_id=value;}
			get{return _menu_id;}
		}
		#endregion Model

	}
}

