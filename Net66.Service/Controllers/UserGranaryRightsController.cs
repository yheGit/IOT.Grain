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
        public bool Index()
        {
            return false;
        }

        [HttpGet]
        public ReturnData GetUsersAndGranarysByOrg(string orgId)
        {
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

    }
}