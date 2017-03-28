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
        private static string endash = StaticClass.Endash;

        public GranaryCore(IGrainRepository<Granary> _gRepository, IGrainRepository<Collector> _cRepository, IGrainRepository<Temperature> _tRepository)
        {
            gRepository = _gRepository;
            cRepository = _cRepository;
            tRepository = _tRepository;
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

        public bool AddList(List<Granary> _addList, int _type)
        {
            //_type 0duiwei、1louceng、2aojian
            _addList.ForEach(a => { a.IsActive = 1; a.Type = _type; });

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
            var fieldArr = new string[] { "IsActive", "Location", "F_Number", "UserId" };
            var reInt = gRepository.Update(new List<Granary>() { _entity }, new string[] { "Number" }, fieldArr, "StampTime");
            return reInt > 0;
        }

        public List<Granary> GetList(List<string> _params)
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

            if (reList == null)
                return null;
            var gNumbers = reList.Select(s => s.Number).ToList();

            var clist=cRepository.GetList(g => gNumbers.Contains(g.HeapNumber));

            return reList;

        }

    }
}
