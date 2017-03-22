using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core.Interface
{
   public interface IFloorCore
    {
        //List<Floor> GetList_Floor(List<int> _whIdList);      

        bool AddFloor(List<Floor> _addList);

        bool UpdateFloor(Floor _entiy);

        bool DeleteFloor(List<Floor> _delList);

        bool HasExist(Floor _entiy);
    }
}
