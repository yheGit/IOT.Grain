using IOT.RightsSys.Entity;
using Net66.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.SysSecCore
{
    public class AccountCore : SecRepository<Sys_UserInfo>
    {

        /// <summary>
        /// 验证用户名和密码是否正确
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>登录成功后的用户信息</returns>
        public Sys_UserInfo ValidateUser(string userName, string password)
        {
            if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(password))
                return null;

            //获取用户信息,请确定web.config中的连接字符串正确
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                return (from p in dbEntity.UserInfos
                        where p.LoginID == userName
                        && p.Password == password
                        && p.State == "开启"
                        select p).FirstOrDefault();
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
                try
                {
                    using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
                    {
                        var person = dbEntity.UserInfos.FirstOrDefault(p => (p.LoginID == personName) && (p.Password == oldPassword));
                        person.Password = newPassword;
                        dbEntity.SaveChanges();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    //ExceptionsHander.WriteExceptions(ex);
                }

            }
            return false;
        }


    }
}
