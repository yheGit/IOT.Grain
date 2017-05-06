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

namespace Net66.Core.SysSecCore
{
   public class UserInfoCore : SecRepository<Sys_UserInfo>
    {

        #region 用户管理
        public List<Sys_UserInfo> GetUserList(List<string> _params, ref int total)
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

            Expression<Func<Sys_UserInfo, bool>> predicate = EfUtils.True<Sys_UserInfo>();
            predicate = p => p.IsShow == 0; ;
            ////var wCode = TypeParse.StrToInt(wareCode, 0);
            //where = where.And(w => w.IsActive == 1 && w.WH_Number == wareCode && w.Type == 0);
            //if (!string.IsNullOrEmpty(granaryCode))
            //    where = where.And(w => w.Number.Contains(granaryCode));//loufangleixing

            //排序字段转换
            Expression<Func<Sys_UserInfo, string>> orderByLambda = p => p.LoginID.ToString();//null;            
            switch (sort)
            {
                case "NickName": orderByLambda = p => p.NickName; break;
                case "LoginID": orderByLambda = p => p.LoginID.ToString(); break;
                    //default: orderByLambda = p => p.Sort.ToString(); break;
            }

            #endregion

            IQueryable<Sys_UserInfo> queryData = null;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                if (isasc)
                    queryData = dbEntity.UserInfos.Where(predicate).OrderBy(orderByLambda);
                else
                    queryData = dbEntity.UserInfos.Where(predicate).OrderByDescending(orderByLambda);
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
        /// 通过主键获取用户的详细信息
        /// </summary>
        public Sys_UserInfo GetUserInfo(string guid)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                ////调试模式则输出SQL
                //if (Utils.DebugApp)
                //    dbEntity.Database.Log = new Action<string>(q => System.Diagnostics.Debug.WriteLine(q));
                return dbEntity.UserInfos.Where(w => w.Id == guid).FirstOrDefault();
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        public bool AddUser(Sys_UserInfo _entity)
        {
            int recordsAffected;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                dbEntity.UserInfos.Add(_entity);
                recordsAffected = dbEntity.SaveChanges();
                return recordsAffected > 0;
            }
        }

        /// <summary>
        /// 是否存在该用户
        /// </summary>
        public bool IsExistUser(Sys_UserInfo _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info = dbEntity.UserInfos.FirstOrDefault(f => f.LoginID == _entity.LoginID.Trim());
                if (info != null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        public bool UpdateUser(Sys_UserInfo _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info = dbEntity.UserInfos.FirstOrDefault(f => f.Id == _entity.Id);
                if (info != null)
                {
                    info.NickName = _entity.NickName;
                    info.LoginID = _entity.LoginID;
                    info.State = _entity.State;
                    info.Password = _entity.Password; 
                    info.PhoneNumber = _entity.PhoneNumber;
                    info.Remark = _entity.Remark;
                    info.RoleId = _entity.RoleId;
                    info.Sex = _entity.Sex;
                    info.TelPhone = _entity.TelPhone;
                    info.Address = _entity.Address;
                    info.DepartmentId = _entity.DepartmentId;
                    info.EmailAddress = _entity.EmailAddress;
                }
                dbEntity.Set<Sys_UserInfo>().Attach(info);
                dbEntity.Entry(info).State = EntityState.Modified;
                return dbEntity.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除用户
        /// 参数实为 主键集合
        /// </summary>
        public bool DeleteUser(List<Sys_UserInfo> _list)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                foreach (var model in _list)
                {
                    var info = dbEntity.UserInfos.FirstOrDefault(f => f.Id == model.Id);
                    if (info == null)
                        continue;
                    info.IsShow = -4;
                    dbEntity.Set<Sys_UserInfo>().Attach(info);
                    dbEntity.Entry(info).State = EntityState.Modified;
                }
                return dbEntity.SaveChanges() > 0;
            }
        }



        #endregion

    }
}
