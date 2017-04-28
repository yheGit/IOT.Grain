
using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Core.SysSecCore;
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
    public class DepartmentCore:SecRepository<Sys_Department>
    {
        /// <summary>
        /// 分页获取消费记录 by yhw96160 2016-11-7 19:33:17
        /// </summary>
        /// <param name="skip">开始记录索引</param>
        /// <param name="take">返回记录数量</param>
        /// <param name="sql">sql</param>
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

        /// <summary>
        /// 获取当前用户未报销消费记录总数 by yhw96160 2017-3-3 17:37:56
        /// </summary>
        public int GetConsumRecordCount(string _userId)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                //StringBuilder sbSql = new StringBuilder();
                //sbSql.Append(string.Format("SELECT COUNT(0) 'Count' FROM [sangfor_sse].[dbo].[tConsumRecord] where AddBy = '{0}' ",_userId));
                //string sql = sbSql.ToString().ToLower();
                //var result = dbEntity.Database.SqlQuery<int>(sql).FirstOrDefault();
                //return result;
                return dbEntity.Departments.Where(w => w.Id == _userId ).Count();
            }
        }


        /// <summary>
        /// 获取消费记录的集合 by yhw96160 2016-11-9 09:58:03
        /// </summary>
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

            

        /// <summary>
        /// 添加消费记录 by yhw96160 2016-11-7 20:32:53
        /// </summary>
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

    
        /// <summary>
        /// 更新消费记录的报销状态 by yhw96160 2016-11-15 17:21:36
        /// </summary>
        /// <param name="_consumIdList">已报销的消费记录Id</param>
        /// <param name="_receiptsId">报销单号ID</param>
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

        #region //消费金额详细

        /// <summary>
        /// 添加消费金额详细 by yhw96160 2016-11-14 15:37:21
        /// </summary>
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
