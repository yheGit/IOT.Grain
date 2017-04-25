using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Net66.Core.Interface;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;

namespace Net66.Service.Controllers
{
    public class SensorController : ApiController
    {

        public static ISensorCore sensorCore;

        public SensorController(ISensorCore _sensorCore)
        {
            sensorCore = _sensorCore;
        }

        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}


        /// <summary>
        /// 给筒仓编号设定序号
        /// </summary>
        [HttpPost]
        public MobiResult UpdateLineCodeInfo(List<ISensorBase> list)
        {
            if (list == null || list.Count < 0)
                return new MobiResult(1009);
            var reBit = sensorCore.UpdateSensorBaseList(list);
            if (reBit == true)
                return new MobiResult(1000, "成功");
            else
                return new MobiResult(1012);

        }


        /// <summary>
        /// 获取筒仓 传感线的数量
        /// </summary>
        [HttpGet]
        public MobiResult GetHeapLineCount(string id)
        {
            string number = id;
            var reList = sensorCore.GetHeapLineCount(number);
            if (reList != null)
                return new MobiResult(1000, "成功", reList);
            else
                return new MobiResult(1012);
        }


        /// <summary>
        /// 获取传感器信息
        /// number堆位编号
        /// </summary>
        [HttpGet]
        public MobiResult GetList(string number)
        {
            var reList = sensorCore.GetSensorList(number);
            if (reList != null)
                return new MobiResult(1000, "成功", reList);
            else
                return new MobiResult(1011);
        }


    }
}