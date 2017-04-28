
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT.RightsSys.Entity
{
	/// <summary>
	/// Sys_Operation:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    [Table("Sys_Operation")]
    public class Sys_Operation
	{
		public Sys_Operation()
		{}
		#region Model
		private string _id;
		private string _name;
		private string _function;
		private string _iconic;
		private int? _sort;
		private string _remark;
		private string _state;

        [Display(Name = "主键", Order = 1)]
        public string Id
		{
			set{ _id=value;}
			get{return _id;}
		}

        [Display(Name = "名称", Order = 2)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}

        [Display(Name = "方法", Order = 3)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Function
		{
			set{ _function=value;}
			get{return _function;}
		}

        [Display(Name = "图标", Order = 4)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Iconic
		{
			set{ _iconic=value;}
			get{return _iconic;}
		}

        [Display(Name = "排序", Order = 5)]
        [Range(0, 2147483646, ErrorMessage = "数值超出范围")]
        public int? Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}

        [Display(Name = "备注", Order = 6)]
        [StringLength(4000, ErrorMessage = "长度不可超过4000")]
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}

        [Display(Name = "状态", Order = 7)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

