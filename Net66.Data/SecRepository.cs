using System.Linq;
using System.Data.Entity;
using Net66.Data.Context;

namespace Net66.Core.SysSecCore
{
    public abstract class SecRepository<T> where T : class
    {
        public string Start_Time { get { return "Start_Time"; } }
        public string End_Time { get { return "End_Time"; } }
        public string Start_Int { get { return "Start_Int"; } }
        public string End_Int { get { return "End_Int"; } }

        public string End_String { get { return "End_String"; } }
        public string DDL_String { get { return "DDL_String"; } }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns>集合</returns>
        public virtual IQueryable<T> GetAll()
        {
            using (DbSysSEC db = new DbSysSEC())
            {
                return GetAll(db);
            }
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns>集合</returns>
        public virtual IQueryable<T> GetAll(DbSysSEC db)
        {
            return db.Set<T>().AsQueryable();
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="db">实体数据</param>
        /// <param name="entity">将要创建的对象</param>
        public virtual void Create(DbSysSEC db, T entity)
        {
            if (entity != null)
            {
                db.Set<T>().Add(entity);
            }
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity">一个对象</param>
        /// <returns></returns>
        public virtual int Create(T entity)
        {
            using (DbSysSEC db = new DbSysSEC())
            {
                Create(db, entity);
                return this.Save(db);
            }
        }
        /// <summary>
        /// 创建对象集合
        /// </summary>
        /// <param name="db">实体数据</param>
        /// <param name="entitys">对象集合</param>
        public virtual void Create(DbSysSEC db, IQueryable<T> entitys)
        {
            foreach (var entity in entitys)
            {
                this.Create(db, entity);
            }
        }
   
        /// <summary>
        /// 编辑一个对象
        /// </summary>
        /// <param name="db">实体数据</param>
        /// <param name="entity">将要编辑的一个对象</param>
        public virtual T Edit(DbSysSEC db, T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry(entity).State = EntityState.Modified;
            return entity;
        }
        /// <summary>
        /// 编辑对象集合
        /// </summary>
        /// <param name="db">实体数据</param>
        /// <param name="entitys">对象集合</param>
        public virtual void Edit(DbSysSEC db, IQueryable<T> entitys)
        {
            foreach (T entity in entitys)
            {
                this.Edit(db, entity);
            }
        }
        /// <summary>
        /// 提交保存，持久化到数据库
        /// </summary>
        /// <param name="db">实体数据</param>
        /// <returns>受影响行数</returns>
        public int Save(DbSysSEC db)
        {
            return db.SaveChanges();
        }

    }
}

