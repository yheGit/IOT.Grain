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
    public class RoleController : ApiController
    {
        // GET: Role
        public bool Index()
        {
            return false;
        }


        [HttpPost]
        public ReturnData SaveRoleRights(IMenuOperation collection)
        {
            bool rebit = false;
            if (collection == null || string.IsNullOrEmpty(collection.roleid))
                return new ReturnData(1009);
            string[] ids = collection.ids.Split(',');
            rebit = new RoleCore().SaveCollection(ids, collection.roleid);
            if (rebit == true)
                return new ReturnData(1000, "操作成功");
            return new ReturnData(1011);

        }

        [HttpGet]
        public IMenuOperation test(string id)
        {
            return new IMenuOperation();
        }

        /// <summary>
        /// 获取组织中的角色SelectList
        /// </summary>
        [HttpGet]
        public ReturnData GetRoleSelectList(string id)
        {

            var queryData = new RoleCore().GetRoleList(id);
            var reList = new datagrid()
            {
                total = -1,
                rows = queryData
            };
            if (queryData != null)
                return new ReturnData(1000, "成功", reList);
            else
                return new ReturnData(1012);
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
            List<Sys_Role> queryData = new RoleCore().GetRoleList(_params, ref total);
            var reList = new datagrid
            {
                total = total,
                rows = queryData.Select(s => new Sys_Role
                {
                    Id = s.Id
                     ,
                    Name = s.Name
                     ,
                    Description = s.Description
                     ,
                    IsShow = s.IsShow
                     ,
                    Sort = s.Sort
                     ,
                    State = s.State
                      ,
                    UpdateTime = s.UpdateTime

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
            Sys_Role item = new RoleCore().GetRoleInfo(id);
            if (item != null)
                return new ReturnData(1000, "成功", new datagrid(item));
            return new ReturnData(1012);

        }

        /// <summary>
        /// 添加org
        /// </summary>
        [HttpPost]
        public ReturnData Create(Sys_Role entity)
        {
            bool rebit = false;
            if (entity != null)
            {
                rebit = new RoleCore().IsExistRole(entity);
                if (rebit == true)
                    return new ReturnData(1008, "已经存在该Code");
                entity.Id = Utils.GetNewId();
                entity.IsShow = 0;
                rebit = new RoleCore().AddRole(entity);
            }
            if (rebit == true)
                return new ReturnData(1000, "添加成功");
            return new ReturnData(1011);
        }


        /// <summary>
        /// 提交编辑信息
        /// </summary>
        [HttpPost]
        public ReturnData Edit(Sys_Role entity)
        {
            bool rebit = false;
            if (entity == null || string.IsNullOrEmpty(entity.Id))
                return new ReturnData(1009);
            rebit = new RoleCore().UpdateRole(entity);
            if (rebit == true)
                return new ReturnData(1000, "编辑成功");
            return new ReturnData(1011);

        }

        /// <summary>
        /// 删除
        /// 删除前由前端判断是否有依附数据存在（若存在，则提示）
        /// </summary>
        [HttpPost]
        public ReturnData Delete(List<Sys_Role> _list)
        {
            bool rebit = false;
            if (_list == null || _list.Count <= 0)
                return new ReturnData(1009);
            rebit = new RoleCore().DeleteRole(_list);
            if (rebit == true)
                return new ReturnData(1000, "删除成功");
            return new ReturnData(1011);
        }


    }
}