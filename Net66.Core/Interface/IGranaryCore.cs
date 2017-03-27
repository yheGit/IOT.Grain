using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.Interface
{
    public interface IGranaryCore
    {
        bool AddGranary(List<Granary> _addList);

        bool UpdateGranary(Granary _entity);

        bool DeleteGranary(List<Granary> _delList);

        bool IsExist(Granary _entity);

        List<Granary> GetHeapList(List<int> idArr);

        #region new
        string IsExist(string _cCode, string _fCode);

        bool AddList(List<Granary> _addList, int _type);

        bool Update(Granary _entity);

        List<Granary> GetList(List<string> _params);


        #endregion


    }
}
