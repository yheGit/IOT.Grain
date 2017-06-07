using Net66.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class OHeap
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> PID { get; set; }
        public Nullable<int> WH_ID { get; set; }
        public string WH_Number { get; set; }
        public Nullable<decimal> AverageHumidity { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsActive { get; set; }
        public List<OSensor> SensorList { get; set; }
        public List<int> LineCount { get; set; }

        /// <summary>
        /// 最高温度
        /// </summary>
        public Nullable<decimal> MaxiTemperature { get; set; }
        /// <summary>
        /// 最低温度
        /// </summary>
        public Nullable<decimal> MinTemperature { get; set; }
        /// <summary>
        /// 平均温度
        /// </summary>
        public Nullable<decimal> AverageTemperature { get; set; }
        /// <summary>
        /// 坏点数
        /// </summary>
        public Nullable<int> BadPoints { get; set; }
        /// <summary>
        /// 仓内温度
        /// </summary>
        public Nullable<decimal> InSideTemperature { get; set; }
        /// <summary>
        /// 仓外温度
        /// </summary>
        public Nullable<decimal> OutSideTemperature { get; set; }

        /// <summary>
        /// 最后一次采集时间
        /// </summary>
        public string LastTime { get; set; }

        public int? Sort { get; set; }//排序


    }
}
