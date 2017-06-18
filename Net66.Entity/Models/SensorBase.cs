using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class SensorBase
    {
        public int ID { get; set; }
        /// <summary>
        /// ����������ʵID
        /// </summary>
        public string SCpu { get; set; }
        public Nullable<int> SSequen { get; set; }
        public string SLineCode { get; set; }
        public string StampTime { get; set; }
        /// <summary>
        /// ���ߵ��ź�
        /// </summary>
        public int SCount { get; set; }
        /// <summary>
        /// ��������ԭʼ������ID
        /// </summary>
        public string SCpuOrg { get; set; }
    }
}
