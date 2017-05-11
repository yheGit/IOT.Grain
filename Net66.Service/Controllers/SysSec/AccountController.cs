using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Core.SysSecCore;
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
        [HttpPost]
        public ReturnData LogIn(LogOnModel model)
        {
            #region 验证码验证

            //if (Session["__VCode"] == null || (Session["__VCode"] != null && model.ValidateCode != Session["__VCode"].ToString()))
            //{
            //    ModelState.AddModelError("PersonName", "验证码错误！"); //return "";
            //    return false;
            //}
            if (model == null)
                return new ReturnData(1009); 

            #endregion

            Sys_UserInfo person = new AccountCore().ValidateUser(model.LoginName, EncryptAndDecrypte.EncryptString(model.Password));
            if (person == null)
                return new ReturnData(1011,"账号或密码不正确");

            #region //登陆成功，根据角色获取菜单列表
            var id = person.RoleId;
            List<Sys_Menu> queryData = new MenuCore().GetMenuListByRole(id);
            int total = queryData.Count();
            var reList = new datagrid
            {
                total = total,
                rows = queryData
            };
            if (total > 0 && reList.rows != null)
                return new ReturnData(1000, "成功", reList);
            else
                return new ReturnData(1012);
            #endregion//登陆成功，根据角色获取菜单列表
        }

        [HttpGet]
        public LogOnModel GetDate()
        {

            return new LogOnModel();
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
            var rebit = new AccountCore().ChangePassword(model.LoginName, 
                EncryptAndDecrypte.EncryptString(model.OldPassword), EncryptAndDecrypte.EncryptString(model.NewPassword));
            return rebit;

        }




    }
}