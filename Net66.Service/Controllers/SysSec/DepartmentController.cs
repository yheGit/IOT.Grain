using IOT.RightsSys.Entity;
using Net66.Comm;
using Net66.Core.SysSecCore;
using Net66.Data.Base;
using Net66.Entity.IO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers.SysSec
{
    public class DepartmentController : ApiController
    {
        // GET: Department
        public bool Index()
        {
            return false;
        }

        [HttpGet]
        public dynamic test(string id)
        {
            var type = TypeParse.StrToInt(id, 0);
            return new DepartmentCore().GetOrgSelectList(type);
        }

        /// <summary>
        /// 获取组织结构SelectList
        /// </summary>
        [HttpGet]
        public ReturnData GetOrgSelectList(string id)
        {
            var type = TypeParse.StrToInt(id, 0);
            var queryData = new DepartmentCore().GetOrgSelectList(type);
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
            List<Sys_Department> queryData = new DepartmentCore().GetOrgList(_params, ref total);
            var reList = new datagrid
            {
                total = total,
                rows = queryData.Select(s => new Sys_Department
                {
                    Id = s.Id
                    ,
                    Code = s.Code
                     ,
                    Name = s.Name
                     ,
                    ParentId = s.ParentId
                     ,
                    Address = s.Address
                     ,
                    Sort = s.Sort
                     ,
                    Remark = s.Remark

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
            Sys_Department item = new DepartmentCore().GetOrgInfo(id);
            if (item != null)
                return new ReturnData(1000, "成功", new datagrid(item));
            return new ReturnData(1012);

        }

        /// <summary>
        /// 添加org
        /// </summary>
        [HttpPost]
        public ReturnData Create(Sys_Department entity)
        {
            bool rebit = false;
            if (entity != null)
            {
                rebit = new DepartmentCore().IsExistOrg(entity);
                if (rebit == true)
                    return new ReturnData(1008, "已经存在该Code");
                entity.IsShow = 0;
                entity.Id = Utils.GetNewId();
                rebit = new DepartmentCore().AddOrg(entity);
            }
            if (rebit == true)
                return new ReturnData(1000, "添加成功");
            return new ReturnData(1011);
        }


        /// <summary>
        /// 提交编辑信息
        /// </summary>
        [HttpPost]
        public ReturnData Edit(Sys_Department entity)
        {
            bool rebit = false;
            if (entity == null || string.IsNullOrEmpty(entity.Id))
                return new ReturnData(1009);
            rebit = new DepartmentCore().UpdateOrg(entity);
            if (rebit == true)
                return new ReturnData(1000, "编辑成功");
            return new ReturnData(1011);

        }

        /// <summary>
        /// 删除
        /// 删除前由前端判断是否有依附数据存在（若存在，则提示）
        /// </summary>
        [HttpPost]
        public ReturnData Delete(List<Sys_Department> _list)
        {
            bool rebit = false;
            if (_list == null || _list.Count <= 0)
                return new ReturnData(1009);
            rebit = new DepartmentCore().DeleteOrg(_list);
            if (rebit == true)
                return new ReturnData(1000, "删除成功");
            return new ReturnData(1011);
        }


        /// <summary>
        /// 获取树形列表的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<Sys_Department> GetAllMetadata(string id)
        {

            List<Sys_Department> rows = null;//m_BLL.GetAllMetadata(id);

            return rows;
        }


    }
}