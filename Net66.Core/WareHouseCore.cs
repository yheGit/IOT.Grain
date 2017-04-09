﻿using Net66.Comm;
using Net66.Core.Interface;
using Net66.Data.Base;
using Net66.Data.Interface;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core
{
    public class WareHouseCore : IWareHouseCore
    {
        private static IGrainRepository<WareHouse> Repository;
        private static IGrainRepository<Temperature> tRepository;
        private static IGrainRepository<Humidity> hRepository;
        private static IGrainRepository<Granary> gRepository;
        private static IGrainRepository<Collector> cRepository;
        private static IGrainRepository<Sensor> sRepository;
        private static string endash = StaticClass.Endash;

        //粮仓类型(1楼房L、2平方P、3浅圆仓Q、4筒仓T)
        public WareHouseCore(IGrainRepository<WareHouse> _Repository, IGrainRepository<Granary> _gRepository
            , IGrainRepository<Temperature> _tRepository, IGrainRepository<Collector> _cRepository
            , IGrainRepository<Humidity> _hRepository, IGrainRepository<Sensor> _sRepository)
        {
            Repository = _Repository;
            hRepository = _hRepository;
            cRepository = _cRepository;
            gRepository = _gRepository;
            tRepository = _tRepository;
            sRepository = _sRepository;
        }

        /// <summary>
        /// 查寻粮仓信息 2017-03-13 05:35:36
        /// </summary>
        public List<OWareHouse> GetPageLists(ISearch _search, List<string> _params)
        {
            #region //条件查询
            int rows = 0;
            int pageIndex = _search.PageIndex <= 0 ? 1 : _search.PageIndex;
            int pageSize = _search.PageCount;
            //collection["query"].GetString().Split(',');
            string userId = Utils.GetValue(_params, "UserId^");
            if (pageSize <= 0 || string.IsNullOrEmpty(userId))
                return null;
            var where = EfUtils.True<WareHouse>();
            where = where.And(w => w.IsActive == 1);

            #region 时间条件
            string startdate = _search.StartDate ?? string.Empty;
            string enddate = _search.EndDate ?? string.Empty;
            DateTime datenow = DateTime.Now.Date;
            DateTime startDate = datenow;
            DateTime endDate = datenow;
            if (startdate.Length == 0 && enddate.Length == 0)
            {//默认加载当前月
                //startDate = datenow.Date.AddMonths(-1);
                //endDate = datenow.Date.AddDays(1);
            }
            else
            if (enddate.Length == 0)
            {
                startDate = DateTime.Parse(startdate);
                endDate = endDate.AddDays(1);
            }
            else if (startdate.Length == 0)
            {
                startDate = DateTime.Parse(enddate);
                endDate = startDate.AddDays(1);
            }
            else
            {
                startDate = DateTime.Parse(startdate);
                endDate = DateTime.Parse(enddate).AddDays(1);
            }
            if (startDate != datenow && endDate != datenow)
                where = where.And(p => (p.StampTime >= startDate && p.StampTime < endDate));
            #endregion

            #region 其他条件

            //int type = TypeParse.StrToInt(_search.Dic["Type"], -1);
            ////0未报销，1已报销 的消费记录
            //if (type != -1)
            //{
            //    where = where.And(p => p.IsCost == type);
            //}
            #endregion

            #endregion
            //huoquliangcangxinxi
            var reList = Repository.GetPageLists(where, p => p.StampTime.ToString(), false, pageIndex, pageSize, ref rows);
            //louceng/aojian
            var reIdList = reList.Select(s => s.Number).ToList();
            var fg_List = gRepository.GetList(g => reIdList.Contains(g.WH_Number) && (g.Type == 1 || g.Type == 2));//WH_Number
            //louceng
            var floorList = fg_List.Where(w => w.Type == 1).ToList();
            //aojianxinxi
            var granaryList = fg_List.Where(w => w.Type == 2).ToList();
            try
            {
                var ofList = floorList.OrderBy(o => o.Code).Select(s => new OFloor()
                {
                    ID = s.ID,
                    IsActive = s.IsActive,
                    Location = s.Location,
                    Number = s.Number,
                    UserId = s.UserId,
                    WH_Number = s.WH_Number,
                    //GranaryList = granaryList.Where(w => SqlFunctions.PatIndex(s.Number + "__", w.Number) > 0).ToList()
                    GranaryList = granaryList.Where(w => w.Number.Contains(s.Number)).OrderBy(o => o.Code).ToList()
                }).ToList();

                return reList.Select(s => new OWareHouse()
                {
                    ID = s.ID,
                    IsActive = s.IsActive,
                    Location = s.Location,
                    AverageTemperature = s.AverageTemperature,
                    Maximumemperature = s.Maximumemperature,
                    MinimumTemperature = s.MinimumTemperature,
                    InSideTemperature = s.InSideTemperature,
                    OutSideTemperature = s.OutSideTemperature,
                    Name = s.Name,
                    Number = s.Number,
                    StampTime = s.StampTime,
                    Type = s.Type,
                    UserId = s.UserId,
                    BadPoints = s.BadPoints,
                    Floors = ofList.Where(w => w.WH_Number == s.Number).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 添加粮仓信息 2017-03-13 05:36:15
        /// </summary>
        /// <param name="_entity"></param>
        /// <returns></returns>
        public bool AddWareHouse(IWareHouse _entity)
        {
            DateTime datenow = Utils.GetServerDateTime();
            WareHouse model = new WareHouse()
            {
                IsActive = 1,
                Location = _entity.Location,
                Name = _entity.Name,
                Number = _entity.Number,
                Type = _entity.Type,//1(L)楼房仓、2(P)平顶仓、3(Q)浅圆筒仓、4（T）筒仓
                UserId = _entity.UserId,
                StampTime = datenow,
                AverageTemperature = 0,
                Maximumemperature = 0,
                MinimumTemperature = 0,
                Width = _entity.Width,
                depth = _entity.depth,
                Height = _entity.Height
            };


            var reInt = Repository.Add(model, f => f.Number == model.Number);
            if (reInt > 0 && model.Type != 1)
            {
                var addList = new List<Granary>() {
                  new Granary() {
                      Code="1",Number=Utils.StrSequenConcat(model.Number,endash,"1"),IsActive=1,Location=null,
                      Type=1,WH_ID=model.ID,WH_Number=model.Number,BadPoints=0
                  },
                  new Granary() {
                       Code="1",Number=Utils.StrSequenConcat(model.Number,endash,"1",endash,"1"),IsActive=1,Location=null,
                      Type=2,WH_ID=model.ID,WH_Number=model.Number,BadPoints=0
                  },
                  new Granary() {
                       Code="1",Number=Utils.StrSequenConcat(model.Number,endash,"1",endash,"1",endash,"1"),IsActive=1,Location=null,
                      Type=0,WH_ID=model.ID,WH_Number=model.Number,BadPoints=0
                  }
            };
                gRepository.Add(addList);
            }

            return reInt > 0;
        }

        /// <summary>
        /// 修改粮仓 2017-03-14 21:40:06
        /// </summary>
        public bool UpdateWareHouse(WareHouse _entity)
        {
            if (string.IsNullOrEmpty(_entity.Number)
                || _entity.IsActive == null || _entity.Type == null)
                return false;
            //_entity.StampTime = Utils.GetServerDateTime();
            //var model = Repository.Get(g => g.ID == _entity.ID);
            var fieldArr = new string[] { "IsActive", "Location", "Name", "Type", "UserId" };
            var reInt = Repository.Update(new List<WareHouse>() { _entity }, new string[] { "Number" }, fieldArr, "StampTime");
            return reInt > 0;
        }

        /// <summary>
        /// 批量删除粮仓 2017-03-14 21:39:06
        /// </summary>
        public bool DeleteWareHouse(List<WareHouse> _delList)
        {
            var delList = new List<WareHouse>();
            foreach (var info in _delList)
            {
                if (string.IsNullOrEmpty(info.Number))
                    continue;
                delList.Add(info);
            }
            var reInt = Repository.Delete(delList, new string[] { "Number" });
            return reInt > 0;
        }

        /// <summary>
        /// 验证粮仓是否存在 2017-03-10 21:45:10
        /// </summary>
        public bool HasExist(string _code)
        {
            var ifno = Repository.Get(g => g.Number == _code);
            if (ifno == null)
                return false;
            return true;

        }

        /// <summary>
        /// IOT设备获取粮仓及结构信息 2017-03-14 01:45:58
        /// </summary>
        public Dictionary<string, object[]> GetBBDList()
        {
            var rList = Repository.GetList(p => p.IsActive == 1);
            if (rList == null)
                return null;
            var rIds = rList.Select(s => s.Number).ToList();
            var gList = gRepository.GetList(g => rIds.Contains(g.WH_Number));
            Dictionary<string, object[]> bbdDic = new Dictionary<string, object[]>();
            foreach (var r in rList)
            {
                char whType = 'L';
                var fCount = 1;
                var gCount = 1;
                if (r.Type == 1)
                {
                    fCount = gList.Count(w => w.WH_Number == r.Number && w.Type == 1);
                    gCount = gList.Count(w => w.WH_Number == r.Number && w.Type == 2);
                }
                else if (r.Type == 2)
                    whType = 'T';
                else if (r.Type == 3)
                    whType = 'Q';

                bbdDic.Add(r.Number, new object[] { whType, fCount, gCount });//1loufang、2pingfang、3jiantong
            }

            return bbdDic;

        }

        /// <summary>
        /// 手机端获取所有的粮仓温度 2017-03-18 12:01:52
        /// </summary>
        public List<OGrainsReport> GetGrainsTemp(string userId = "0")
        {
            var rList = Repository.GetList(p => p.IsActive == 1) ?? new List<WareHouse>();
            var temps = tRepository.GetList(g => g.RealHeart == 0) ?? new List<Temperature>();
            var humtys = hRepository.GetList(g => g.RealHeart == 0) ?? new List<Humidity>();
            var badlist = cRepository.GetList(g => g.IsActive == 1 && g.BadPoints > 0); //坏点数

            return rList.Select(s => new OGrainsReport(temps.Where(w => w.WH_Number == s.Number).ToList()
                , humtys.Where(w => w.WH_Number == s.Number).ToList()
                , badlist.Where(w => w.HeapNumber.IndexOf(s.Number) > -1).ToList())
            {
                Number = s.Number,
                Type = s.Type ?? 0,
                UserId = s.UserId
            }).ToList();
        }

        /// <summary>
        /// 通过粮仓编号获取廒间温度报表 2017-4-8 17:35:48
        /// 粮仓编号
        /// </summary>
        public List<OGranaryReport> getGranaryTemp(string number)
        {
            var granaryList = gRepository.GetList(g => g.WH_Number == number && g.Type == 2 && g.IsActive == 1);
            var temps = tRepository.GetList(g => g.WH_Number == number && g.RealHeart == 0) ?? new List<Temperature>();
            var outtemp = temps.FirstOrDefault(f => f.G_Number == "0" && f.Type == 2).Temp;
            var humtys = hRepository.GetList(g => g.WH_Number == number && g.RealHeart == 0) ?? new List<Humidity>();
            var outhumty = humtys.FirstOrDefault(w => w.Type == 1 && w.G_Number == "0").Humility;
            var badlist = cRepository.GetList(g => g.IsActive == 1 && g.BadPoints > 0 && g.HeapNumber.IndexOf(number) > -1) ?? new List<Collector>();
            return granaryList.Select(s => new OGranaryReport(temps.Where(w => w.G_Number == s.Number).ToList()
                , humtys.Where(w => w.G_Number == s.Number).ToList()
                , badlist.Where(w => w.HeapNumber.IndexOf(s.Number) > -1).ToList())
            {
                Number = s.Number,
                OutSideTemperature = outtemp,
                OutSideHumidity = outhumty,
                //BadPoints = badlist.Where(w => w.HeapNumber == s.Number).Sum(u => u.BadPoints),
                UserId = s.UserId.ToString()
            }).ToList();
        }

        /// <summary>
        /// 通过廒间编号获取堆位温度报表 2017-03-24 22:04:15
        /// 廒间编号
        /// </summary>
        public List<OHeapReport> getHeapsTemp(string number)
        {
            var heapList = gRepository.GetList(g => g.Number.Contains(number) && g.Type == 0 && g.IsActive == 1);
            var temps = tRepository.GetList(g => number.Contains(g.WH_Number)&&( g.G_Number == number||g.G_Number=="0" )&& g.RealHeart == 0) ?? new List<Temperature>();
            var outtemp = temps.FirstOrDefault(f => f.G_Number == "0" && f.Type == 2).Temp;
            //var intemp = temps.Average(a => a.Temp);//堆位的仓内温度取，所在廒间的所有温度的平均值
            var ck_fenji = cRepository.GetList(g => g.IsActive == 1 && g.HeapNumber.IndexOf(number) > -1) ?? new List<Collector>();//廒间里所有堆位的采集器
            var badlist = ck_fenji.Where(g => g.BadPoints > 0) ?? new List<Collector>();

            var ck_fjIdList = ck_fenji.Select(s => s.CPUId).ToList();
            var chuanganqi = sRepository.GetList(g => ck_fjIdList.Contains(g.Collector));
            return heapList.Select(s => new OHeapReport(ck_fenji.Where(w=>w.HeapNumber==s.Number).Select(e=>e.CPUId).ToList(), chuanganqi, temps)
            {
                OutSideTemperature=outtemp,
                Number = s.Number,
                BadPoints = badlist.Where(w => w.HeapNumber == s.Number).Sum(u => u.BadPoints),
                UserId = s.UserId.ToString()
            }).ToList();
        }




    }
}
