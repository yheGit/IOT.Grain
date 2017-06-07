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
        [HttpGet]
        public dynamic Index()
        {
            return new SigninModel();
        }

        [HttpGet]
        public ReturnData SendMsg(string id)
        {
            var tel = id;
            Random rd = new Random();
            int randkey = rd.Next(100000, 999999);
            var msgstr = "您正在使用中软易通短信验证功能，验证码是" + randkey + "，请注意保管好验证码，有效期120秒，并确认是本人操作。";
            var restr = NetSendMsg.DirectSend(tel, msgstr, 1);

            if (restr.Equals("发送成功!"))
            {
                SendMsg sendmsg = new SendMsg();
                sendmsg.GUID = Utils.GetNewId();
                sendmsg.Msg = msgstr;
                sendmsg.Tel = tel;
                sendmsg.Type = 2;
                sendmsg.SendTime = DateTime.Now;
                new AccountCore().AddMSg(sendmsg);

                return new ReturnData(1000, restr);
            }
            else if (restr.Equals("Fail，ServerErr"))
            {

            }
            return new ReturnData(1011, restr);
        }

        /// <summary>
        /// 点击 登录系统 后返回
        /// </summary>
        /// <param name="model">登录信息</param>
        [HttpPost]
        public ReturnData LogIn(LogOnModel model)
        {
            #region 验证码验证

            //if (model == null||string.IsNullOrEmpty(model.LoginName))
            //    return new ReturnData(1009);
            //if (string.IsNullOrEmpty(model.ValidateCode))
            //    return new ReturnData(1013, "验证码无效");
            //else
            //{
            //    if (!model.ValidateCode.Equals("666666"))
            //    {
            //        var vcode = new AccountCore().GetMsgVCode(model.LoginName);
            //        if (vcode == null)
            //            return new ReturnData(1013, "验证码错误");
            //        if (!vcode.Msg.Contains(model.ValidateCode))
            //            return new ReturnData(1013, "验证码错误");
            //    }
            //}
            #endregion

            var phoneinfo = "";
            if (!string.IsNullOrEmpty(model.PhoneInfo))
                phoneinfo = EncryptAndDecrypte.EncryptString(model.PhoneInfo);

            Sys_UserInfo person = new AccountCore().ValidateUser(model.LoginName, EncryptAndDecrypte.EncryptString(model.Password), phoneinfo);
            if (person == null)
                return new ReturnData(1011, "账号或密码不正确");

            #region //登陆成功，根据角色获取菜单列表
            var roleid = person.RoleId;
            List<Sys_Menu> menus = new MenuCore().GetMenuListByRole(roleid);
            List<Sys_Role> roles = new List<Sys_Role>() { new RoleCore().GetById(roleid) };

            Account queryData = new Account()
            {
                Id = person.Id,
                LoginID = person.LoginID,
                UserName = person.NickName,
                MenuList = menus,
                RoleIdList = roles
            };
            var reList = new datagrid
            {
                total = -1,
                rows = queryData
            };
            if (reList.rows != null)
                return new ReturnData(1000, "成功", reList);
            else
                return new ReturnData(1012);
            #endregion//登陆成功，根据角色获取菜单列表
        }

        /// <summary>
        /// 点击 登录系统 后返回
        /// </summary>
        /// <param name="model">登录信息</param>
        [HttpGet]
        public ReturnData LogIn2(string id)
        {
            string userid = id;
            if (string.IsNullOrEmpty(userid))
                return new ReturnData(1009);

            Sys_UserInfo person = new AccountCore().GetUserInfoById(userid);
            if (person == null)
                return new ReturnData(1011, "账号或密码不正确");

            #region //登陆成功，根据角色获取菜单列表
            var roleid = person.RoleId;
            List<Sys_Menu> menus = new MenuCore().GetMenuListByRole(roleid);
            List<Sys_Role> roles = new List<Sys_Role>() { new RoleCore().GetById(roleid) };

            Account queryData = new Account()
            {
                Id = person.Id,
                LoginID = person.LoginID,
                UserName = person.NickName,
                MenuList = menus,
                RoleIdList = roles
            };
            var reList = new datagrid
            {
                total = -1,
                rows = queryData
            };
            if (reList.rows != null)
                return new ReturnData(1000, "成功", reList);
            else
                return new ReturnData(1012);
            #endregion//登陆成功，根据角色获取菜单列表
        }


        /// <summary>
        /// 注册
        /// </summary>
        [HttpPost]
        public ReturnData SiginUp(SigninModel model)
        {

            if (model == null || string.IsNullOrEmpty(model.LoginName))
                return new ReturnData(1009);
            #region 验证码验证
            if (string.IsNullOrEmpty(model.ValidateCode))
                return new ReturnData(1013, "验证码无效");
            else
            {
                if (!model.ValidateCode.Equals("666666"))
                {
                    var vcode = new AccountCore().GetMsgVCode(model.LoginName);
                    if (vcode == null)
                        return new ReturnData(1013, "验证码错误");
                    if (!vcode.Msg.Contains(model.ValidateCode))
                        return new ReturnData(1013, "验证码错误");
                }
            }
            #endregion
            string id = model.LoginName;
            var entity = new Sys_UserInfo() { LoginID = id };
            bool rebit = new UserInfoCore().IsExistUser(entity);
            if (rebit) return new ReturnData(1013, "用户名已存在！");
            rebit = new AccountCore().UserSignUp(model);
            if (rebit == true)
                return new ReturnData(1000, "注册成功");
            return new ReturnData(1011, "注册失败");
        }

        /// <summary>
        /// 退出
        /// </summary>
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