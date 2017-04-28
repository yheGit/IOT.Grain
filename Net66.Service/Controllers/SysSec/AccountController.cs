using IOT.RightsSys.Entity;
using Net66.Entity.IO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers.SysSec
{
    /// <summary>
    /// 用户账户信息
    /// </summary>
    public class AccountController : ApiController
    {
        // GET: Account
        public bool Index()
        {
            return false;
        }

        /// <summary>
        /// 点击 登录系统 后返回
        /// </summary>
        /// <param name="model">登录信息</param>
        /// <returns></returns>
        [HttpPost]
        public bool LogIn(LogOnModel model)
        {
            #region 验证码验证

            //if (Session["__VCode"] == null || (Session["__VCode"] != null && model.ValidateCode != Session["__VCode"].ToString()))
            //{
            //    ModelState.AddModelError("PersonName", "验证码错误！"); //return "";
            //    return false;
            //}
            #endregion

            //Sys_UserInfo person = accountBLL.ValidateUser(model.PersonName, EncryptAndDecrypte.EncryptString(model.Password));


            return false;
        }


        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public bool LogOff()
        {
            //if (Session["account"] != null)
            //{
            //    Session["account"] = null;
            //}
            return true;
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public bool ChangePassword(ChangePasswordModel model)
        {
            return false;
            
        }
       
       


    }
}