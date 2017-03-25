using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net66.Comm.SysApi
{
   public class Tools
    {
        /// <summary>
        /// py温度算法
        /// </summary>
        public static decimal ConvertTemp(int _temp)
        {
            decimal num = _temp / 4;
            var re= Math.Round(num, 2);
            return re;
        }

        /// <summary>
        /// py温度获取方法
        /// </summary>
        public static decimal GetTemp(string _temp, int i = 0)
        {
            if (string.IsNullOrEmpty(_temp))
                return 0;
            var index = i * 2;
            var dex = _temp.Substring(index, 2);
            var numTemp = TypeParse._16NAC_To_10NSC(dex);
            return ConvertTemp(numTemp);
        }


        public static decimal GetTempTest(string _temp, int i = 0)
        {
            if (string.IsNullOrEmpty(_temp))
                return 0;
            var index = i * 2;
            var dex = _temp.Substring(index, 2);
            return TypeParse._16NAC_To_10NSC(dex);
        }

    }
}
