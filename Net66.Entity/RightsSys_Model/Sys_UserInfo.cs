
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT.RightsSys.Entity
{
	/// <summary>
	/// Sys_UserInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    [Table("Sys_UserInfo")]
    public class Sys_UserInfo
	{
		public Sys_UserInfo()
		{}

		#region Model
		private string _id;
		private string _loginid;
		private string _nickname;
		private string _password;
		private string _sex;
		private string _departmentid;
		private string _roleid;
		private string _telphone;
		private string _phonenumber;
		private string _emailaddress;
		private string _state;
		private string _address;
		private string _remark;

        [Key]
        [Display(Name = "主键", Order = 1)]
        public string Id
		{
			set{ _id=value;}
			get{return _id;}
		}

        //[ScaffoldColumn(true)]
        [Display(Name = "用户名", Order = 2)]
        //[Required(ErrorMessage = "不能为空")]
        //[StringLength(200, ErrorMessage = "长度不可超过200")]
        public string LoginID
		{
			set{ _loginid=value;}
			get{return _loginid;}
		}

        [Display(Name = "姓名", Order = 3)]
        //[StringLength(200, ErrorMessage = "长度不可超过200")]
        public string NickName
		{
			set{ _nickname=value;}
			get{return _nickname;}
		}

        [Display(Name = "密码", Order = 4)]
        //[Required(ErrorMessage = "不能为空")]
        //[StringLength(200, MinimumLength = 6, ErrorMessage = "长度不可小于6")]
        public string Password
		{
			set{ _password=value;}
			get{return _password;}
		}

        [Display(Name = "性别", Order = 5)]
        //[StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}

        [Display(Name = "部门", Order = 6)]
        //[StringLength(36, ErrorMessage = "长度不可超过36")]
        public string DepartmentId
		{
			set{ _departmentid=value;}
			get{return _departmentid;}
		}

        [Display(Name = "角色", Order = 7)]
        //[StringLength(36, ErrorMessage = "长度不可超过36")]
        public string RoleId
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}

        [Display(Name = "手机号码", Order = 8)]
        //[StringLength(200, ErrorMessage = "长度不可超过200")]
        public string TelPhone
		{
			set{ _telphone=value;}
			get{return _telphone;}
		}

        [Display(Name = "办公电话", Order = 9)]
        //[StringLength(200, ErrorMessage = "长度不可超过200")]
        //[DataType(DataType.PhoneNumber, ErrorMessage = "号码格式不正确")]
        public string PhoneNumber
		{
			set{ _phonenumber=value;}
			get{return _phonenumber;}
		}

        //[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "{0}的格式不正确")]
        [Display(Name = "邮箱", Order = 10)]
        //[StringLength(200, ErrorMessage = "长度不可超过200")]
        public string EmailAddress
		{
			set{ _emailaddress=value;}
			get{return _emailaddress;}
		}

        [Display(Name = "状态", Order = 11)]
        //[StringLength(200, ErrorMessage = "长度不可超过200")]
        public string State
		{
			set{ _state=value;}
			get{return _state;}
		}

        [Display(Name = "联系地址", Order = 12)]
        //[StringLength(200, ErrorMessage = "长度不可超过200")]
        public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}

        [Display(Name = "备注", Order = 13)]
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}

        private int? _isshow;
        public int? IsShow
        {
            set { _isshow = value; }
            get { return _isshow; }
        }

        private string _orgid;
        public string OrgId {
            set { _orgid = value; }
            get { return _orgid; }
        }
        private string _orgcode;
        public string OrgCode {
            set { _orgcode = value; }
            get { return _orgcode; }
        }

        private string _phoneinfo;
        public string PhoneInfo
        {
            set { _phoneinfo = value; }
            get { return _phoneinfo; }
        }

        
        #endregion Model


    }
}

