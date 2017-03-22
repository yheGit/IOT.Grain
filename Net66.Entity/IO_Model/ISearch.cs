using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/******************************************
*Creater:yhw[96160]
*CreatTime:
*Description:
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:
*******************************************/
namespace Net66.Entity.IO_Model
{
    public class ISearch:ISearchBase
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// 可选参数 eg: ["Type^1","UserId^32203"]
        /// </summary>
        public List<string> DicList{ get; set; }
    }
}
