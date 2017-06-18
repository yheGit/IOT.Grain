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
    public class UserVxinInfoCore : SecRepository<UserVxinInfo>
    {


        #region 用户微信关系绑定

        /// <summary>
        /// 通过主键获取角色的详细信息
        /// </summary>
        public UserVxinInfo GetVxinInfo(string loginid)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            { 
                return dbEntity.UserVxinInfos.Where(w => w.XvinID == loginid).FirstOrDefault();
            }
        }

        /// <summary>
        /// 绑定用户微信ID与账号
        /// </summary>
        public bool AddVxin(UserVxinInfo _entity)
        {
            int recordsAffected;
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                dbEntity.UserVxinInfos.Add(_entity);
                recordsAffected = dbEntity.SaveChanges();
                return recordsAffected > 0;
            }
        }

        /// <summary>
        /// 是否存在改角色
        /// </summary>
        public bool IsExistVxin(UserVxinInfo _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info = dbEntity.UserVxinInfos.FirstOrDefault(f => f.XvinID == _entity.XvinID.Trim());
                if (info != null)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        public bool UpdateVxin(UserVxinInfo _entity)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                var info = dbEntity.UserVxinInfos.FirstOrDefault(f => f.XvinID == _entity.XvinID);
                if (info != null)
                {
                    info.UserLoginID = _entity.UserLoginID;
                    info.SendTime = _entity.SendTime;

                }
                dbEntity.Set<UserVxinInfo>().Attach(info);
                dbEntity.Entry(info).State = EntityState.Modified;
                return dbEntity.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// 删除角色信息
        /// 参数实为 主键集合
        /// </summary>
        public bool DeleteVxin(List<UserVxinInfo> _list)
        {
            using (DbSysSEC dbEntity = new DbSysSEC("DB_SEC"))
            {
                foreach (var model in _list)
                {
                    var info = dbEntity.UserVxinInfos.FirstOrDefault(f => f.XvinID == model.XvinID);
                    if (info == null)
                        continue;
                    dbEntity.Set<UserVxinInfo>().Attach(info);
                    dbEntity.Entry(info).State = EntityState.Deleted;
                }
                return dbEntity.SaveChanges() > 0;
            }
        }



        #endregion



    }
}
