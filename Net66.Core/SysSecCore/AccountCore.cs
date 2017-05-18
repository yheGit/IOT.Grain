using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Data.Context;
using Net66.Entity.IO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.SysSecCore
{
    public class AccountCore : SecRepository<Sys_UserInfo>
    {


        public bool AddMSg(SendMsg model)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                dbEntity.SendMsgs.Add(model);
                return dbEntity.SaveChanges() > 0;
            }
        }

        public SendMsg GetMsgVCode(string tel)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var nowtime = DateTime.Now.AddMinutes(-2);
                return dbEntity.SendMsgs.FirstOrDefault(f => f.SendTime >= nowtime);
            }
        }



        /// <summary>
        /// 验证用户名和密码是否正确
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录成功后的用户信息</returns>
        public Sys_UserInfo ValidateUser(string userName, string password, string phoneinfo = "")
        {
            if (String.IsNullOrWhiteSpace(userName))
                return null;
            if (String.IsNullOrWhiteSpace(phoneinfo) && String.IsNullOrWhiteSpace(password))
                return null;


            //获取用户信息,请确定web.config中的连接字符串正确
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                //return (from p in dbEntity.UserInfos
                //        where p.LoginID == userName
                //        && p.Password == password
                //        && p.State == "开启"
                //        select p).FirstOrDefault();
                if (!string.IsNullOrEmpty(phoneinfo))
                {
                    return dbEntity.UserInfos.FirstOrDefault(f => f.LoginID == userName && (f.Password == password || f.PhoneInfo == phoneinfo) && f.State == "开启");
                }
                else
                    return dbEntity.UserInfos.FirstOrDefault(f => f.LoginID == userName && f.Password == password && f.State == "开启");

            }
        }

        /// <summary>
        /// 注册用户（最好先验证）
        /// </summary>
        public bool UserSignUp(SigninModel model)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var entity = new Sys_UserInfo()
                {
                    Id = Utils.GetNewId(),
                    //RoleId = model.RoleId,
                    //DepartmentId = model.OrgId,
                    //OrgId = model.OrgId,
                    //OrgCode = model.OrgCode,
                    LoginID = model.LoginName,
                    TelPhone = model.LoginName,
                    IsShow = 0,
                    NickName = model.UserName,
                    PhoneNumber = model.LoginName,
                    Password = EncryptAndDecrypte.EncryptString(model.Password),
                    State = "开启",
                    PhoneInfo = EncryptAndDecrypte.EncryptString(model.PhoneInfo)
                };
                dbEntity.UserInfos.Add(entity);
                try
                {
                    return dbEntity.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="personName">用户名</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns>修改密码是否成功</returns>
        public bool ChangePassword(string personName, string oldPassword, string newPassword)
        {
            if (!string.IsNullOrWhiteSpace(personName) && !string.IsNullOrWhiteSpace(oldPassword) && !string.IsNullOrWhiteSpace(newPassword))
            {
                using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
                {
                    var person = dbEntity.UserInfos.FirstOrDefault(p => p.LoginID == personName && p.Password == oldPassword);
                    if (person == null)
                        return false;
                    person.Password = newPassword;
                    dbEntity.SaveChanges();
                    return true;
                }
            }
            return false;
        }


    }
}
