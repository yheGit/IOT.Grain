using Net66.Core.Interface;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers
{
    public class FloorController : ApiController
    {
        public static IFloorCore floorCore;

        public FloorController(IFloorCore _floorCore)
        {
            floorCore = _floorCore;
        }

        [HttpPost]
        //[System.Web.Mvc.Route("PostArray")]
        public MobiResult Add(List<Floor> _addList)
        {
            if (_addList == null || _addList.Count <= 0)
                return new MobiResult(1009);
            var reBit = floorCore.AddFloor(_addList);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Modify(Floor _entity)
        {
            if (_entity == null)
                return new MobiResult(1009);
            var reBit = floorCore.UpdateFloor(_entity);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Delete(List<Floor> _delList)
        {
            if (_delList == null || _delList.Count < 0)
                return new MobiResult(1009);
            var reBit = floorCore.DeleteFloor(_delList);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }


        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult IsExist(Floor _entity)
        {
            if (_entity==null)
                return new MobiResult(1009);
            var reBit = floorCore.HasExist(_entity);
            if (reBit == true)
                return new MobiResult(1000, "存在");
            else
                return new MobiResult(1011, "不存在");
        }

    }
}