using IOT.RightsSys.Entity;
using Net66.Data.Context;
using Net66.Entity.IO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.SysSecCore
{
    public class MenuCore : SecRepository<Sys_Menu>
    {

        /// <summary>
        /// 根据PersonId获取已经启用的菜单
        /// </summary>
        /// <param name="personId">人员的Id</param>
        /// <returns>菜单拼接的字符串</returns>
        public List<Sys_Menu> GetMenuByAccount(Account person)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                string personId = person.Id;
                var roleIds =
                           (
                           from r in dbEntity.Roles
                           from p in dbEntity.UserInfos
                           where p.Id == personId
                           select r.Id
                           ).ToList();
                person.RoleIds = roleIds;

                List<Sys_Menu> menuNeed =
                            (
                            from m2 in dbEntity.Menus
                            from f in dbEntity.MenuRoleOperations
                            where roleIds.Any(a => a == f.RoleId) && f.OperationId == null
                            select m2
                            ).Distinct().OrderBy(o => o.Remark).ToList();

                return menuNeed;


            }
        }

    }
}
