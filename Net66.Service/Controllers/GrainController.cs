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

        [HttpGet]
        /// <summary>
        /// 获取该组织结构下的所有仓结构 2017-6-8 14:41:51
        /// </summary>
        public ReturnData GetListByOrgid(string id)
        {           
            if (string.IsNullOrEmpty(id))
                return new ReturnData(1009);
            var orgId = id;
            if (id.Equals("0"))
                orgId = "";
            var grainlist = wareHouseCore.GetGrainTree(orgId);
            if (grainlist == null)
                return new ReturnData(1012);
            return new ReturnData(1000, "成功", new datagrid() { total = -1, rows = grainlist });
        }
        /// <summary>
        /// 查寻粮仓信息（包涵楼层、廒间信息）
        /// </summary>
        [HttpPost]
        //[System.Web.Mvc.Route("PostArray")]  
        public MobiResult GetList(ISearch _search)
        {

            Utils.PrintLog("GetList", "GetList");
            var _params = _search.DicList;
            if (_params == null) return new MobiResult(1009, "参数不合法，请用可选参数占位，如TextValue", null, "'DicList': [ ],");
            if (!_params.Exists(e => e.Contains("UserId^")) || !_params.Exists(e => e.Contains("Type^")))
                return new MobiResult(1009);
            var reList = wareHouseCore.GetPageLists(_search, _params);
            if (reList != null)
                return new MobiResult(1000, "成功", reList);
            else
                return new MobiResult(1012);
        }

        /// <summary>
        /// 添加粮仓信息 
        /// </summary>
        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Add(IWareHouse _entity)
        {
            //var jsonStr=JsonConvertHelper.SerializeObject(_entity);
            if (_entity == null)
                return new MobiResult(1009);
            var reBit = wareHouseCore.AddWareHouse(_entity);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        /// <summary>
        /// 修改粮仓信息
        /// </summary>
        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Modify(WareHouse _entity)
        {
            //var jsonStr=JsonConvertHelper.SerializeObject(_entity);
            if (_entity == null)
                return new MobiResult(1009);
            var reBit = wareHouseCore.UpdateWareHouse(_entity);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        /// <summary>
        /// 批量删除粮仓信息
        /// </summary>
        [HttpPost]
        [System.Web.Mvc.Route("PostArray")]
        public MobiResult Delete(List<WareHouse> _delList)
        {
            //var jsonStr=JsonConvertHelper.SerializeObject(_entity);
            if (_delList == null || _delList.Count < 0)
                return new MobiResult(1009);
            var reBit = wareHouseCore.DeleteWareHouse(_delList);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1011);
        }

        /// <summary>
        /// 验证粮仓是否已经存在
        /// </summary>
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
                return new MobiResult(1011, "不存在");
        }

        /// <summary>
        /// 移动设备展示粮仓bao'biao
        /// 移动端首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public MobiResult GetList_GrainReport_ByUserId()
        {
            #region  调试打印日志               
            if (Utils.DebugApp)
            {
                Utils.PrintLog("GetList_GrainReport_ByUserId", "GetList_GrainReport_ByUserId-ME移动端首页", "InParamLog");
            }
            #endregion //调试打印日志
            List<OGrainsReport> reList=new List<OGrainsReport>();
            try
            {
                reList = wareHouseCore.GetGrainsTemp();
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "GetList_GrainReport_ByUserId");
            }
            #region  调试打印日志               
            if (Utils.DebugApp)
            {
                var msg = JsonConvertHelper.SerializeObject(reList);
                Utils.PrintLog(msg, "GetList_GrainReport_ByUserId-ME移动端首页", "OutParamLog");
            }
            #endregion //调试打印日志
            if (reList != null)
                return new MobiResult(1000, "成功", reList);
            else
                return new MobiResult(1012);

        }

        /// <summary>
        /// 廒间温度报表
        /// </summary>
        [HttpGet]
        public MobiResult GranaryTemp_GetList(string id)
        {
            string number = id;//粮仓编号
            #region  调试打印日志               
            if (Utils.DebugApp)
            {
                var msg = "number:" + id;
                Utils.PrintLog(msg, "GranaryTemp_GetList-ME廒间温度报表", "InParamLog");
            }
            #endregion //调试打印日志

            if (string.IsNullOrEmpty(number))
                return new MobiResult(1009);
            var reList = wareHouseCore.getGranaryTemp(number);
            #region  调试打印日志               
            if (Utils.DebugApp)
            {
                var msg = JsonConvertHelper.SerializeObject(reList);
                Utils.PrintLog(msg, "GranaryTemp_GetList-ME廒间温度报表", "OutParamLog");
            }
            #endregion //调试打印日志
            if (reList != null)
                return new MobiResult(1000, "成功", reList);
            else
                return new MobiResult(1012);
        }


        /// <summary>
        /// 堆位温度报表
        /// </summary>
        [HttpGet]
        public MobiResult HeapsTemp_GetList(string id)
        {

            string number = id;//廒间编号

            #region  调试打印日志               
            if (Utils.DebugApp)
            {
                var msg = "number:"+id;
                Utils.PrintLog(msg, "HeapsTemp_GetList-ME堆位温度报表", "InParamLog");
            }
            #endregion //调试打印日志

            if (string.IsNullOrEmpty(number))
                return new MobiResult(1009);
            try
            {
                var reList = wareHouseCore.getHeapsTemp(number);
                #region  调试打印日志               
                if (Utils.DebugApp)
                {
                    var msg = JsonConvertHelper.SerializeObject(reList);
                    Utils.PrintLog(msg, "HeapsTemp_GetList-ME堆位温度报表", "OutParamLog");
                }
                #endregion //调试打印日志
                if (reList != null)
                    return new MobiResult(1000, "成功", reList);
                else
                    return new MobiResult(1012);
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "HeapsTemp_GetList");
                return new MobiResult(1013, "服务器异常");
            }
        }

    }
}
