using Net66.Entity.IO_Model;
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
namespace Net66.Core.Interface
{
    public interface ISensorCore
    {

        List<Net66.Entity.IO_Model.OSensor> GetSensorList(string id);


        OSensorBase GetHeapLineCount(string heapNumber);

        bool UpdateSensorBaseList(List<ISensorBase> list);

    }
}
