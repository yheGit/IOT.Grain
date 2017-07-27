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
namespace Net66.Entity.IO_Model
{
    public class OSensor
    {
        public OSensor(){}

        public OSensor(Net66.Entity.Models.Temperature t) 
        {
            if (t != null)
            {
                RealTemp = t.Temp;
                LastDateTime = t.UpdateTime;
            }
        }

        public int ID { get; set; }
        public string SensorId { get; set; }
        public string CRC { get; set; }
        public string Label { get; set; }
        public string Sequen { get; set; }
        public string Collector { get; set; }
        public Nullable<int> Direction_X { get; set; }
        public Nullable<int> Direction_Y { get; set; }
        public Nullable<int> Direction_Z { get; set; }
        public Nullable<decimal> MaxTemp { get; set; }
        public Nullable<decimal> MinTemp { get; set; }
        /// <summary>
        /// 实时温度
        /// </summary>
        public Nullable<decimal> RealTemp { get; set; }
        /// <summary>
        /// 最后/最近一次温度上传时间
        /// </summary>
        public DateTime? LastDateTime { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> IsBad { get; set; }//0否，1已坏
        public string GuidID { get; set; }

    }
}
