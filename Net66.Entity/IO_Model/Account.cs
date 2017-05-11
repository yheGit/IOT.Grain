using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Net66.Entity.IO_Model
{
    /// <summary>
    /// 登录的用户信息
    /// </summary>
    [DataContract]
    public class Account
    {
        /// <summary>
        /// 主键
        /// </summary>
        [DataMember]
        public string Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember]
        public string LoginID { get; set; }
        /// <summary>
        /// 登录的用户名
        /// </summary>
        [DataMember]
        public string UserName { get; set; }
        /// <summary>
        /// 角色的集合
        /// </summary>
        [DataMember]
        public List<string> RoleIds { get; set; }

        /// <summary>
        /// 菜单的集合
        /// </summary>
        [DataMember]
        public List<string> MenuIds { get; set; }


    }


    public class ChangePasswordModel
    {
        //[Required(ErrorMessage = "请填写用户名")]
        [DisplayName("用户名")]
        public string LoginName { get; set; }

        //[Required(ErrorMessage = "请填写当前密码")]
        //[DataType(DataType.Password)]
        [DisplayName("当前密码")]
        public string OldPassword { get; set; }

        //[Required(ErrorMessage = "请填写新密码")]
        //[StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        [DisplayName("新密码")]
        public string NewPassword { get; set; }

        //[DataType(DataType.Password)]
        [DisplayName("确认密码")]
        //[Compare("NewPassword", ErrorMessage = "两次密码不一致")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        //[Required(ErrorMessage = "请填写用户名")]
        [DisplayName("用户名")]
        public string LoginName { get; set; }//PersonName

        //[StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        //[Required(ErrorMessage = "请填写密码")]
        //[DataType(DataType.Password)]
        [DisplayName("密码")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "请填写验证码")]
        [DisplayName("验证码")]
        public string ValidateCode { get; set; }

        [DisplayName("记住我?")]
        public bool RememberMe { get; set; }
    }





}
