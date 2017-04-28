
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT.RightsSys.Entity
{
	/// <summary>
	/// Sys_Role:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    [Table("Sys_Role")]
    public class Sys_Role
	{
		public Sys_Role()
		{}
		#region Model
		private string _id;
		private string _name;
		private int? _state;
		private int? _sort;
		private string _description;
		private DateTime? _updatetime;

        [Display(Name = "主键", Order = 1)]
        public string Id
		{
			set{ _id=value;}
			get{return _id;}
		}

        [Display(Name = "名称", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}

        [Display(Name = "状态", Order = 3)]
        [Range(0, 2147483646, ErrorMessage = "数值超出范围")]
        public int? State
		{
			set{ _state=value;}
			get{return _state;}
		}

        [Display(Name = "排序", Order = 4)]
        [Range(0, 2147483646, ErrorMessage = "数值超出范围")]
        public int? Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}

        [Display(Name = "描述", Order =5)]
        [StringLength(4000, ErrorMessage = "长度不可超过4000")]
        public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}

        [Display(Name = "编辑时间", Order = 6)]
        [DataType(DataType.DateTime, ErrorMessage = "时间格式不正确")]
        public DateTime? UpdateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		#endregion Model

	}
}

