
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT.RightsSys.Entity
{
	/// <summary>
	/// Sys_Menu:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    [Table("Sys_Menu")]
    public class Sys_Menu
	{
		public Sys_Menu()
		{}
		#region Model
		private string _id;
		private string _name;
		private string _parentid;
		private string _isleaf;
		private string _linkurl;
		private int? _sort;
		private string _state;
		private string _iconic;
		private string _remark;

        //[Key]
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

        [Display(Name = "父模块", Order = 3)]
        [StringLength(36, ErrorMessage = "长度不可超过36")]
        public string ParentId
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}

        [Display(Name = "子节点", Order = 4)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string IsLeaf
		{
			set{ _isleaf=value;}
			get{return _isleaf;}
		}

        [Display(Name = "网址", Order = 5)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string LinkUrl
		{
			set{ _linkurl=value;}
			get{return _linkurl;}
		}

        [Display(Name = "排序", Order = 6)]
        [Range(0, 2147483646, ErrorMessage = "数值超出范围")]
        public int? Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}

        [Display(Name = "状态", Order = 7)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string State
		{
			set{ _state=value;}
			get{return _state;}
		}

        [Display(Name = "图标", Order = 8)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Iconic
		{
			set{ _iconic=value;}
			get{return _iconic;}
		}

        [Display(Name = "备注", Order = 9)]
        [StringLength(4000, ErrorMessage = "长度不可超过4000")]
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

