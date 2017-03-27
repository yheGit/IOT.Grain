using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


/******************************************
*Creater:yhw[]
*CreatTime:
*Description:
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:
*******************************************/
namespace Net66.Data.Interface
{
    public interface IGrainRepository<T> where T : class
    {
        int Add(T entity, Expression<Func<T, bool>> predicate);

        int AddDelete(T entity, Expression<Func<T, bool>> predicate);
        int AddUpdate(List<T> list, string[] selectKey, string[] updateKey);
        int AddUpdateDelete(List<T> addupdateList, List<T> deleteList, string[] selectKey, string selectOrKey, string[] updateKey, string[] deleteKey, string defDate);
        int Add(T entity);
        int Add(List<T> list, string[] AndKeys, string OrKeys);
        int Add(List<T> list);
        int Update(T entity);
        int Update(List<T> list, string[] selectKey, string[] updateKey, string defDate);
        int AddUpdate(List<T> list, string[] selectKey, string[] updateKey, string defDate);
        int Delete(T entity);

        int Delete(List<T> deleteList, string[] deleteKey);
        T Get(Expression<Func<T, bool>> predicate);
        List<T> GetList(Expression<Func<T, bool>> predicate);
        DataTable QueryByTablde(string sql);
        int ExecuteSql(string sql);

        List<T> GetPageLists(Expression<Func<T, bool>> where, Expression<Func<T, string>> orderBy, bool ascending, int pageIndex, int pageSize, ref int rows);
        IEnumerable<T> GetPageLists2(Expression<Func<T, bool>> where, Expression<Func<T, string>> orderBy, bool ascending, int pageIndex, int pageSize, ref int rows);




        #region Custom yhw

        /// <summary>
        /// add Temp and update temp
        /// </summary>
        int AddUpdate(List<T> list, Expression<Func<T, bool>> predicate, string updateKey, int updateValue, string defDate);

        #endregion



    }
}
