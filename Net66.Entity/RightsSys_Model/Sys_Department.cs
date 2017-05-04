
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT.RightsSys.Entity
{
	/// <summary>
	/// Sys_Department:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    [Table("Sys_Department")]
    public class Sys_Department
	{
		public Sys_Department()
		{}
		#region Model
		private string _id;
		private string _code;
		private string _name;
		private string _parentid;
		private string _address;
		private int? _sort;
        private int? _isshow;
		private string _remark;

        [Key]
        [Display(Name = "主键", Order = 1)]
        public string Id
		{
			set{ _id=value;}
			get{return _id;}
		}      

        [Display(Name = "部门编码", Order = 2)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Code
		{
			set{ _code=value;}
			get{return _code;}
		}

        [Display(Name = "名称", Order = 3)]
        [Required(ErrorMessage = "不能为空")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}

        [Display(Name = "父部门", Order = 4)]
        [StringLength(36, ErrorMessage = "长度不可超过36")]
        public string ParentId
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}

        [Display(Name = "联系地址", Order = 5)]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}

        [Display(Name = "排序", Order = 6)]
        [Range(0, 2147483646, ErrorMessage = "数值超出范围")]
        public int? Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}

        [Display(Name = "备注", Order = 7)]
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}

        public int? IsShow
        {
            set;get;
        }

        #endregion Model

    }
}

