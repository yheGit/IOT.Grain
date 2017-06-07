using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using Net66.Data.Context;
using Net66.Data.Base;
using Net66.Data.Interface;

namespace Net66.Data
{
    public class GrainRepository<T> : IDisposable, IGrainRepository<T> where T : class
    {
        private GrainContext db2;
        private bool isDisposed;
        private static SqlHelper sqlHelper;

        public GrainContext DB
        {
            get
            {
                if (db2 == null)
                    db2 = new GrainContext();
                return db2;
            }
        }

        /// <summary>
        /// 获取DbSet 
        /// </summary>
        public DbSet<T> DbSet
        {
            get
            {
                return DB.Set<T>();
            }
        }

        public int Add(T entity, Expression<Func<T, bool>> predicate)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    var count = db.Set<T>().Where(predicate).Count();
                    if (count > 0)
                        return 1;
                    db.Set<T>().Add(entity);
                    return db.SaveChanges();
                }
                catch
                {
                    return -22;
                }
            }
        }


        public int AddDelete(T entity, Expression<Func<T, bool>> predicate)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    var list = db.Set<T>().Where(predicate);
                    foreach (var m in list)
                        db.Entry<T>(m).State = EntityState.Deleted;
                    db.Set<T>().Add(entity);
                    return db.SaveChanges();
                }
                catch
                {
                    return -22;
                }
            }
        }


        public int Add(T entity)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    db.Set<T>().Add(entity);
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = string.Concat(new object[]
                    {
                        "Message：",
                        ex.Message,
                        "。 Source:",
                        ex.Source,
                        "。 TargetSite:",
                        ex.TargetSite,
                        "。 InnerException：",
                        ex.InnerException
                    });
                    //Error.WriteLog("Repository-Add", msg);
                    return -22;
                }
            }
        }

        public int Add(List<T> list, string[] AndKeys, string OrKeys)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    foreach (var current in list)
                    {
                        var t = db.Set<T>().Where(EfUtils.AndOr<T>(AndKeys, OrKeys, current)).FirstOrDefault<T>();
                        if (t == null)
                            db.Set<T>().Add(current);
                    }

                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return -22;
                }

            }
        }

        public int Add(List<T> list)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    foreach (var current in list)
                    {
                        db.Set<T>().Add(current);
                    }

                    return db.SaveChanges();
                }
                catch
                {
                    return -22;
                }
            }
        }

        public int AddUpdate(List<T> list, string[] selectKey, string[] updateKey)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    foreach (var current in list)
                    {
                        var t = db.Set<T>().Where(EfUtils.And<T>(selectKey, current)).FirstOrDefault<T>();
                        if (t == null)
                        {
                            db.Set<T>().Add(current);
                        }
                        else
                        {
                            foreach (var k in updateKey)
                            {
                                var value = current.GetType().GetProperty(k).GetValue(current, null);
                                t.GetType().GetProperty(k).SetValue(t, value, null);
                            }
                            db.Set<T>().Attach(t);
                            db.Entry<T>(t).State = EntityState.Modified;
                        }
                    }

                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return -22;
                }
            }
        }

        public int AddUpdate(List<T> list, string[] selectKey, string[] updateKey, string defDate)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    foreach (var current in list)
                    {
                        var t = db.Set<T>().Where(EfUtils.And<T>(selectKey, current)).FirstOrDefault<T>();
                        if (t == null)
                        {
                            current.GetType().GetProperty(defDate).SetValue(current, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), null);
                            db.Set<T>().Add(current);
                        }
                        else
                        {
                            foreach (var k in updateKey)
                            {
                                if (k.Equals(defDate))
                                {
                                    t.GetType().GetProperty(k).SetValue(t, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), null);
                                }
                                else
                                {
                                    var value = current.GetType().GetProperty(k).GetValue(current, null);
                                    t.GetType().GetProperty(k).SetValue(t, value, null);
                                }
                            }
                            db.Set<T>().Attach(t);
                            db.Entry<T>(t).State = EntityState.Modified;
                        }
                    }

                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return -22;
                }
            }
        }

        public int AddUpdateDelete(List<T> addupdateList, List<T> deleteList, string[] selectKey, string selectOrKey, string[] updateKey, string[] deleteKey, string defDate)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    if (addupdateList != null && addupdateList.Count > 0)
                    {
                        foreach (var current in addupdateList)
                        {
                            var t = db.Set<T>().Where(EfUtils.AndOr<T>(selectKey, selectOrKey, current)).FirstOrDefault<T>();
                            if (t == null)
                            {
                                if (!string.IsNullOrEmpty(defDate))
                                    current.GetType().GetProperty(defDate).SetValue(current, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), null);
                                db.Set<T>().Add(current);
                            }
                            else
                            {
                                foreach (var k in updateKey)
                                {
                                    if (!string.IsNullOrEmpty(defDate) && k.Equals(defDate))
                                        t.GetType().GetProperty(k).SetValue(t, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), null);
                                    else
                                    {
                                        var value = current.GetType().GetProperty(k).GetValue(current, null);
                                        t.GetType().GetProperty(k).SetValue(t, value, null);
                                    }
                                }
                                db.Set<T>().Attach(t);
                                db.Entry<T>(t).State = EntityState.Modified;
                            }
                        }
                    }

                    if (deleteList != null && deleteList.Count > 0)
                    {
                        foreach (var current2 in deleteList)
                        {
                            var t2 = db.Set<T>().Where(EfUtils.And<T>(deleteKey, current2)).FirstOrDefault<T>();
                            if (t2 != null)
                            {

                                db.Entry<T>(t2).State = EntityState.Deleted;
                            }
                        }
                    }
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = string.Concat(new object[]
                    {
                        "Message：",
                        ex.Message,
                        "。 Source:",
                        ex.Source,
                        "。 TargetSite:",
                        ex.TargetSite,
                        "。 InnerException：",
                        ex.InnerException
                    });
                    Error.WriteLog("Repository-AddUpdateDelete", msg);
                    return -22;
                }
            }
        }

        public int Update(List<T> list, string[] selectKey, string[] updateKey, string defDate)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    foreach (var current in list)
                    {
                        var t = db.Set<T>().Where(EfUtils.And<T>(selectKey, current)).FirstOrDefault<T>();
                        if (t != null)
                        {
                            foreach (var k in updateKey)
                            {
                                if (!string.IsNullOrEmpty(defDate) && k.Equals(defDate))
                                {
                                    t.GetType().GetProperty(k).SetValue(t, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), null);
                                }
                                else
                                {
                                    var value = current.GetType().GetProperty(k).GetValue(current, null);
                                    t.GetType().GetProperty(k).SetValue(t, value, null);
                                }
                            }
                            db.Set<T>().Attach(t);
                            db.Entry<T>(t).State = EntityState.Modified;
                        }
                    }
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = string.Concat(new object[]
                    {
                        "Message：",
                        ex.Message,
                        "。 Source:",
                        ex.Source,
                        "。 TargetSite:",
                        ex.TargetSite,
                        "。 InnerException：",
                        ex.InnerException
                    });
                    Error.WriteLog("Repository-Update-defDate", msg);
                    return -22;
                }
            }
        }

        public int Update(T entity)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    db.Set<T>().Attach(entity);
                    db.Entry<T>(entity).State = EntityState.Modified;
                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    string msg = string.Concat(new object[]
                    {
                        "Message：",
                        ex.Message,
                        "。 Source:",
                        ex.Source,
                        "。 TargetSite:",
                        ex.TargetSite,
                        "。 InnerException：",
                        ex.InnerException
                    });
                    Error.WriteLog("Repository-Update", msg);
                    return -22;
                }
            }
        }

        public int Delete(T entity)
        {
            using (GrainContext db = new GrainContext())
            {
                db.Entry<T>(entity).State = EntityState.Deleted;
                return db.SaveChanges();
            }
        }

        public int Delete(List<T> deleteList, string[] deleteKey)
        {
            using (GrainContext db = new GrainContext())
            {

                if (deleteList != null && deleteList.Count > 0)
                {
                    foreach (var current2 in deleteList)
                    {
                        var t2 = db.Set<T>().Where(EfUtils.And<T>(deleteKey, current2)).FirstOrDefault<T>();
                        if (t2 != null)
                        {
                            db.Entry<T>(t2).State = EntityState.Deleted;
                        }
                    }
                }
                return db.SaveChanges();
            }
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            using (GrainContext db = new GrainContext())
            {
                if (predicate == null)
                    return null;
                return db.Set<T>().Where(predicate).FirstOrDefault<T>();
            }
        }

        public List<T> GetList(Expression<Func<T, bool>> predicate)
        {
            using (GrainContext db = new GrainContext())
            {
                //调试模式则输出SQL
                //if (Utils.DebugApp)
                db.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                if (predicate == null)
                    return db.Set<T>().ToList<T>();
                return db.Set<T>().Where(predicate).ToList<T>();
            }
        }

        public System.Data.DataTable QueryByTablde(string sql)
        {
            sqlHelper = sqlHelper ?? new SqlHelper(EfUtils.GetContext_ConneString);
            return sqlHelper.QueryByTablde(sql);
        }

        public int ExecuteSql(string sql)
        {
            sqlHelper = sqlHelper ?? new SqlHelper(EfUtils.GetContext_ConneString);
            return sqlHelper.ExecuteSql(sql);
        }

        public List<T> GetPageLists(Expression<Func<T, bool>> where, Expression<Func<T, string>> orderBy, bool ascending, int pageIndex, int pageSize, ref int rows)
        {
            using (var db = new GrainContext())
            {
                //调试模式则输出SQL
                db.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                if (where != null)
                {
                    var list = db.Set<T>().Where(where);
                    rows = db.Set<T>().Count();
                    if (rows <= 0)
                        return new List<T>();
                    var list2 = list.OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    return list2;
                }
                else
                {
                    var list = db.Set<T>().ToList();
                    rows = list.Count;
                    if (rows <= 0)
                        return new List<T>();
                    var list2 = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    return list2;
                }
            }
        }

        public IEnumerable<T> GetPageLists2(Expression<Func<T, bool>> where, Expression<Func<T, string>> orderBy, bool ascending, int pageIndex, int pageSize, ref int rows)
        {
            if (where != null)
            {
                var list = DbSet.Where(where);
                rows = list.Count();
                if (rows <= 0)
                    return new List<T>();
                list = list.OrderByDescending(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking();
                return list;
            }
            else
            {
                var list = DbSet.ToList();
                rows = list.Count();
                if (rows <= 0)
                    return new List<T>();
                list = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }


        ~GrainRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                if (db2 != null)
                {
                    db2.Dispose();
                    db2 = null;
                }
            }
            isDisposed = true;
        }


        #region Custom yhw

        /// <summary>
        /// add Temp and update temp
        /// </summary>
        /// <param name="list">Addlist </param>
        /// <param name="predicate">更新条件</param>
        /// <param name="updateKey">更新key</param>
        /// <param name="updateValue">更新值</param>
        /// <param name="defDate">更新时间</param>
        /// <returns></returns>
        public int AddUpdate(List<T> list, Expression<Func<T, bool>> predicate, string updateKey, int updateValue, string defDate)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    var plist = db.Set<T>().Where(predicate).ToList();
                    if (plist != null)
                    {
                        foreach (var current in plist)
                        {
                            if(!string.IsNullOrEmpty(defDate))
                                current.GetType().GetProperty(defDate).SetValue(current, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), null);
                            current.GetType().GetProperty(updateKey).SetValue(current, updateValue, null);
                            db.Set<T>().Attach(current);
                            db.Entry<T>(current).State = EntityState.Modified;
                        }
                    }

                    if (list != null && list.Count > 0)
                    {
                        foreach (var current in list)
                        {
                            db.Set<T>().Add(current);
                        }
                    }

                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return -22;
                }
            }
        }


        public int AddDelete(List<T> list, string[] selectKey,string defDate)
        {
            using (GrainContext db = new GrainContext())
            {
                try
                {
                    foreach (var current in list)
                    {
                        var t = db.Set<T>().Where(EfUtils.And<T>(selectKey, current)).FirstOrDefault<T>();
                        if (t != null)
                            db.Set<T>().Remove(t);
                        current.GetType().GetProperty(defDate).SetValue(current, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), null);
                        db.Set<T>().Add(current);
                    }

                    return db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return -22;
                }
            }
        }



        #endregion

    }
}
