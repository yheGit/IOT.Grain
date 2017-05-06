using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Data.Base;
using Net66.Data.Context;
using Net66.Entity.IO_Model;
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


        /// <summary>
        /// 通过用户角色ID获取相应的菜单
        /// </summary>
        public List<Sys_Menu> GetMenuListByRole(string RoleID)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                //调试模式则输出SQL
                if (Utils.DebugApp)
                    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                var menuIdList = dbEntity.MenuRoleOperations.Where(m => m.RoleId == RoleID && m.OperationId == null).Select(s => s.MenuId);
                var queryData = dbEntity.Menus.Where(w => menuIdList.Contains(w.Id)).OrderBy(d => d.Sort);
                return queryData.ToList();
            }
        }




        #region 菜单管理
        public List<Sys_Menu> GetMenuList(List<string> _params, ref int total)
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

            Expression<Func<Sys_Menu, bool>> predicate = EfUtils.True<Sys_Menu>();
            predicate = p => p.IsShow == 0; ;
            ////var wCode = TypeParse.StrToInt(wareCode, 0);
            //where = where.And(w => w.IsActive == 1 && w.WH_Number == wareCode && w.Type == 0);
            //if (!string.IsNullOrEmpty(granaryCode))
            //    where = where.And(w => w.Number.Contains(granaryCode));//loufangleixing

            //排序字段转换
            Expression<Func<Sys_Menu, string>> orderByLambda = p => p.Sort.ToString();//null;            
            switch (sort)
            {
                case "Name": orderByLambda = p => p.Name; break;
                case "Sort": orderByLambda = p => p.Sort.ToString(); break;
                    //default: orderByLambda = p => p.Sort.ToString(); break;
            }

            #endregion

            IQueryable<Sys_Menu> queryData = null;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                if (isasc)
                    queryData = dbEntity.Menus.Where(predicate).OrderBy(orderByLambda);
                else
                    queryData = dbEntity.Menus.Where(predicate).OrderByDescending(orderByLambda);
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
        public Sys_Menu GetMenuInfo(string guid)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                return dbEntity.Menus.Where(w => w.Id == guid).FirstOrDefault();
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        public bool AddMenu(Sys_Menu _entity)
        {
            int recordsAffected;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                dbEntity.Menus.Add(_entity);
                recordsAffected = dbEntity.SaveChanges();
                return recordsAffected > 0;
            }
        }

        /// <summary>
        /// 是否存在改角色
        /// </summary>
        public bool IsExistMenu(Sys_Menu _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info = dbEntity.Menus.FirstOrDefault(f => f.Name == _entity.Name.Trim());
                if (info != null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        public bool UpdateMenu(Sys_Menu _entity)
        {
            var rebit = false;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        #region  //更新实体
                        var info = dbEntity.Menus.FirstOrDefault(f => f.Id == _entity.Id);
                        if (info != null)
                        {
                            info.Name = _entity.Name;
                            info.Iconic = _entity.Iconic;
                            info.State = _entity.State;
                            info.IsLeaf = _entity.IsLeaf;
                            info.Sort = _entity.Sort;
                            info.IsShow = _entity.IsShow;
                            info.LinkUrl = _entity.LinkUrl;
                            info.ParentId = _entity.ParentId;
                            info.Remark = _entity.Remark;
                        }
                        dbEntity.Set<Sys_Menu>().Attach(info);
                        dbEntity.Entry(info).State = EntityState.Modified;
                        #endregion////更新实体
                                          
                        int count = 0;
                        List<string> addSysOperationId = new List<string>();
                        List<string> deleteSysOperationId = new List<string>();
                        if (_entity.SysOperationId != null)
                        {
                            addSysOperationId = _entity.SysOperationId.Split(',').ToList();
                        }
                        if (_entity.SysOperationIdOld != null)
                        {
                            deleteSysOperationId = _entity.SysOperationIdOld.Split(',').ToList();
                        }
                        DataOfDiffrent.GetDiffrent(addSysOperationId, deleteSysOperationId, ref addSysOperationId, ref deleteSysOperationId);

                        if (addSysOperationId != null && addSysOperationId.Count() > 0)
                        {
                            foreach (var item in addSysOperationId)
                            {
                                Sys_MenuOperation sys = new Sys_MenuOperation
                                {
                                    GuidID = Utils.GetNewId(),
                                    Menu_Id = item,
                                    Operation_Id = _entity.Id
                                };
                                dbEntity.MenuOperations.Add(sys);
                                count++;
                            }
                        }
                        if (deleteSysOperationId != null && deleteSysOperationId.Count() > 0)
                        {
                            //数据库设置级联关系，自动删除子表的内容   
                            IQueryable<Sys_MenuOperation> collection = from f in dbEntity.MenuOperations
                                                                       where deleteSysOperationId.Contains(f.Operation_Id)
                                                                       select f;
                            foreach (var deleteItem in collection)
                            {
                                dbEntity.Entry<Sys_MenuOperation>(deleteItem).State = EntityState.Deleted;
                            }
                            count += dbEntity.SaveChanges();
                        }

                        if (count > 0)
                        {
                            transactionScope.Complete();
                            rebit = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Transaction.Current.Rollback();
                    }
                }

                return rebit;
            }
        }

        /// <summary>
        /// 删除角色信息
        /// 参数实为 主键集合
        /// </summary>
        public bool DeleteMenu(List<Sys_Menu> _list)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                foreach (var model in _list)
                {
                    var info = dbEntity.Menus.FirstOrDefault(f => f.Id == model.Id);
                    if (info == null)
                        continue;
                    info.IsShow = -4;
                    dbEntity.Set<Sys_Menu>().Attach(info);
                    dbEntity.Entry(info).State = EntityState.Modified;
                }
                return dbEntity.SaveChanges() > 0;
            }
        }



        #endregion



    }
}
