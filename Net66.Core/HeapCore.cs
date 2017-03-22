using Net66.Core.Interface;
using Net66.Data.Interface;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net66.Entity.IO_Model;
using Net66.Data.Base;
using Net66.Comm;

namespace Net66.Core
{
   public class HeapCore: IHeapCore
    {
        private static IGrainRepository<Heap> hRepository;

        public HeapCore(IGrainRepository<Heap> _hRepository)
        {
            hRepository = _hRepository;
        }

        public bool AddHeap(List<Heap> _addList)
        {
            _addList.ForEach(a => a.IsActive = 1);
            var reInt = hRepository.Add(_addList);
            return reInt > 0;
        }

        public bool DeleteHeap(List<Heap> _delList)
        {
            var reInt = hRepository.Delete(_delList, new string[] { "Number" });
            return reInt > 0;
        }

        public List<Heap> GetPageLists(ISearch _search, List<string> _params)
        {
            #region //条件查询
            int rows = 0;
            int pageIndex = _search.PageIndex <= 0 ? 1 : _search.PageIndex;
            int pageSize = _search.PageCount;
            //collection["query"].GetString().Split(',');
            string userId = Utils.GetValue(_params, "UserId^");
            string granaryCode = Utils.GetValue(_params, "gCode^");
            if (pageSize <= 0 || string.IsNullOrEmpty(userId)||string.IsNullOrEmpty(granaryCode))
                return null;
            var where = EfUtils.True<Heap>();
            where = where.And(w => w.IsActive == 1&&w.G_Number==granaryCode);
            #endregion
            //获取粮仓信息
            var reList = hRepository.GetPageLists(where, p => p.ID.ToString(), false, pageIndex, pageSize, ref rows);
            return reList;

        }

        public bool IsExist(Heap _entity)
        {
            var ifno = hRepository.Get(g => g.Number == _entity.Number && g.G_Number == _entity.G_Number);
            if (ifno == null)
                return false;
            return true;
        }

        public bool UpdateHeap(Heap _entity)
        {
            var fieldArr = new string[] { "IsActive", "Location", "G_Number", "UserId" };
            var reInt = hRepository.Update(new List<Heap>() { _entity }, new string[] { "Number" }, fieldArr, "StampTime");
            return reInt > 0;
        }
    }
}
