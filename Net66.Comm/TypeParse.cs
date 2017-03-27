using System;
using System.Text.RegularExpressions;

namespace Net66.Comm
{
    public class TypeParse
    {

        /// <summary>
        /// �ж϶����Ƿ�ΪInt32���͵�����
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
        /// �ж϶����Ƿ�ΪInt32���͵�����
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
        /// �Ƿ�ΪDouble����
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
        /// string��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
        public static bool StrToBool(object expression, bool defValue)
        {
            if (expression != null)
            {
                return StrToBool(expression, defValue);
            }
            return defValue;
        }

        /// <summary>
        /// string��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
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
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static int StrToInt(object expression, int defValue)
        {
            if (expression != null)
            {
                return StrToInt(expression.ToString(), defValue);
            }
            return defValue;
        }

        /// <summary>
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="str">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
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
        /// string��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null))
            {
                return defValue;
            }

            return StrToFloat(strValue.ToString(), defValue);
        }

        /// <summary>
        /// string��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
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
        ///  string��ת��Ϊdouble�� by yhw 2016-11-18 17:53:32
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
        /// string��ת��Ϊdouble�� by yhw 2016-11-18 17:53:03
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
        /// ��֤�Ƿ�ΪDouble���� by yhw
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
        /// string��ת��Ϊlong��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����long���ͽ��</returns>
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
        /// ��ʽ���ַ���
        /// </summary>
        /// <param name="_format">_format=2 С���������λС��</param>
        public static string FormatStr(string _str, int _format = 2)
        {
            if (_format == 2)
                _str= string.Format("{0:N2}", _str);

           var s= Math.Round(StrToDouble(_str,0), 2);//������������Ĺ��ʱ�׼

            return _str;
        }

        /// <summary>
        /// 16�����ַ���ת��Ϊ10���� 
        /// </summary>
        /// <param name="_strNSC">16���Ƶ��ַ�����ʽ</param>
        /// <returns></returns>
        public static Int32 _16NAC_To_10NSC(string _strNSC)
        {
            return Convert.ToInt32(_strNSC, 16);
        }

        /// <summary>
        /// 10����ת16����
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
            ////ʮ����ת�������ַ���
            //Convert.ToString(d, 2);
            ////���: 1010

            ////ʮ����תʮ�������ַ���
            //Convert.ToString(d, 16);
            ////���: a

            ////�������ַ���תʮ������
            //string bin = "1010";
            //Convert.ToInt32(bin, 2);
            ////���: 10

            ////�������ַ���תʮ��������
            //string bin = "1010";
            //string.Format("{0:x}", Convert.ToInt32(bin, 2));
            ////���: a

            //////ʮ������ת�������ַ���
            ////Convert.ToString(0xa, 2);
            //////���: 1010

            ////ʮ������תʮ������
            //Convert.ToString(0xa, 10);
            ////���: 10

            return 0;
        }


        /// <summary>
        /// �жϸ������ַ�������(strNumber)�е������ǲ��Ƕ�Ϊ��ֵ��
        /// </summary>
        /// <param name="strNumber">Ҫȷ�ϵ��ַ�������</param>
        /// <returns>���򷵼�true �����򷵻� false</returns>
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
        /// ��������ʱ��ת��Ϊ��׼ʱ��(�򻯰��ISO8601����ĸ�ʽ)  by yhw 2016-11-14 19:16:21
        ///  //javaʱ��ת��ΪC#ʱ��
        /// </summary>
        /// <param name="mitime"></param>
        /// <returns></returns>
        public static DateTime JavaTimeToDateTime(long mitime)
        {
            DateTime dt_1970 = new DateTime(1970, 1, 1, 0, 0, 0);
            long tricks_1970 = dt_1970.Ticks;//1970��1��1�տ̶�                         
            long time_tricks = tricks_1970 + mitime * 10000;//��־���ڿ̶�                         
            DateTime dt = new DateTime(time_tricks).AddHours(8);//ת��ΪDateTime
            return dt;
        }


        /// <summary>
        /// ��׼ʱ��(�򻯰��ISO8601����ĸ�ʽ) ת��Ϊ��������ʱ�� by yhw 2016-11-14 19:16:10
        /// //c#ʱ��ת��Ϊjavaʱ��
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long DateTimeToJavaTime(DateTime time)
        {
            TimeSpan ts = new TimeSpan(time.AddHours(-8).Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// ��ȡϵͳʱ�䲢��ʽ��Ϊָ���ַ����� by yhw 2016-11-14 19:23:10
        /// </summary>
        /// <returns></returns>
        public static string GetSysDateTime(string _format = "yyyy-MM-dd HH:mm:ss")
        {

            return DateTime.Now.ToString(_format);
        }

        /// <summary>
        /// ���ر�׼���ڸ�ʽstring by yhw 2016-11-16 15:06:27
        /// </summary>
        public static string GetDateString()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        /// <summary>
        /// ���ر�׼���ڸ�ʽstring by yhw 2016-11-16 15:06:29
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
