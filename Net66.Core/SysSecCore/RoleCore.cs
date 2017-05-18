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
using System.Transactions;
using System.Threading.Tasks;

namespace Net66.Core.SysSecCore
{
    /// <summary>
    /// 角色分配权限
    /// </summary>
    public class RoleCore : SecRepository<Sys_Role>
    {

        public bool SaveCollection(string[] ids, string RoleID)
        {
            #region //获取传入的菜单
            char split = '^';
            var data = (
                            from f in ids
                            where f.Contains(split)
                            select f.Substring(0, f.IndexOf(split))
                        ).Union(
                            from f in ids
                            where !string.IsNullOrWhiteSpace(f) && !f.Contains(split)
                            select f
                         ).Select(p => p);
            #endregion //获取传入的菜单

            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    #region //利用编码机制，查询出所有的菜单
                    var codes = dbEntity.Menus.Where(w => data.Contains(w.Id)).Select(s => s.Code).Distinct();
                    List<string> ls = new List<string>();
                    foreach (var item in codes)
                    {
                        for (int i = 0; i < item.Length / 4; i++)
                        {
                            string num = System.Numerics.BigInteger.Divide(System.Numerics.BigInteger.Parse(item), System.Numerics.BigInteger.Pow(10000, i)).ToString();

                            if (!ls.Contains(num))
                                ls.Add(num);
                        }
                    }
                    var SysMenusIds = from f in dbEntity.Menus
                                      where ls.Contains(f.Code)
                                      select f.Id;
                    #endregion //现在所有的菜单

                    #region //删除该角色的所有的菜单和操作
                    var delList = dbEntity.MenuRoleOperations.Where(w => w.RoleId == RoleID).ToList();
                    foreach (var del in delList)
                    {
                        dbEntity.Entry<Sys_MenuRoleOperation>(del).State = EntityState.Deleted;
                    }
                    #endregion //删除该角色的所有的菜单和操作

                    #region //插入菜单
                    foreach (var item in SysMenusIds)
                    {
                        dbEntity.MenuRoleOperations.Add(new Sys_MenuRoleOperation() { GuidID = Utils.GetNewId(), RoleId = RoleID, MenuId = item });
                    }
                    #endregion //插入菜单

                    #region //插入操作
                    foreach (var item in ids)
                    {
                        if (item.Contains(split))
                            dbEntity.MenuRoleOperations.Add(new Sys_MenuRoleOperation()
                            { GuidID = Utils.GetNewId(), RoleId = RoleID, MenuId = item.Substring(0, item.IndexOf(split)), OperationId = item.Substring(item.IndexOf(split) + 1) });
                    }
                    #endregion //插入操作

                    dbEntity.SaveChanges();
                    transactionScope.Complete();
                    return true;
                }

            }
        }

        public Sys_Role GetById(string GUID)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                var queryData = dbEntity.Roles.SingleOrDefault(m => m.Id == GUID);
                return queryData;
            }

        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        public dynamic GetRoleList(string orgid)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                return dbEntity.Roles.Where(w => w.IsShow == 0).ToList();
            }
        }


        #region 角色管理
        public List<Sys_Role> GetRoleList(List<string> _params, ref int total)
        {

            #region //条件查询
            int pIndex = TypeParse.StrToInt(Utils.GetValue(_params, "PageIndex^"), 0);
            int pageIndex = pIndex <= 0 ? 1 : pIndex;//页码
            int pageSize = TypeParse.StrToInt(Utils.GetValue(_params, "PageCount^"), 0);//每页显示的行数
            string sort = Utils.GetValue(_params, "Sort^");//排序字段
            string order = Utils.GetValue(_params, "OrderType^").ToLower();//升序asc（默认）还是降序desc
            string search = Utils.GetValue(_params, "Search^");
            bool isasc = true;
            if (order == "desc")
                isasc = false;
            if (pageSize <= 0)
                return null;

            Expression<Func<Sys_Role, bool>> predicate = EfUtils.True<Sys_Role>();
            predicate = p => p.IsShow == 0; ;
            ////var wCode = TypeParse.StrToInt(wareCode, 0);
            //where = where.And(w => w.IsActive == 1 && w.WH_Number == wareCode && w.Type == 0);
            //if (!string.IsNullOrEmpty(granaryCode))
            //    where = where.And(w => w.Number.Contains(granaryCode));//loufangleixing

            //排序字段转换
            Expression<Func<Sys_Role, string>> orderByLambda = p => p.Sort.ToString();//null;            
            switch (sort)
            {
                case "Name": orderByLambda = p => p.Name; break;
                case "Sort": orderByLambda = p => p.Sort.ToString(); break;
                    //default: orderByLambda = p => p.Sort.ToString(); break;
            }

            #endregion

            IQueryable<Sys_Role> queryData = null;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                if (isasc)
                    queryData = dbEntity.Roles.Where(predicate).OrderBy(orderByLambda);
                else
                    queryData = dbEntity.Roles.Where(predicate).OrderByDescending(orderByLambda);
                total = queryData.Count();
                if (total > 0)
                {
                    if (pageIndex <= 1)
                        queryData = queryData.Take(pageSize);
                    else
                        queryData = queryData.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
                return queryData.ToList();
            }

        }

        /// <summary>
        /// 通过主键获取角色的详细信息
        /// </summary>
        public Sys_Role GetRoleInfo(string guid)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                return dbEntity.Roles.Where(w => w.Id == guid).FirstOrDefault();
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        public bool AddRole(Sys_Role _entity)
        {
            int recordsAffected;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                dbEntity.Roles.Add(_entity);
                recordsAffected = dbEntity.SaveChanges();
                return recordsAffected > 0;
            }
        }

        /// <summary>
        /// 是否存在改角色
        /// </summary>
        public bool IsExistRole(Sys_Role _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info = dbEntity.Roles.FirstOrDefault(f => f.Name == _entity.Name.Trim());
                if (info != null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        public bool UpdateRole(Sys_Role _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info = dbEntity.Roles.FirstOrDefault(f => f.Id == _entity.Id);
                if (info != null)
                {
                    info.Name = _entity.Name;
                    info.Description = _entity.Description;
                    info.State = _entity.State;
                    info.UpdateTime = Utils.GetServerDateTime();
                    info.Sort = _entity.Sort;

                }
                dbEntity.Set<Sys_Role>().Attach(info);
                dbEntity.Entry(info).State = EntityState.Modified;
                return dbEntity.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除角色信息
        /// 参数实为 主键集合
        /// </summary>
        public bool DeleteRole(List<Sys_Role> _list)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                foreach (var model in _list)
                {
                    var info = dbEntity.Roles.FirstOrDefault(f => f.Id == model.Id);
                    if (info == null)
                        continue;
                    info.IsShow = -4;
                    dbEntity.Set<Sys_Role>().Attach(info);
                    dbEntity.Entry(info).State = EntityState.Modified;
                }
                return dbEntity.SaveChanges() > 0;
            }
        }



        #endregion

    }
}
