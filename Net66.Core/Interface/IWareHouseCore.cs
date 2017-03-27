using Net66.Entity.IO_Model;
using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
namespace Net66.Core.Interface
{
    public interface IWareHouseCore
    {

        List<OWareHouse> GetPageLists(ISearch _search,List<string> _params);

        bool AddWareHouse(IWareHouse _entity);

        bool HasExist(string _code);

        bool UpdateWareHouse(WareHouse _entity);

        bool DeleteWareHouse(List<WareHouse> _delList);


        Dictionary<string, object[]> GetBBDList();

    }
}
