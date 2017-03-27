using Net66.Core.Interface;
using Net66.Data.Interface;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/******************************************
*Creater:yhw[]
*CreatTime:
*Description:
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:
*******************************************/
namespace Net66.Core
{
    public class FloorCore:IFloorCore
    {
        private static IGrainRepository<Floor> fRepository;

        public FloorCore(IGrainRepository<Floor> _fRepository)
        {
            fRepository = _fRepository;
        }

        public bool AddFloor(List<Floor> _addList)
        {
            //_entity.IsActive = 1;

            _addList.ForEach(a => a.IsActive = 1);
           
            var reInt=fRepository.Add(_addList);
            return reInt > 0;

        }

        public bool DeleteFloor(List<Floor> _delList)
        {
            var reInt = fRepository.Delete(_delList, new string[] { "Number" });
            return reInt > 0;
        }

        public bool UpdateFloor(Floor _entiy)
        {
            var fieldArr = new string[] { "IsActive", "Location", "WH_Number","UserId" };
            var reInt = fRepository.Update(new List<Floor>() { _entiy }, new string[] { "Number" }, fieldArr, "StampTime");
            return reInt > 0;
        }

        public bool HasExist(Floor _entiy)
        {
            var ifno = fRepository.Get(g => g.Number == _entiy.Number&&g.WH_Number==_entiy.WH_Number);
            if (ifno == null)
                return false;
            return true;

        }
    }
}
