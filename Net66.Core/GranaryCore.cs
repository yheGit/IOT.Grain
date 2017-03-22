using Net66.Core.Interface;
using Net66.Data.Interface;
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
            var ifno = gRepository.Get(g => g.Number == _entity.Number && g.F_Number == _entity.F_Number);
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
    }
}
