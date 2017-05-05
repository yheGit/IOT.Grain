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
        [HttpPost]
        public ReturnData GetData(List<string> _params)
        {
            if (_params == null)
                return new ReturnData(1009);
            int total = 0;
            List<Sys_Menu> queryData = new MenuCore().GetMenuList(_params, ref total);
            var reList = new datagrid
            {
                total = total,
                rows = queryData.Select(s => new Sys_Menu
                {
                    Id = s.Id
                     ,
                    Name = s.Name
                     ,
                    IsLeaf = s.IsLeaf
                     ,
                    IsShow = s.IsShow
                     ,
                    Sort = s.Sort
                     ,
                    State = s.State
                      ,
                    Iconic = s.Iconic
                    ,
                    Remark = s.Remark
                     ,
                    LinkUrl = s.LinkUrl
                     ,
                    ParentId = s.ParentId

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
            Sys_Menu item = new MenuCore().GetMenuInfo(id);
            if (item != null)
                return new ReturnData(1000, "成功", new datagrid(item));
            return new ReturnData(1012);

        }

        /// <summary>
        /// 添加org
        /// </summary>
        [HttpPost]
        public ReturnData Create(Sys_Menu entity)
        {
            bool rebit = false;
            if (entity != null)
            {
                rebit = new MenuCore().IsExistMenu(entity);
                if (rebit == true)
                    return new ReturnData(1008, "已经存在该Code");
                entity.Id = Utils.GetNewId();
                rebit = new MenuCore().AddMenu(entity);
            }
            if (rebit == true)
                return new ReturnData(1000, "添加成功");
            return new ReturnData(1011);
        }


        /// <summary>
        /// 提交编辑信息
        /// </summary>
        [HttpPost]
        public ReturnData Edit(Sys_Menu entity)
        {
            bool rebit = false;
            if (entity == null || string.IsNullOrEmpty(entity.Id))
                return new ReturnData(1009);
            rebit = new MenuCore().UpdateMenu(entity);
            if (rebit == true)
                return new ReturnData(1000, "编辑成功");
            return new ReturnData(1011);

        }

        /// <summary>
        /// 删除
        /// 删除前由前端判断是否有依附数据存在（若存在，则提示）
        /// </summary>
        [HttpPost]
        public ReturnData Delete(List<Sys_Menu> _list)
        {
            bool rebit = false;
            if (_list == null || _list.Count <= 0)
                return new ReturnData(1009);
            rebit = new MenuCore().DeleteMenu(_list);
            if (rebit == true)
                return new ReturnData(1000, "删除成功");
            return new ReturnData(1011);
        }


    }
}