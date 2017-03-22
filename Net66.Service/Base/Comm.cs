using Net66.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Net66.Service.Base
{
    public class Comm
    {

        /// <summary>
        /// 27-36
        /// </summary>
        /// <param name="_param"></param>
        public static void GetXYZ(int _param,out int i,out int j,out int k)
        {
            int sum = _param > 27 ? _param : 27;
            int remainder = sum / 9;//商
            int quotient = sum % 9;//余数
            int x, y, z;
            int[] arr = new int[remainder];
            int cout = 0;
            int xCount = arr.Length, yCount = arr.Length, zCount = arr.Length;
            if (quotient > 0 && quotient <= 27)
                zCount++;
            i = j = k = 1;
            for (x = 1; x <= xCount; x++)
            {
                for (y = 1; y <= yCount; y++)
                {
                    for (z = 1; z <= zCount; z++)
                    {
                        //z = 1;
                        i = x;
                        j = y;
                        k = z;
                        //Console.WriteLine("{0} {1} {2}->{3}", x, y, z, ++cout);
                    }
                }
            }

        }
        
    }
}