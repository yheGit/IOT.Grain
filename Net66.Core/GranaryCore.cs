using Net66.Comm;
using Net66.Core.Interface;
using Net66.Data.Base;
using Net66.Data.Interface;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core
{
    public class GranaryCore : IGranaryCore
    {
        private static IGrainRepository<Granary> gRepository;
        private static IGrainRepository<Collector> cRepository;
        private static IGrainRepository<Temperature> tRepository;
        private static IGrainRepository<Receiver> rRepository;
        private static IGrainRepository<Sensor> sRepository;
        private static IGrainRepository<HeapLine> hlRepository;
        private static string endash = StaticClass.Endash;

        public GranaryCore(IGrainRepository<Granary> _gRepository, IGrainRepository<Collector> _cRepository, IGrainRepository<Temperature> _tRepository,
            IGrainRepository<Sensor> _sRepository, IGrainRepository<Receiver> _rRepository, IGrainRepository<HeapLine> _hlRepository)
        {
            gRepository = _gRepository;
            cRepository = _cRepository;
            tRepository = _tRepository;
            sRepository = _sRepository;
            rRepository = _rRepository;
            hlRepository = _hlRepository;
        }

        #region old
        public bool AddGranary(List<Granary> _addList)
        {
            _addList.ForEach(a => a.IsActive = 1);

            var reInt = gRepository.Add(_addList);
            return reInt > 0;
        }

        public bool DeleteGranary(List<Granary> _delList)
        {
            var reInt = gRepository.Delete(_delList, new string[] { "Number" });
            return reInt > 0;
        }

        public bool IsExist(Granary _entity)
        {
            var ifno = gRepository.Get(g => g.Number == _entity.Number && g.Code == _entity.Code);
            if (ifno == null)
                return false;
            return true;
        }

        public bool IsExist2(List<string> _params)
        {
            //TypeParse.StrToInt(Utils.GetValue(_params, "number^"), 0)
            string _number = Utils.GetValue(_params, "number^");
            int _type = TypeParse.StrToInt(Utils.GetValue(_params, "type^"), 0);
            var ifno = gRepository.Get(g => g.Number == _number && g.Type == _type);
            if (ifno == null)
                return false;
            return true;
        }


        public bool UpdateGranary(Granary _entity)
        {
            var fieldArr = new string[] { "IsActive", "Location", "Code", "UserId", "WH_Number", "MaxiTemperature", "MinTemperature" };
            var reInt = gRepository.Update(new List<Granary>() { _entity }, new string[] { "Number" }, fieldArr, "StampTime");
            return reInt > 0;
        }

        /// <summary>
        /// 批量更新楼层、鏖间、堆位等信息 2017-03-14 22:35:47
        /// </summary>
        /// <param name="_list"></param>
        /// <returns></returns>
        public bool UpdateList(List<Granary> _list)
        {
            List<Granary> addList = new List<Granary>();
            foreach (var model in _list)
            {
                if (model.Type == null || string.IsNullOrEmpty(model.Number))
                    continue;
                model.AverageHumidity = model.AverageHumidity ?? 0;
                model.AverageTemperature = model.AverageTemperature ?? 0;
                model.BadPoints = model.BadPoints ?? 0;
                model.IsActive = model.IsActive ?? 1;
                model.MaxiTemperature = model.MaxiTemperature ?? 0;
                model.MinTemperature = model.MinTemperature ?? 0;
                model.StampTime = Utils.GetServerTime();
                addList.Add(model);
            }

            var fieldArr = new string[] { "IsActive", "Location", "Code", "UserId", "WH_Number", "MaxiTemperature", "MinTemperature", "Sort" };
            var reInt = gRepository.AddUpdate(addList, new string[] { "Number" }, fieldArr, "StampTime");
            return reInt > 0;
        }


        public List<Granary> GetHeapList(ISearch _search, List<string> _params)
        {
            #region //条件查询
            int rows = 0;
            int pageIndex = _search.PageIndex <= 0 ? 1 : _search.PageIndex;
            int pageSize = _search.PageCount;
            //collection["query"].GetString().Split(',');
            string userId = Utils.GetValue(_params, "UserId^");
            string wareCode = Utils.GetValue(_params, "wCode^");
            if (pageSize <= 0 || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(wareCode))
                return null;
            var where = EfUtils.True<Granary>();
            var wCode = TypeParse.StrToInt(wareCode, 0);
            where = where.And(w => w.IsActive == 1 && w.WH_ID == wCode);
            #endregion
            //获取粮仓堆位信息
            var reList = gRepository.GetPageLists(where, p => p.ID.ToString(), false, pageIndex, pageSize, ref rows);
            return reList;

        }

        public List<Granary> GetHeapList(List<int> idArr)
        {
            if (idArr == null)
                return null;
            //获取粮仓堆位信息
            var reList = gRepository.GetList(g => g.IsActive == 1 && idArr.Contains(g.WH_ID.Value) && g.Type == 0);
            return reList;

        }

        #endregion

        /// <summary>
        /// 验证（louceng、aojian、duiwei）是否存在 by yhw 
        /// </summary>
        /// <param name="_cCode">当前短编码</param>
        /// <param name="_fCode">父级短编码</param>
        /// <returns></returns>
        public string IsExist(string _cCode, string _fCode)
        {
            var number = Utils.StrSequenConcat(_fCode, endash, _cCode);
            var ifno = gRepository.Get(g => g.Number == number);
            if (ifno == null)
                return number;
            return "";
        }

        /// <summary>
        /// 0添加堆位、1添加楼层、2添加廒间
        /// </summary>
        /// <param name="_addList"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        public bool AddList(List<Granary> _addList, int _type)
        {
            //_type 0duiwei、1louceng、2aojian
            _addList.ForEach(a => { a.IsActive = 1; a.Type = _type; });

            var reInt = gRepository.Add(_addList);
            return reInt > 0;
        }

        /// <summary>
        /// 0添加堆位、1添加楼层、2添加廒间
        /// </summary>
        public bool AddList2(List<Granary> _addList)
        {
            //_type 0duiwei、1louceng、2aojian
            _addList.ForEach(a => { a.IsActive = 1; });

            var reInt = gRepository.Add(_addList);
            return reInt > 0;
        }

        public bool Delete(List<Granary> _delList)
        {
            var reInt = gRepository.Delete(_delList, new string[] { "Number" });
            return reInt > 0;
        }

        public bool Update(Granary _entity)
        {
            var fieldArr = new string[] { "IsActive", "Location", "Code", "UserId", "WH_Number", "MaxiTemperature", "MinTemperature" };
            var reInt = gRepository.Update(new List<Granary>() { _entity }, new string[] { "Number" }, fieldArr, "StampTime");
            return reInt > 0;
        }

        /// <summary>
        /// 获取堆位信息（包含温度信息）
        /// </summary>
        public List<OHeap> GetList(List<string> _params)
        {
            #region //条件查询
            int rows = 0;
            int pIndex = TypeParse.StrToInt(Utils.GetValue(_params, "PageIndex^"), 0);
            int pageIndex = pIndex <= 0 ? 1 : pIndex;
            int pageSize = TypeParse.StrToInt(Utils.GetValue(_params, "PageCount^"), 0);
            string userId = Utils.GetValue(_params, "UserId^");
            string wareCode = Utils.GetValue(_params, "wCode^");//liangcang
            string granaryCode = Utils.GetValue(_params, "gCode^");//aojian
            if (pageSize <= 0 || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(wareCode))
                return null;
            var where = EfUtils.True<Granary>();
            //var wCode = TypeParse.StrToInt(wareCode, 0);
            where = where.And(w => w.IsActive == 1 && w.WH_Number == wareCode && w.Type == 0);
            if (!string.IsNullOrEmpty(granaryCode))
                where = where.And(w => w.Number.Contains(granaryCode));//loufangleixing
            #endregion
            //huoquliangcangduiweidexinxi
            var reList = gRepository.GetPageLists(where, p => p.ID.ToString(), true, pageIndex, pageSize, ref rows);
            if (reList == null || reList.Count < 0)
                return null;
            var gNumbers = reList.Select(s => s.Number).ToList();//duiweibianhao

            var clist = cRepository.GetList(g => g.IsActive == 1 && gNumbers.Contains(g.HeapNumber)) ?? new List<Collector>();
            var cIds = clist.Select(s => s.CPUId).ToList();
            //huoqu tongcang chaunganxiandegeshu
            var hllist = hlRepository.GetList(g => gNumbers.Contains(g.HeapNumber)) ?? new List<HeapLine>();

            var sList = sRepository.GetList(g => cIds.Contains(g.Collector)) ?? new List<Sensor>();

            #region //坏点数;
            var badlist = (from c in clist
                           join s in sList on c.CPUId equals s.Collector
                           select new { heapnumber = c.HeapNumber, collector = c.CPUId, badcount = s.IsBad });
            #endregion //坏点数    

            var sIds = sList.Select(s => s.GuidID).ToList();
            var tempList = tRepository.GetList(g => g.Type == 0 && sIds.Contains(g.PId) && g.RealHeart == 0) ?? new List<Temperature>();

            #region 传感器绑定 temp
            var osensorList = sList.Select(s => new OSensor(tempList.FirstOrDefault(f => f.PId == s.GuidID))
            {
                Collector = s.Collector,
                CRC = s.CRC,
                Direction_X = s.Direction_X,
                Direction_Y = s.Direction_Y,
                Direction_Z = s.Direction_Z,
                GuidID = s.GuidID,
                ID = s.ID,
                IsActive = s.IsActive,
                Label = s.Label,
                MaxTemp = s.MaxTemp,
                MinTemp = s.MinTemp,
                IsBad = s.IsBad,
                SensorId = s.SensorId,
                Sequen = s.Sequen,
                UserId = s.UserId,
            }).ToList() ?? new List<OSensor>();
            #endregion

            return reList.Select(s => new OHeap()
            {
                #region 格式化数据
                ID = s.ID,
                AverageHumidity = s.AverageHumidity,
                AverageTemperature = s.AverageTemperature,
                MaxiTemperature = s.MaxiTemperature,
                MinTemperature = s.MinTemperature,
                BadPoints = badlist.Where(w => w.heapnumber == s.Number && w.badcount > 0).Count(),//  s.BadPoints,
                OutSideTemperature = 0,
                InSideTemperature = s.AverageTemperature,
                Code = s.Code,
                Name = s.Name,
                IsActive = s.IsActive,
                Location = s.Location,
                Number = s.Number,
                PID = s.PID,
                SensorList = osensorList.Where(w => clist.Where(wh => wh.HeapNumber == s.Number).Select(se => se.GuidID).Contains(w.Sequen)).ToList(),
                Type = s.Type,
                UserId = s.UserId,
                WH_ID = s.WH_ID,
                WH_Number = s.WH_Number,
                LineCount = hllist.Where(w => w.HeapNumber == s.Number).OrderBy(o => o.Sort).Select(e => e.Counts.Value).ToList(),
                LastTime = tempList.OrderByDescending(d => d.UpdateTime).FirstOrDefault() == null ? "" : tempList.OrderByDescending(d => d.UpdateTime).FirstOrDefault().StampTime,
                Sort = s.Sort
                #endregion 

            }).ToList();

        }

        /// <summary>
        /// 通过对位编号获取三温图
        /// Q1-1-1-1
        /// type=0 最近24小时、 1最近7天、 2最近1个月、3 最近1年
        /// </summary>
        public List<Temperature> GetHeapTempsChart(string number, int type = 0)
        {
            var cList = cRepository.GetList(g => g.HeapNumber == number) ?? new List<Collector>();
            List<string> cpuIdList = cList.Select(s => s.CPUId).ToList();
            var sList = sRepository.GetList(g => cpuIdList.Contains(g.Collector)) ?? new List<Sensor>();
            var sidList = sList.Select(s => s.GuidID);
            DateTime datenow = DateTime.Now;

            switch (type)
            {
                case 1:
                    datenow = datenow.AddDays(-7); break;//zuijin 1zhou
                case 2:
                    datenow = datenow.AddMonths(-1); break;//zuijin 1yue
                case 3:
                    datenow = datenow.AddYears(-1); break;//zuijin 1nian
                default:
                    datenow = datenow.AddHours(-24); break;//zuijin 1tian
            }

            //0传感器、1采集器(粮堆平均温度)、2收集器（室内）、3收集器（室外）
            var temps = tRepository.GetList(g => ((sidList.Contains(g.PId) && g.Type == 0) || ((g.Type == 2 || g.Type == 3) && number.Contains(g.WH_Number))) && g.UpdateTime >= datenow);

            return temps = temps.OrderBy(o => o.UpdateTime).ToList();

        }

        /// <summary>
        /// 2017-06-27 15:46:10
        /// </summary>
        public List<Temperature> GetHeapTempsChart2(string number, int type = 0)
        {
            DateTime datenow = DateTime.Now;
            string TimeFlag = "DAY";//MONTH,YEAR
            var where = EfUtils.True<Temperature>();
            //var where2 = EfUtils.True<Temperature>();
            switch (type)
            {
                case 1:
                    datenow = datenow.AddDays(-7);
                    break;//zuijin 1zhou
                case 2:
                    datenow = datenow.AddMonths(-1);
                    TimeFlag = "MONTH"; break;//zuijin 1yue
                case 3:
                    datenow = datenow.AddYears(-1);
                    TimeFlag = "YEAR"; break;//zuijin 1nian
                default:
                    datenow = datenow.AddHours(-24);
                    TimeFlag = "DAY"; break;//zuijin 1tian
            }

            var wh_number = number.Substring(0, number.IndexOf('-'));
            var g_number = number.Substring(0, number.LastIndexOf('-'));

            Utils.PrintLog("wh_number" + wh_number + ",g_number" + g_number, "ILog");
            //0传感器、1采集器(粮堆平均温度)、2收集器（室内）、3收集器（室外）
            var temps = tRepository.GetList(g => (
               (g.H_Number.Equals(number) && g.Type == 1)
            || (g_number.Equals(g.G_Number) && g.Type == 2)
            || (wh_number.Equals(g.WH_Number) && g.Type == 3)
            ) && g.RealHeart == 1 && g.UpdateTime >= datenow && g.TimeFlag == TimeFlag);

            return temps = temps.OrderBy(o => o.UpdateTime).Select(s => new Temperature()
            {
                G_Number = s.G_Number,
                H_Number = s.H_Number,
                ID = s.ID,
                PId = s.PId,
                RealHeart = s.RealHeart,
                StampTime = s.StampTime,
                Temp = s.Temp,
                UpdateTime = s.UpdateTime,
                WH_Number = s.WH_Number,
                Type = s.Type == 1 ? 0 : s.Type,
                TimeFlag = s.TimeFlag
            }).ToList();

        }


        /// <summary>
        /// 通过传感器编号获取其折线变化图 2017-03-12 14:48:03
        /// Q1-1-1-1
        /// type=0 最近24小时、 1最近7天、 2最近1个月、3 最近1年
        /// </summary>
        public List<Temperature> GetSensorsChart(string number, int type = 0)
        {
            //var sInfo = sRepository.Get(g => g.SensorId == number);
            var sInfo = sRepository.Get(g => g.GuidID == number);
            if (sInfo == null)
                return null;
            DateTime datenow = DateTime.Now;
            switch (type)
            {
                case 1:
                    datenow = datenow.AddDays(-7); break;//zuijin 1zhou
                case 2:
                    datenow = datenow.AddMonths(-1); break;//zuijin 1yue
                case 3:
                    datenow = datenow.AddYears(-1); break;//zuijin 1nian
                default:
                    datenow = datenow.AddHours(-24); break;//zuijin 1tian
            }
            var temps = tRepository.GetList(g => sInfo.GuidID.Equals(g.PId) && g.Type == 0 && g.UpdateTime > datenow);

            return temps = temps.OrderBy(o => o.UpdateTime).ToList();

        }

        /// <summary>
        /// 通过传感器编号获取其折线变化图 2017-06-27 15:46:00
        /// Q1-1-1-1
        /// type=0 最近24小时、 1最近7天、 2最近1个月、3 最近1年
        /// </summary>
        public List<Temperature> GetSensorsChart2(string number, int type = 0)
        {
            var guidid = number;
            var sInfo = sRepository.Get(g => g.GuidID == guidid);
            if (sInfo == null)
                return null;
            var h_number = cRepository.Get(g => g.CPUId == sInfo.Collector).HeapNumber;

            DateTime datenow = DateTime.Now;
            string TimeFlag = "DAY";//MONTH,YEAR
            switch (type)
            {
                case 1:
                    datenow = datenow.AddDays(-7);
                    break;//zuijin 1zhou
                case 2:
                    datenow = datenow.AddMonths(-1);
                    TimeFlag = "MONTH"; break;//zuijin 1yue
                case 3:
                    datenow = datenow.AddYears(-1);
                    TimeFlag = "YEAR"; break;//zuijin 1nian
                default:
                    datenow = datenow.AddHours(-24);
                    TimeFlag = "DAY"; break;//zuijin 1tian
            }

            var temps = tRepository.GetList(g => sInfo.GuidID.Equals(g.PId) && g.Type == 0 && g.UpdateTime > datenow && g.TimeFlag == TimeFlag);//WH_Number---sensorGuid

            return temps = temps.OrderBy(o => o.UpdateTime).Select(s => new Temperature()
            {
                G_Number = s.G_Number,
                H_Number = s.H_Number,
                ID = s.ID,
                PId = s.PId,
                RealHeart = s.RealHeart,
                StampTime = s.StampTime,
                Temp = s.Temp,
                UpdateTime = s.UpdateTime,
                WH_Number = s.WH_Number,
                Type = s.Type == 1 ? 0 : s.Type
            }).ToList();

        }


    }
}
