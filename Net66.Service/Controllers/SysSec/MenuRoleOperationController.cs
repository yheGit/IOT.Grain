using IOT.RightsSys.Entity;
using Net66.Core.SysSecCore;
using Net66.Entity.IO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers.SysSec
{
    public class MenuRoleOperationController : ApiController
    {
        // GET: MenuRoleOperation
        public bool Index()
        {
            return false;
        }


        /// <summary>
        /// 异步加载菜单
        /// </summary>
        public ReturnData GetData(string id)
        {
            if (id == null)
                return new ReturnData(1009);            
            List<Sys_Menu> queryData = new MenuCore().GetMenuListByRole(id);
            int total = queryData.Count();
            var reList = new datagrid
            {
                total = total,
                rows = queryData
            };
            if (total > 0 && reList.rows != null)
                return new ReturnData(1000, "成功", reList);
            else
                return new ReturnData(1012);

        }

        [HttpGet]
        public ReturnData GetOperationData(string RoleID,string MenuID)
        {
            if (string.IsNullOrEmpty(RoleID)|| string.IsNullOrEmpty(MenuID))
                return new ReturnData(1009);
            List<Sys_Operation> queryData = new OperationCore().GetOperationByMenuRole(MenuID, RoleID);
            int total = queryData.Count();
            var reList = new datagrid
            {
                total = total,
                rows = queryData
            };
            if (total > 0 && reList.rows != null)
                return new ReturnData(1000, "成功", reList);
            else
                return new ReturnData(1012);

        }



    }
}