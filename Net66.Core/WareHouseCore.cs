using Net66.Comm;
using Net66.Core.Interface;
using Net66.Data.Base;
using Net66.Data.Interface;
using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core
{
    public class WareHouseCore : IWareHouseCore
    {
        private static IGrainRepository<WareHouse> Repository;
        private static IGrainRepository<Floor> fRepository;
        private static IGrainRepository<Granary> gRepository;

        public WareHouseCore(IGrainRepository<WareHouse> _Repository, IGrainRepository<Floor> _fRepository, IGrainRepository<Granary> _gRepository)
        {
            Repository = _Repository;
            fRepository = _fRepository;
            gRepository = _gRepository;
        }

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
            //获取粮仓信息
            var reList = Repository.GetPageLists(where, p => p.StampTime.ToString(), false, pageIndex, pageSize, ref rows);
            //获取楼层信息
            var reIdList = reList.Select(s => s.Number).ToList();
            var floorList = fRepository.GetList(g => reIdList.Contains(g.WH_Number));//WH_Number
            //廒间信息
            var fIdList = floorList.Select(s => s.Number).ToList();
            var granaryList = gRepository.GetList(g => fIdList.Contains(g.F_Number));

            var ofList = floorList.Select(s => new OFloor()
            {
                ID = s.ID,
                IsActive = s.IsActive,
                Location = s.Location,
                Number = s.Number,
                UserId = s.UserId,
                WH_Number = s.WH_Number,
                GranaryList = granaryList.Where(w => w.F_Number == s.Number).ToList()
            }).ToList();

            return reList.Select(s => new OWareHouse()
            {
                ID = s.ID,
                AverageTemperature = s.AverageTemperature,
                IsActive = s.IsActive,
                Location = s.Location,
                Maximumemperature = s.Maximumemperature,
                MinimumTemperature = s.MinimumTemperature,
                Name = s.Name,
                Number = s.Number,
                StampTime = s.StampTime,
                Type = s.Type,
                UserId = s.UserId,
                Floors = ofList.Where(w => w.WH_Number == s.Number).ToList()
            }).ToList();
        }

        public bool AddWareHouse(IWareHouse _entity)
        {
            DateTime datenow = Utils.GetServerDateTime();
            WareHouse model = new WareHouse()
            {
                IsActive = 1,
                Location = _entity.Location,
                Name = _entity.Name,
                Number = _entity.Number,
                Type = _entity.Type,
                UserId = _entity.UserId,
                StampTime = datenow,
                AverageTemperature = 0,
                Maximumemperature = 0,
                MinimumTemperature = 0
            };
            var reInt = Repository.Add(model,f=>f.Number==model.Number);
            return reInt > 0;
        }

        public bool UpdateWareHouse(WareHouse _entity)
        {
            //var model = Repository.Get(g => g.ID == _entity.ID);
            var fieldArr = new string[] { "IsActive", "Location", "Name", "Type", "UserId" };
            var reInt = Repository.Update(new List<WareHouse>() { _entity }, new string[] { "Number" }, fieldArr, "StampTime");
            return reInt > 0;
        }

        public bool DeleteWareHouse(List<WareHouse> _delList)
        {
            var reInt = Repository.Delete(_delList, new string[] { "Number" });
            return reInt > 0;
        }
       

        public bool HasExist(string _code)
        {
            var ifno = Repository.Get(g => g.Number == _code);
            if (ifno == null)
                return false;
            return true;

        }

    }
}
