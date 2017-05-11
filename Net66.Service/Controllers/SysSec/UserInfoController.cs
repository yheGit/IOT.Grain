using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Core.SysSecCore;
using Net66.Entity.IO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers.SysSec
{
    /// <summary>
    /// 人员
    /// </summary>
    public class UserInfoController : ApiController
    {
        // GET: UserInfo
        public bool Index()
        {
            return false;
        }

        /// <summary>
        /// 验证用户名是否存在
        /// </summary>
        /// <param name="name">用户名</param>
        [HttpGet]
        public ReturnData CheckName(string id)
        {
            var entity = new Sys_UserInfo() { LoginID=id};
            bool rebit = new UserInfoCore().IsExistUser(entity);
            if (rebit == true)
                return new ReturnData(1008, "已经存在该Code");
            return new ReturnData(1000,"恭喜你，可以使用。");

        }

        /// <summary>
        /// 异步加载数据
        /// </summary>
        [HttpPost]
        public ReturnData GetData(List<string> _params)
        {
            if (_params == null)
                return new ReturnData(1009);
            int total = 0;
            List<Sys_UserInfo> queryData = new UserInfoCore().GetUserList(_params, ref total);
            var reList = new datagrid
            {
                total = total,
                rows = queryData.Select(s => new Sys_UserInfo
                {
                    Id = s.Id
                     ,
                    NickName = s.NickName
                     ,
                    LoginID = s.LoginID
                     ,
                    IsShow = s.IsShow
                     ,
                    Address = s.Address
                     ,
                    State = s.State
                      ,
                    DepartmentId = s.DepartmentId
                    ,
                    Remark = s.Remark
                     ,
                    EmailAddress = s.EmailAddress
                     ,
                    Password = s.Password
                      ,
                    PhoneNumber = s.PhoneNumber
                     ,
                    RoleId = s.RoleId
                      ,
                    Sex = s.Sex
                       ,
                    TelPhone = s.TelPhone

                })
            };
            if (total > 0 && reList.rows != null)
                return new ReturnData(1000, "成功", reList);
            else
                return new ReturnData(1012);

        }


        /// <summary>
        /// 查看详细
        /// </summary>
        public ReturnData Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new ReturnData(1009);
            Sys_UserInfo item = new UserInfoCore().GetUserInfo(id);
            if (item != null)
                return new ReturnData(1000, "成功", new datagrid(item));
            return new ReturnData(1012);

        }

        /// <summary>
        /// 添加org
        /// </summary>
        [HttpPost]
        public ReturnData Create(Sys_UserInfo entity)
        {
            bool rebit = false;
            if (entity != null)
            {
                rebit = new UserInfoCore().IsExistUser(entity);
                if (rebit == true)
                    return new ReturnData(1008, "已经存在该Code");
                entity.Id = Utils.GetNewId();
                entity.Password = EncryptAndDecrypte.EncryptString(entity.Password);
                rebit = new UserInfoCore().AddUser(entity);
            }
            if (rebit == true)
                return new ReturnData(1000, "添加成功");
            return new ReturnData(1011);
        }


        /// <summary>
        /// 提交编辑信息
        /// </summary>
        [HttpPost]
        public ReturnData Edit(Sys_UserInfo entity)
        {
            bool rebit = false;
            if (entity == null || string.IsNullOrEmpty(entity.Id))
                return new ReturnData(1009);
            entity.Password = EncryptAndDecrypte.EncryptString(entity.Password);
            rebit = new UserInfoCore().UpdateUser(entity);
            if (rebit == true)
                return new ReturnData(1000, "编辑成功");
            return new ReturnData(1011);

        }

        /// <summary>
        /// 删除
        /// 删除前由前端判断是否有依附数据存在（若存在，则提示）
        /// </summary>
        [HttpPost]
        public ReturnData Delete(List<Sys_UserInfo> _list)
        {
            bool rebit = false;
            if (_list == null || _list.Count <= 0)
                return new ReturnData(1009);
            rebit = new UserInfoCore().DeleteUser(_list);
            if (rebit == true)
                return new ReturnData(1000, "删除成功");
            return new ReturnData(1011);
        }



    }
}