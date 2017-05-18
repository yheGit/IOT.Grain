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
    public class OperationCore : SecRepository<Sys_Operation>
    {

        /// <summary>
        /// 设置一个菜单
        /// </summary>
        public bool SetSysMenu(Sys_Operation entity)
        {

            bool bResult = false;
            int count = 0;

            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        Sys_Operation editEntity = dbEntity.Operations.SingleOrDefault(s => s.Id == entity.Id);
                        List<string> addSysMenuId = new List<string>();
                        List<string> deleteSysMenuId = new List<string>();
                        if (entity.SysMenuId != null)
                        {
                            addSysMenuId = entity.SysMenuId.Split(',').ToList();
                        }
                        if (entity.SysMenuIdOld != null)
                        {
                            deleteSysMenuId = entity.SysMenuIdOld.Split(',').ToList();
                        }
                        DataOfDiffrent.GetDiffrent(addSysMenuId, deleteSysMenuId, ref addSysMenuId, ref deleteSysMenuId);

                        if (addSysMenuId != null && addSysMenuId.Count() > 0)
                        {
                            foreach (var item in addSysMenuId)
                            {
                                Sys_MenuOperation sys = new Sys_MenuOperation
                                {
                                    GuidID = Utils.GetNewId(),
                                    Menu_Id = item,
                                    Operation_Id = entity.Id
                                };
                                dbEntity.MenuOperations.Add(sys);
                                count++;
                            }
                        }
                        if (deleteSysMenuId != null && deleteSysMenuId.Count() > 0)
                        {
                            //数据库设置级联关系，自动删除子表的内容   
                            IQueryable<Sys_MenuOperation> collection = from f in dbEntity.MenuOperations
                                                                       where deleteSysMenuId.Contains(f.Menu_Id)
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
                            bResult = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Transaction.Current.Rollback();
                    }
                }
            }

            return bResult;
        }

        /// <summary>
        /// 获取角色RoleID下菜单MenuID的操作
        /// </summary>
        public List<Sys_Operation> GetOperationByMenuRole(string MenuID, string RoleID)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                var operationIdList = dbEntity.MenuRoleOperations.Where(m => m.MenuId == MenuID && m.RoleId == RoleID).Select(s => s.OperationId);
                var queryData = dbEntity.Operations.Where(w => operationIdList.Contains(w.Id)).OrderBy(d => d.Sort);
                return queryData.ToList();
            }
        }


        #region 操作管理
        public List<Sys_Operation> GetOperationList(List<string> _params, ref int total)
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

            Expression<Func<Sys_Operation, bool>> predicate = EfUtils.True<Sys_Operation>();
            predicate = p => p.IsShow == 0; ;
            ////var wCode = TypeParse.StrToInt(wareCode, 0);
            //where = where.And(w => w.IsActive == 1 && w.WH_Number == wareCode && w.Type == 0);
            //if (!string.IsNullOrEmpty(granaryCode))
            //    where = where.And(w => w.Number.Contains(granaryCode));//loufangleixing

            //排序字段转换
            Expression<Func<Sys_Operation, string>> orderByLambda = p => p.Sort.ToString();//null;            
            switch (sort)
            {
                case "Name": orderByLambda = p => p.Name; break;
                case "Sort": orderByLambda = p => p.Sort.ToString(); break;
                    //default: orderByLambda = p => p.Sort.ToString(); break;
            }

            #endregion

            IQueryable<Sys_Operation> queryData = null;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                if (isasc)
                    queryData = dbEntity.Operations.Where(predicate).OrderBy(orderByLambda);
                else
                    queryData = dbEntity.Operations.Where(predicate).OrderByDescending(orderByLambda);
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
        public Sys_Operation GetOperationInfo(string guid)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                return dbEntity.Operations.Where(w => w.Id == guid).FirstOrDefault();
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        public bool AddOperation(Sys_Operation _entity)
        {
            int recordsAffected;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                dbEntity.Operations.Add(_entity);
                recordsAffected = dbEntity.SaveChanges();
                return recordsAffected > 0;
            }
        }

        /// <summary>
        /// 是否存在改角色
        /// </summary>
        public bool IsExistOperation(Sys_Operation _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info = dbEntity.Operations.FirstOrDefault(f => f.Function == _entity.Function.Trim());
                if (info != null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        public bool UpdateOperation(Sys_Operation _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info = dbEntity.Operations.FirstOrDefault(f => f.Id == _entity.Id);
                if (info != null)
                {
                    info.Name = _entity.Name;
                    info.Function = _entity.Function;
                    info.State = _entity.State;
                    info.Iconic = _entity.Iconic;
                    info.Remark = _entity.Remark;
                    info.Sort = _entity.Sort;

                }
                dbEntity.Set<Sys_Operation>().Attach(info);
                dbEntity.Entry(info).State = EntityState.Modified;
                return dbEntity.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除角色信息
        /// 参数实为 主键集合
        /// </summary>
        public bool DeleteOperation(List<Sys_Operation> _list)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                foreach (var model in _list)
                {
                    var info = dbEntity.Operations.FirstOrDefault(f => f.Id == model.Id);
                    if (info == null)
                        continue;
                    info.IsShow = -4;
                    dbEntity.Set<Sys_Operation>().Attach(info);
                    dbEntity.Entry(info).State = EntityState.Modified;
                }
                return dbEntity.SaveChanges() > 0;
            }
        }



        #endregion

    }
}
