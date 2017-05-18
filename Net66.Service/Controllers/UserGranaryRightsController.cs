using IOT.RightsSys.Entity;
using Net66.Core;
using Net66.Core.Interface;
using Net66.Core.SysSecCore;
using Net66.Entity.IO_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers
{
    public class UserGranaryRightsController : ApiController
    {

        public static IWareHouseCore wareHouseCore;

        public UserGranaryRightsController(IWareHouseCore _wareHouseCore)
        {
            wareHouseCore = _wareHouseCore;
        }

        // GET: UserGranaryRights
        /// <summary>
        /// 通过用户ID获取其拥有的仓结构
        /// </summary>
        [HttpGet]
        public ReturnData GetUserGranaryListByUid(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new ReturnData(1009);
            var userid = id;
           var relist= new UserGranaryRightsCore().GetUserGranaryListByUid(userid);
            if (relist != null)
                return new ReturnData(1000, "成功", new datagrid() { total = -1, rows = relist });
            return new ReturnData(1012);
        }

        /// <summary>
        /// 通过组织id获取 旗下对应的仓结构和人员信息
        /// </summary>
        [HttpGet]
        public ReturnData GetUsersAndGranarysByOrg(string id)
        {
            var orgId = id;
            if (string.IsNullOrEmpty(orgId))
                return new ReturnData(1009);
           var personlist= new UserGranaryRightsCore().GetAllPersonByOrg(orgId);
            var grainlist= wareHouseCore.GetGrainTree(orgId);
            if (personlist == null && grainlist == null)
                return new ReturnData(1012);
            var relist = new {
                personArr = personlist,
                grainArr= grainlist
            };
            return new ReturnData(1000,"成功",new datagrid() { total=-1,rows= relist });
        }

        /// <summary>
        /// 设置用户拥有的仓结构权限（添加、修改）
        /// </summary>
        [HttpPost]
        public ReturnData SetUsersGranaryRights(List<UserGranaryRights> list)
        {
            if (list == null || list.Count <= 0)
                return new ReturnData(1009);
           var rebit= new UserGranaryRightsCore().Set_UserGranaryRights(list);
            if (rebit)
                return new ReturnData(1000);
            return new ReturnData(1011);
        }

        /// <summary>
        /// 删除用户拥有的仓结构权限
        /// </summary>
        [HttpPost]
        public ReturnData DelUsersGranaryRights(List<UserGranaryRights> list)
        {
            if (list == null || list.Count <= 0)
                return new ReturnData(1009);
            var rebit = new UserGranaryRightsCore().Set_UserGranaryRights(list,1);
            if (rebit)
                return new ReturnData(1000);
            return new ReturnData(1011);
        }


    }
}