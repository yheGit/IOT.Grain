using Net66.Comm;
using Net66.Core;
using Net66.Core.Interface;
using Net66.Data.Base;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Net66.Service.Controllers
{
    public class GrainController : ApiController
    {
        public static IWareHouseCore wareHouseCore;

        public GrainController(IWareHouseCore _wareHouseCore)
        {
            wareHouseCore = _wareHouseCore;
        }

        [HttpPost]
        //[System.Web.Mvc.Route("PostArray")]  
        public MobiResult GetList(ISearch _search)
        {
            var _params = _search.DicList;
            if (_params == null) return new MobiResult(1009, "参数不合法，请用可选参数占位，如TextValue", null, "'DicList': [ ],");
            if (!_params.Exists(e => e.Contains("UserId^")) || !_params.Exists(e => e.Contains("Type^")))
                return new MobiResult(1009);
            //if (!_search.Dic.Any() || !_search.Dic.ContainsKey("UserId") || !_search.Dic.ContainsKey("Type"))
            //    return new MobiResult(1009);
            //IWareHouseCore wareHouseCore = new WareHouseCore();
            var reList = wareHouseCore.GetPageLists(_search, _params);
            if (reList != null)
                return new MobiResult(1000, "成功", reList);
            else
                return new MobiResult(1012);
        }

        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Add(IWareHouse _entity)
        {
            //var jsonStr=JsonConvertHelper.SerializeObject(_entity);
            if (_entity == null)
                return new MobiResult(1009);
            //var hasExist = wareHouseCore.HasExist(_entity.Number);
            //if (hasExist)
            //    return new MobiResult(1015, "该粮仓已存在", null, _entity.Number);
            var reBit = wareHouseCore.AddWareHouse(_entity);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }


        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Modify(WareHouse _entity)
        {
            //var jsonStr=JsonConvertHelper.SerializeObject(_entity);
            if (_entity == null)
                return new MobiResult(1009);
            //var hasExist = wareHouseCore.HasExist(_entity.Number);
            //if (!hasExist)
            //    return new MobiResult(1015, "该粮仓不存在", null, _entity.Number);
            var reBit = wareHouseCore.UpdateWareHouse(_entity);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }


        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Delete(List<WareHouse> _delList)
        {
            //var jsonStr=JsonConvertHelper.SerializeObject(_entity);
            if (_delList == null|| _delList.Count<0)
                return new MobiResult(1009);
            var reBit = wareHouseCore.DeleteWareHouse(_delList);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        [HttpGet]
        //[System.Web.Mvc.Route("Grain/IsExist/{_code}")]
        public MobiResult IsExist(string id)
        {
           string _code = id;
            //var jsonStr=JsonConvertHelper.SerializeObject(_entity);
            if (string.IsNullOrEmpty(_code))
                return new MobiResult(1009);
            var reBit = wareHouseCore.HasExist(_code);
            if (reBit == true)
                return new MobiResult(1000, "存在");
            else
                return new MobiResult(1011,"不存在");
        }

        public MobiResult GetList_GrainReport_ByUserId()
        {
            var reList = wareHouseCore.GetGrainsTemp();
            if (reList != null)
                return new MobiResult(1000, "成功", reList);
            else
                return new MobiResult(1012);
        }


    }
}
