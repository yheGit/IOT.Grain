using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.Interface
{
   public interface IHeapCore
    {
        List<Heap> GetPageLists(ISearch _search, List<string> _params);

        bool AddHeap(List<Heap> _addList);

        bool UpdateHeap(Heap _entity);

        bool DeleteHeap(List<Heap> _delList);

        bool IsExist(Heap _entity);

    }
}
