using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class LineBase
    {
        public int ID { get; set; }
        /// <summary>
        /// Ͳ�ֶѺ�
        /// </summary>
        public string HeapNumber { get; set; }
        public Nullable<int> LSequence { get; set; }
        public string LineCode { get; set; }
        public string StampTime { get; set; }

    }
}
