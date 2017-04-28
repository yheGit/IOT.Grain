using IOT.RightsSys.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;


/******************************************
*Creater:yhw[96160]
*CreatTime:2017-2-6 10:03:58
*Description:人员主数据库
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:
*******************************************/
namespace Net66.Data.Context
{
    public class DbSysSEC : DbContext
    {
        public DbSysSEC()
            : base("DB_SEC")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSysSEC(string name)
            : base(name)
        {
        }

        /// <summary>
        /// 部门表
        /// </summary>
        public DbSet<Sys_Department> Departments { get; set; }

        /// <summary>
        /// 菜单表
        /// </summary>
        public DbSet<Sys_Menu> Menus { get; set; }

        /// <summary>
        /// 菜单-操作表
        /// </summary>
        public DbSet<Sys_MenuOperation> MenuOperations { get; set; }

        /// <summary>
        /// 菜单-角色-操作表
        /// </summary>
        public DbSet<Sys_MenuRoleOperation> MenuRoleOperations { get; set; }

        /// <summary>
        /// 操作表
        /// </summary>
        public DbSet<Sys_Operation> Operations { get; set; }

        /// <summary>
        /// 角色表
        /// </summary>
        public DbSet<Sys_Role> Roles { get; set; }

        /// <summary>
        /// 用户角色表
        /// </summary>
        public DbSet<Sys_RoleUser> RoleUsers { get; set; }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<Sys_UserInfo> UserInfos { get; set; }




    }
}
