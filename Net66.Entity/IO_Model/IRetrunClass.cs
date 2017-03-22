using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Net66.Entity.IO_Model
{
    /// <summary>
    /// 适用于webservice、webapi、一般处理程序
    /// DateTime类型序列化后为 2016-11-09T20:04:08.83
    /// </summary>
    [DataContract]
    public class ReturnClass
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
        public string JsonValue { get; set; }

        /// <summary>
        /// 返回值，可选
        /// </summary>
        [DataMember]
        public string TextValue { get; set; }

        public ReturnClass()
        { }

        /// <summary>
        /// 1000成功，
        /// 1009参数不合法，
        /// 1010Token不合法，
        /// 1011提交失败，
        /// 1012未找到数据，
        /// 1013其它
        /// </summary>
        public ReturnClass(int _code, string _msg = "", object _jsonValue = null, string _textValue = "")
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

            JsonValue = "";
            TextValue = _textValue;
            if (_jsonValue != null)
            {
                try
                {
                    JsonValue = JsonConvert.SerializeObject(_jsonValue);
                }
                catch
                {
                    JsonValue = "";
                }
            }
        }
    }

}
