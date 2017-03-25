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
    public class GranaryCore:IGranaryCore
    {
        private static IGrainRepository<Granary> gRepository;

        public GranaryCore(IGrainRepository<Granary> _gRepository)
        {
            gRepository = _gRepository;
        }

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

        public bool UpdateGranary(Granary _entity)
        {
            var fieldArr = new string[] { "IsActive", "Location", "F_Number", "UserId" };
            var reInt = gRepository.Update(new List<Granary>() { _entity }, new string[] { "Number" }, fieldArr, "StampTime");
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
            var wCode=TypeParse.StrToInt(wareCode,0);
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
            var reList = gRepository.GetList(g=>g.IsActive==1&&idArr.Contains(g.WH_ID.Value)&&g.Type==0);
            return reList;

        }

    }
}
