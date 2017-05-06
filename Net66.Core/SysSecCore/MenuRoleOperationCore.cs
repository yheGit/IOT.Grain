using IOT.RightsSys.Entity;
using Net66.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.SysSecCore
{
   public class MenuRoleOperationCore : SecRepository<Sys_MenuRoleOperation>
    {
        public List<Sys_MenuRoleOperation> GetListByRole(string RoleID)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                var queryData = dbEntity.MenuRoleOperations.Where(m =>m.RoleId == RoleID);
                return queryData.ToList();
            }

        }

        public Sys_MenuRoleOperation GetById(string GUID)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                var queryData = dbEntity.MenuRoleOperations.SingleOrDefault(m => m.GuidID == GUID);
                return queryData;
            }
        }




    }
}
