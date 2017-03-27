using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
    #region 通用返回类
    /// <summary>
    /// 返回的构造实体
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class ReturnClass<T1, T2>
    {


        #region 私有变量
        private bool _isSuccess;
        private T1 _returnObject1;
        private T2 _returnObject2;
        #endregion

        #region 构造函数
        public ReturnClass()
        {

        }

        public ReturnClass(bool isSuccess, T1 returnObject1, T2 returnObject2)
        {
            IsSuccess = isSuccess;
            _returnObject1 = returnObject1;
            _returnObject2 = returnObject2;
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess
        {
            get { return _isSuccess; }
            set { _isSuccess = value; }
        }

        /// <summary>
        /// 返回对象1
        /// </summary>
        public T1 ReturnObject1
        {
            get { return _returnObject1; }
            set { _returnObject1 = value; }
        }

        /// <summary>
        /// 返回对象2
        /// </summary>
        public T2 ReturnObject2
        {
            get { return _returnObject2; }
            set { _returnObject2 = value; }
        }
        #endregion
    }
    #endregion

}
