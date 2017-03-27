using System;
using System.Text.RegularExpressions;

namespace Net66.Comm
{
    public class TypeParse
    {

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object expression)
        {
            if (expression != null)
            {
                return IsNumeric(expression.ToString());
            }
            return false;

        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (expression != null)
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object expression)
        {
            if (expression != null)
            {
                return Regex.IsMatch(expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                return StrToBool(expression, defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string expression, bool defValue)
        {
            if (expression != null)
            {
                if (string.Compare(expression, "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(expression, "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object expression, int defValue)
        {
            if (expression != null)
            {
                return StrToInt(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string str, int defValue)
        {
            if (str == null)
                return defValue;
            if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*$"))
            {
                if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                {
                    return Convert.ToInt32(str);
                }
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
            {
                return defValue;
            }

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                }
            }
            return intValue;
        }

        /// <summary>
        ///  string型转换为double型 by yhw 2016-11-18 17:53:32
        /// </summary>
        public static double StrToDouble(object strValue, double defValue)
        {
            if ((strValue == null))
            {
                return defValue;
            }

            return StrToDouble(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string型转换为double型 by yhw 2016-11-18 17:53:03
        /// </summary>
        public static double StrToDouble(string strValue, double defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
            {
                return defValue;
            }

            double intValue = defValue;
            if (strValue != null)
            {
                bool isDouble = IsDouble(strValue);
                if (isDouble)
                {
                    intValue = Convert.ToDouble(strValue);
                }
            }
            return intValue;
        }

        /// <summary>
        /// 验证是否为Double类型 by yhw
        /// </summary>
        public static bool IsDouble(string _String)
        {
            try
            {
                Double.Parse(_String);
            }
            catch
            {
                return false;
            }
            return true;
        }    

        /// <summary>
        /// string型转换为long型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的long类型结果</returns>
        public static long StrToLong(string strValue, long defValue)
        {
            if (string.IsNullOrEmpty(strValue))
                return defValue;

            long intValue = defValue;
            try
            {
                intValue = Convert.ToInt64(strValue);
            }
            catch
            {
                return intValue;
            }
            return intValue;
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="_format">_format=2 小数点后保留两位小数</param>
        public static string FormatStr(string _str, int _format = 2)
        {
            if (_format == 2)
                _str= string.Format("{0:N2}", _str);

           var s= Math.Round(StrToDouble(_str,0), 2);//按照四舍五入的国际标准

            return _str;
        }

        /// <summary>
        /// 16进制字符串转换为10进制 
        /// </summary>
        /// <param name="_strNSC">16进制的字符串形式</param>
        /// <returns></returns>
        public static Int32 _16NAC_To_10NSC(string _strNSC)
        {
            return Convert.ToInt32(_strNSC, 16);
        }

        /// <summary>
        /// 10进制转16进制
        /// </summary>
        /// <param name="_nac"></param>
        /// <returns></returns>
        public static string _10NSC_To_16NAC(int _nac)
        {
            return _nac.ToString("X2");
        }

        public static Int32 NumberSystemConversion(string _str, int Num = 2)
        {
          return  Convert.ToInt32(_str, Num);
            ////十进制转二进制字符串
            //Convert.ToString(d, 2);
            ////输出: 1010

            ////十进制转十六进制字符串
            //Convert.ToString(d, 16);
            ////输出: a

            ////二进制字符串转十进制数
            //string bin = "1010";
            //Convert.ToInt32(bin, 2);
            ////输出: 10

            ////二进制字符串转十六进制数
            //string bin = "1010";
            //string.Format("{0:x}", Convert.ToInt32(bin, 2));
            ////输出: a

            //////十六进制转二进制字符串
            ////Convert.ToString(0xa, 2);
            //////输出: 1010

            ////十六进制转十进制数
            //Convert.ToString(0xa, 10);
            ////输出: 10

            return 0;
        }


        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 格林威治时间转换为标准时间(简化版的ISO8601延伸的格式)  by yhw 2016-11-14 19:16:21
        ///  //java时间转换为C#时间
        /// </summary>
        /// <param name="mitime"></param>
        /// <returns></returns>
        public static DateTime JavaTimeToDateTime(long mitime)
        {
            DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
            long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度                         
            long time_tricks = tricks_1970 + mitime * 10000;//日志日期刻度                         
            DateTime dt = new DateTime(time_tricks).AddHours(8);//转化为DateTime
            return dt;
        }


        /// <summary>
        /// 标准时间(简化版的ISO8601延伸的格式) 转换为格林威治时间 by yhw 2016-11-14 19:16:10
        /// //c#时间转换为java时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long DateTimeToJavaTime(DateTime time)
        {
            TimeSpan ts = new TimeSpan(time.AddHours(-8).Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 获取系统时间并格式化为指定字符类型 by yhw 2016-11-14 19:23:10
        /// </summary>
        /// <returns></returns>
        public static string GetSysDateTime(string _format = "yyyy-MM-dd HH:mm:ss")
        {

            return DateTime.Now.ToString(_format);
        }

        /// <summary>
        /// 返回标准日期格式string by yhw 2016-11-16 15:06:27
        /// </summary>
        public static string GetDateString()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// 返回标准日期格式string by yhw 2016-11-16 15:06:29
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
