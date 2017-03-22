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
    public class HeapController : ApiController
    {
        public static IHeapCore heapCore;

        public HeapController(IHeapCore _heapCore)
        {
            heapCore = _heapCore;
        }

        [HttpPost]
        //[System.Web.Mvc.Route("PostArray")]  
        public MobiResult GetList(ISearch _search)
        {
            var _params = _search.DicList;
            if (_params == null) return new MobiResult(1009, "参数不合法，请用可选参数占位，如TextValue", null, "'DicList': [ ],");
            if (!_params.Exists(e => e.Contains("UserId^")) || !_params.Exists(e => e.Contains("gCode^")))
                return new MobiResult(1009);
            var reList = heapCore.GetPageLists(_search, _params);
            if (reList != null)
                return new MobiResult(1000, "成功", reList);
            else
                return new MobiResult(1011);
        }

        [HttpPost]
        //[System.Web.Mvc.Route("PostArray")]
        public MobiResult Add(List<Heap> _addList)
        {
            if (_addList == null || _addList.Count <= 0)
                return new MobiResult(1009);
            var reBit = heapCore.AddHeap(_addList);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Modify(Heap _entity)
        {
            if (_entity == null)
                return new MobiResult(1009);
            var reBit = heapCore.UpdateHeap(_entity);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Delete(List<Heap> _delList)
        {
            if (_delList == null || _delList.Count < 0)
                return new MobiResult(1009);
            var reBit = heapCore.DeleteHeap(_delList);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }


        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult IsExist(Heap _entity)
        {
            if (_entity == null)
                return new MobiResult(1009);
            var reBit = heapCore.IsExist(_entity);
            if (reBit == true)
                return new MobiResult(1000, "存在");
            else
                return new MobiResult(1011, "不存在");
        }


    }
}