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
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_Department Details(string id)
        {
            Sys_Department item = null;// m_BLL.GetById(id);
            return item;

        }


        [HttpPost]
        public bool Create(Sys_Department entity)
        {
            if (entity != null)
            {

            }

            return false;
        }


        /// <summary>
        /// 提交编辑信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="collection">客户端传回的集合</param>
        /// <returns></returns>
        [HttpPost]
        public bool Edit(string id, Sys_Department entity)
        {
            if (entity != null)
            {   //数据校验


            }
            return false;

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