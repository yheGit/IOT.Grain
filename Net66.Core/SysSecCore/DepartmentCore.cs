
using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Core.SysSecCore;
using Net66.Data.Base;
using Net66.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


/******************************************
*Creater:yhw[96160]
*CreatTime:2016-11-7 19:25:34
*Description:
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:
*******************************************/
namespace Net66.Core.SysSecCore
{
    public class DepartmentCore : SecRepository<Sys_Department>
    {

        #region 获取组织结构关系（公司-部门）
        public List<Sys_Department> GetOrgList(List<string> _params, ref int total)
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

            Expression<Func<Sys_Department, bool>> predicate = EfUtils.True<Sys_Department>();
            ////var wCode = TypeParse.StrToInt(wareCode, 0);
            //where = where.And(w => w.IsActive == 1 && w.WH_Number == wareCode && w.Type == 0);
            //if (!string.IsNullOrEmpty(granaryCode))
            //    where = where.And(w => w.Number.Contains(granaryCode));//loufangleixing

            //排序字段转换
            Expression<Func<Sys_Department, string>> orderByLambda = p => p.Sort.ToString();//null;            
            switch (sort)
            {
                case "Name": orderByLambda = p => p.Name; break;
                case "Sort": orderByLambda = p => p.Sort.ToString(); break;
                //default: orderByLambda = p => p.Sort.ToString(); break;
            }

            #endregion

            IQueryable<Sys_Department> queryData = null;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                if (isasc)
                    queryData = dbEntity.Departments.Where(predicate).OrderBy(orderByLambda);
                else
                    queryData = dbEntity.Departments.Where(predicate).OrderByDescending(orderByLambda);
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

        #endregion


        public List<Sys_Department> GetConsumRecordBySql(int _skip, int _take, string _sql)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                if (_skip > 0)
                {
                    _take = _skip + _take;
                    _skip = _skip + 1;
                }
                _sql = _sql.ToLower().Replace(" from", ",row_number() over(order by a.[AddDate] desc) as rank from");
                _sql = string.Format("select  * from ({0}) as t where t.rank between {1} and {2}", _sql, _skip, _take);
                try
                {
                    return dbEntity.Departments.SqlQuery(_sql).ToList();

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }


        public int GetConsumRecordCount(string _userId)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                //StringBuilder sbSql = new StringBuilder();
                //sbSql.Append(string.Format("SELECT COUNT(0) 'Count' FROM [sangfor_sse].[dbo].[tConsumRecord] where AddBy = '{0}' ",_userId));
                //string sql = sbSql.ToString().ToLower();
                //var result = dbEntity.Database.SqlQuery<int>(sql).FirstOrDefault();
                //return result;
                return dbEntity.Departments.Where(w => w.Id == _userId).Count();
            }
        }



        public List<Sys_Department> GetPageListConsumRecord(Expression<Func<Sys_Department, bool>> where, Expression<Func<Sys_Department, string>> orderBy
            , bool ascending, int pageIndex, int pageSize, ref int rows)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                //调试模式则输出SQL
                if (Utils.DebugApp)
                    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));

                var list2 = new List<Sys_Department>();
                rows = dbEntity.Set<Sys_Department>().Count(where);
                if (rows <= 0)
                    return list2;

                var list = dbEntity.Set<Sys_Department>().Where(where);

                if (ascending == true)
                    list2 = list.OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                else
                    list2 = list.OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list2;
            }
        }

        /// <summary>
        /// 通过报销单获取消费记录
        /// </summary>
        public List<Sys_Department> GetListConsumRecordByReceiptId(string _receiptId)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var list = dbEntity.Departments.Where(s => s.Id == _receiptId).ToList();
                return list;
            }
        }


        public bool AddConsumRecord(Sys_Department _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                dbEntity.Departments.Add(_entity);
                int recordsAffected;
                recordsAffected = dbEntity.SaveChanges();
                return recordsAffected > 0;
            }
        }


        public bool UpdateConsumRecordIsCost(List<string> _consumIdList, string _receiptsId)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var list = dbEntity.Departments.Where(w => _consumIdList.Contains(w.Id)).ToList();
                foreach (var mode in list)
                {
                    mode.Sort = 1;
                    mode.Name = _receiptsId;
                    dbEntity.Set<Sys_Department>().Attach(mode);
                    dbEntity.Entry(mode).State = EntityState.Modified;
                }
                return dbEntity.SaveChanges() > 0;
            }
        }

        #region 

        public int AddListCaDetail(List<Sys_Department> _cadList)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                try
                {
                    if (!_cadList.Any())
                        return 0;
                    foreach (var current in _cadList)
                    {
                        dbEntity.Set<Sys_Department>().Add(current);
                    }

                    return dbEntity.SaveChanges();
                }
                catch
                {
                    return -1;
                }
            }
        }

        #endregion


    }
}
