using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Entity.IO_Model
{
    public class ReturnData
    {
        /// <summary>
        /// 返回代码，1000为成功，1001到1008为每个方法自定义，1009参数不合法，1010Token不合法，1011提交失败,1012未找到数据，1013其它，1014身份不合法
        /// </summary>
        public int Code
        {
            get;
            set;
        }
        public string Msg
        {
            get;
            set;
        }
        public datagrid DataValue
        {
            get;
            set;
        }
        public string TextValue
        {
            get;
            set;
        }


        /// <summary>
        /// 1009参数不合法,1010Token不合法,1011提交失败,1012未找到数据,1013其它,1014身份不合法
        /// </summary>
        /// <param name="_code"></param>
        /// <param name="_msg"></param>
        /// <param name="_jsonValue"></param>
        /// <param name="_textValue"></param>
        public ReturnData(int _code, string _msg = "", datagrid _jsonValue = null, string _textValue = "")
        {
            this.Code = _code;
            if (!string.IsNullOrEmpty(_msg))
            {
                this.Msg = _msg;
            }
            else
            {
                if (_code != 1000)
                {
                    switch (_code)
                    {
                        case 1009:
                            this.Msg = "参数不合法";
                            break;
                        case 1010:
                            this.Msg = "Token不合法";
                            break;
                        case 1011:
                            this.Msg = "提交失败";
                            break;
                        case 1012:
                            this.Msg = "未找到数据";
                            break;
                        case 1013:
                            this.Msg = "其它";
                            break;
                        case 1014:
                            this.Msg = "身份不合法";
                            break;
                        default:
                            this.Msg = "";
                            break;
                    }
                }
                else
                {
                    this.Msg = "成功";
                }
            }
            this.DataValue = null;
            if (string.IsNullOrEmpty(_textValue))
                _textValue = "";

            this.TextValue = _textValue;
            if (_jsonValue != null)
                this.DataValue = _jsonValue;

        }


    }

    public class datagrid
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int total;
        /// <summary>
        /// 行数据集
        /// </summary>
        public dynamic rows;
    }



}
