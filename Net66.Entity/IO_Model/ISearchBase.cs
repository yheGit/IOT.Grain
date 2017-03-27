using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/******************************************
*Creater:yhw[]
*CreatTime:
*Description:
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:
*******************************************/
namespace Net66.Entity.IO_Model
{
    /// <summary>
    /// 增量分屏分页拉取
    /// </summary>
    public class ISearchBase
    {
        public ISearchBase()
        {

            this.PageIndex = 0;
            this.PageCount = 0;
            this.OrderType = 0;
            this.UpdateTime = string.Empty;        
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页容
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// 排序 0降序 1升序(默认是数据的时间)
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 增量拉取的时间
        /// </summary>
        public string UpdateTime { get; set; }       

        /// <summary>
        /// 可选参数
        /// </summary>
        public Dictionary<string, object> Dic { get; set; }

    }
}
