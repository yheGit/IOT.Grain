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
    public class GranaryController : ApiController
    {
        public static IGranaryCore granaryCore;

        public GranaryController(IGranaryCore _granaryCore)
        {
            granaryCore = _granaryCore;
        }

        [HttpPost]
        //[System.Web.Mvc.Route("PostArray")]
        public MobiResult Add(List<Granary> _addList)
        {
            if (_addList == null || _addList.Count <= 0)
                return new MobiResult(1009);
            var reBit = granaryCore.AddGranary(_addList);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Modify(Granary _entity)
        {
            if (_entity == null)
                return new MobiResult(1009);
            var reBit = granaryCore.UpdateGranary(_entity);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Delete(List<Granary> _delList)
        {
            if (_delList == null || _delList.Count < 0)
                return new MobiResult(1009);
            var reBit = granaryCore.DeleteGranary(_delList);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }


        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult IsExist(Granary _entity)
        {
            if (_entity == null)
                return new MobiResult(1009);
            var reBit = granaryCore.IsExist(_entity);
            if (reBit == true)
                return new MobiResult(1000, "存在");
            else
                return new MobiResult(1011, "不存在");
        }

        [HttpPost]
        public MobiResult GetList(List<int> ids)
        {
            if (ids == null || ids.Count <= 0)
                return new MobiResult(1009);
            var reList = granaryCore.GetHeapList(ids);
            if (reList!=null)
                return new MobiResult(1000, "成功", reList);
            else
                return new MobiResult(1011);
        }

    }
}