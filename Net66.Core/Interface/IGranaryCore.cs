using Net66.Entity.IO_Model;
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

        bool UpdateList(List<Granary> _list);

        bool DeleteGranary(List<Granary> _delList);

        bool IsExist(Granary _entity);

        List<Granary> GetHeapList(List<int> idArr);

        #region new
        string IsExist(string _cCode, string _fCode);

        bool AddList(List<Granary> _addList, int _type);

        bool Update(Granary _entity);

        List<OHeap> GetList(List<string> _params);

        List<Temperature> GetHeapTempsChart(string number, int type = 0);

        List<Temperature> GetSensorsChart(string number, int type = 0);

        bool AddList2(List<Granary> _addList);

        bool IsExist2(List<string> _params);

        #endregion


    }
}
