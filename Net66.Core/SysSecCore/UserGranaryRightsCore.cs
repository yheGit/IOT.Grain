using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Data.Base;
using Net66.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Net66.Core.SysSecCore
{
    public class UserGranaryRightsCore : SecRepository<Sys_Department>
    {

        /// <summary>
        /// 获取组织信息
        /// 0获取前三级，1获取第三级
        /// </summary>
        public dynamic GetOrgSelectList(string departid)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {

                //var list = dbEntity.Departments.Where(w => System.Data.Entity.SqlServer.SqlFunctions.PatIndex("S______", w.Code) > 0
                // || System.Data.Entity.SqlServer.SqlFunctions.PatIndex("S____", w.Code) > 0
                // || System.Data.Entity.SqlServer.SqlFunctions.PatIndex("S__", w.Code) > 0).AsEnumerable();
                //if (type == 1)
                //    return list.Where(w => w.Code.Length > 6).Select(s => new { s.Id, s.Code, s.Name, s.ParentId }).ToList();
                //else
                //    return list.Select(s => new { s.Id, s.Code, s.Name, s.ParentId }).ToList();
            }
            return null;
        }

        public dynamic GetAllPersonByOrg(string departid)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info=dbEntity.Departments.FirstOrDefault(f => f.Id == departid);
                if (info == null)
                    return null;
                var ulist=dbEntity.UserInfos.Where(w2 => w2.DepartmentId == departid || info.Code.IndexOf(w2.OrgCode) > -1).ToList();

                var relist= ulist.Select(s => new {
                    Id=s.Id,
                    LoginID=s.LoginID,
                    NickName=s.NickName,
                    PhoneNumber=s.PhoneNumber,
                    OrgCode=s.OrgCode,
                    OrgId=s.OrgId
                }).ToList();
                return relist;
            }
        }

        /// <summary>
        /// type=1删除、0添加或修改
        /// </summary>
        public bool Set_UserGranaryRights(List<UserGranaryRights> list,int type=0)
        {

            list.ForEach(f => f.UpdateTime = Utils.GetServerDateTime());
            var useridlist = list.Select(s => s.UserId).ToList();
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    var dellist = dbEntity.UserGranaryRights.Where(w => useridlist.Contains(w.UserId)).ToList();
                    foreach (var dmode in dellist)
                        dbEntity.Entry<UserGranaryRights>(dmode).State = EntityState.Deleted;
                    if(type==0)
                        dbEntity.UserGranaryRights.AddRange(list);
                    var rebit= dbEntity.SaveChanges() > 0;
                    if (rebit)
                    {
                        transactionScope.Complete();
                        return rebit;
                    }
                }
            }

            return false;
        }

        public dynamic GetUserGranaryListByUid(string userid)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                return dbEntity.UserGranaryRights.Where(w => w.UserId == userid).ToList();
            }
        }



    }
}
