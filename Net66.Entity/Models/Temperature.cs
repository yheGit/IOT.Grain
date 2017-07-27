using System;
using System.Collections.Generic;

namespace Net66.Entity.Models
{
    public partial class Temperature
    {
        public int ID { get; set; }
        public Nullable<decimal> Temp { get; set; }
        public string StampTime { get; set; }
        public Nullable<DateTime> UpdateTime { get; set; }
        /// <summary>
        /// 1��������ID���ֻ�ID������ID��2��DAY(���24Сʱ)��MONTH(���һ����)��YEAR(���һ��)
        /// </summary>
        public string PId { get; set; }
        public Nullable<int> RealHeart { get; set; }
        /// <summary>
        /// 0�������������¶ȣ���1�ɼ�������λƽ���£���2�ռ����������¶ȣ���3�ռ��������⣩��-4ɾ��
        /// </summary>
        public Nullable<int> Type { get; set; }
        public string WH_Number { get; set; }
        public string G_Number { get; set; }
        public string H_Number { get; set; }

        public string TimeFlag { get; set; }

    }
}
