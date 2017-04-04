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
        private static string endash = StaticClass.Endash;

        public GranaryCore(IGrainRepository<Granary> _gRepository, IGrainRepository<Collector> _cRepository, IGrainRepository<Temperature> _tRepository,
            IGrainRepository<Sensor> _sRepository, IGrainRepository<Receiver> _rRepository)
        {
            gRepository = _gRepository;
            cRepository = _cRepository;
            tRepository = _tRepository;
            sRepository = _sRepository;
            rRepository = _rRepository;
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

            var fieldArr = new string[] { "IsActive", "Location", "Code", "UserId", "WH_Number", "MaxiTemperature", "MinTemperature" };
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
            _addList.ForEach(a => { a.IsActive = 1;});

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

            var clist = cRepository.GetList(g => gNumbers.Contains(g.HeapNumber)) ?? new List<Collector>();
            var cIds = clist.Select(s => s.CPUId).ToList();
            var sList = sRepository.GetList(g => cIds.Contains(g.Collector)) ?? new List<Sensor>();
            var sIds = sList.Select(s => s.SensorId).ToList();
            var tempList = tRepository.GetList(g => g.Type == 0 && sIds.Contains(g.PId) && g.RealHeart == 0) ?? new List<Temperature>();

            #region 传感器绑定 temp
            var osensorList = sList.Select(s => new OSensor(tempList.FirstOrDefault(f => f.PId == s.SensorId))
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
                //RealTemp=null,
                SensorId = s.SensorId,
                Sequen = s.Sequen,
                UserId = s.UserId
            }).ToList() ?? new List<OSensor>();
            #endregion

            return reList.Select(s => new OHeap()
            {
                ID = s.ID,
                AverageHumidity = s.AverageHumidity,
                AverageTemperature = s.AverageTemperature,
                BadPoints = s.BadPoints,
                Code = s.Code,
                IsActive = s.IsActive,
                Location = s.Location,
                MaxiTemperature = s.MaxiTemperature,
                MinTemperature = s.MinTemperature,
                Number = s.Number,
                PID = s.PID,
                SensorList = osensorList.Where(w => clist.Where(wh => wh.HeapNumber == s.Number).Select(se => se.CPUId).Contains(w.Collector)).ToList(),
                Type = s.Type,
                UserId = s.UserId,
                WH_ID = s.WH_ID,
                WH_Number = s.WH_Number
            }).ToList();

        }

        /// <summary>
        /// 通过对位编号获取三温图
        /// Q1-1-1-1
        /// type=0 最近24小时、 1最近7天、 2最近1个月、3 最近1年
        /// </summary>
        /// <param name="number"></param>
        public List<Temperature> GetHeapTempsChart(string number, int type = 0)
        {
            var cList = cRepository.GetList(g => g.HeapNumber == number) ?? new List<Collector>();
            List<string> cpuIdList = cList.Select(s => s.CPUId).ToList();
            var rIdList = cList.Select(s => s.R_Code).Distinct().ToList();
            var rList = rRepository.GetList(g => rIdList.Contains(g.ID));
            cpuIdList.AddRange(rList.Select(s => s.CPUId).ToList());
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
            //0chuanganqi、1caijiqi、2shoujiqi nei、3shoujiqi wai
            //var temps = tRepository.GetList(g => cpuIdList.Contains(g.PId)&& string.Compare(g.StampTime,DateTime.Now.ToString())>=0);
            //0传感器、1采集器、2收集器（室内）、3收集器（室外）
            var temps = tRepository.GetList(g => cpuIdList.Contains(g.PId) && g.Type != 0 && g.UpdateTime > datenow);
            return temps=temps.OrderBy(o => o.UpdateTime).ToList();

        }


        /// <summary>
        /// 通过传感器编号获取其折线变化图 2017-03-12 14:48:03
        /// Q1-1-1-1
        /// type=0 最近24小时、 1最近7天、 2最近1个月、3 最近1年
        /// </summary>
        /// <param name="number"></param>
        public List<Temperature> GetSensorsChart(string number, int type = 0)
        {
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
            var temps = tRepository.GetList(g => number.Equals(g.PId) && g.Type == 0 && g.UpdateTime > datenow);
            return temps = temps.OrderBy(o => o.UpdateTime).ToList();

        }


    }
}
