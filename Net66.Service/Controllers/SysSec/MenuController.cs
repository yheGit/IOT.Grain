using IOT.RightsSys.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers.SysSec
{
    public class MenuController : ApiController
    {
        // GET: Menu
        public bool Index()
        {
            return false;
        }

        /// <summary>
        /// 异步加载数据
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="rows">每页显示的行数</param>
        /// <param name="order">排序字段</param>
        /// <param name="sort">升序asc（默认）还是降序desc</param>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        public List<Sys_Menu> GetData(string id, int page, int rows, string order, string sort, string search)
        {

            int total = 0;
            List<Sys_Menu> queryData = null;//m_BLL.GetByParam(id, page, rows, order, sort, search, ref total);
            return queryData.Select(s => new Sys_Menu
            {
                Id = s.Id
                    ,
                Name = s.Name
                    ,
                ParentId = s.ParentId
                    ,
                //Url = s.Url
                    //,
                Iconic = s.Iconic
                    ,
                Sort = s.Sort
                    ,
                Remark = s.Remark
                    ,
                State = s.State

            }).ToList();
        }
        /// <summary>
        /// 查看详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_Menu Details(string id)
        {
            Sys_Menu item = null;// m_BLL.GetById(id);
            return item;

        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public bool Create(Sys_Menu entity)
        {
            if (entity != null )
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
        public bool Edit(string id, Sys_Menu entity)
        {
            if (entity != null)
            {   //数据校验

              
            }
            return false;             

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>   
        [HttpPost]
        public bool Delete()
        {
            string returnValue = string.Empty;
           
            return false;
        }



    }
}