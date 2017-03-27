using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    /// 适用于WCF
    /// 与ReturnClass的区别为，JSONValue值直接为对象，因为WCF自身具有序列化功能
    /// DateTime类型序列化后为 /Date(1478693048830+0800)/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class ResultClass<T>
    {

        /// <summary>
        /// 返回代码 1000为成功，……
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// 返回代码说明，描述，可选
        /// </summary>
        [DataMember]
        public string Msg { get; set; }

        /// <summary>
        /// 返回Json，可选
        /// </summary>
        [DataMember]
        public List<T> JsonValue { get; set; }

        /// <summary>
        /// 返回值，可选
        /// </summary>
        [DataMember]
        public string TextValue { get; set; }


        public ResultClass() { }

        /// <summary>
        /// 1000成功，
        /// 1009参数不合法，
        /// 1010Token不合法，
        /// 1011提交失败，
        /// 1012未找到数据，
        /// 1013其它
        /// </summary>
        public ResultClass(int _code, T _jsonValue, string _msg="", string _textValue = "")
        {
            Code = _code;
            if (!string.IsNullOrEmpty(_msg))
                Msg = _msg;
            else
            {
                switch (_code)
                {
                    case 1000: Msg = "成功"; break;
                    case 1009: Msg = "参数不合法"; break;
                    case 1010: Msg = "Token不合法"; break;
                    case 1011: Msg = "提交失败"; break;
                    case 1012: Msg = "未找到数据"; break;
                    case 1013: Msg = "其它"; break;
                    default: Msg = "其它"; break;
                }
            }

            JsonValue = null;
            TextValue = _textValue;
            if (_jsonValue != null)
            {
                var listJson = new List<T>() { _jsonValue };
                JsonValue = listJson;
            }
        }


        /// <summary>
        /// 1000成功，
        /// 1009参数不合法，
        /// 1010Token不合法，
        /// 1011提交失败，
        /// 1012未找到数据，
        /// 1013其它
        /// </summary>
        public ResultClass(int _code, string _msg = "", List<T> _jsonValue = null, string _textValue = "")
        {
            Code = _code;
            if (!string.IsNullOrEmpty(_msg))
                Msg = _msg;
            else
            {
                switch (_code)
                {
                    case 1000: Msg = "成功"; break;
                    case 1009: Msg = "参数不合法"; break;
                    case 1010: Msg = "Token不合法"; break;
                    case 1011: Msg = "提交失败"; break;
                    case 1012: Msg = "未找到数据"; break;
                    case 1013: Msg = "其它"; break;
                    default: Msg = "其它"; break;
                }
            }

            JsonValue = null;
            TextValue = _textValue;
            if (_jsonValue != null)
            {
                JsonValue = _jsonValue;
            }
        }


    }
}
