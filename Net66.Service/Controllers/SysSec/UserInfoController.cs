using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Net66.Service.Controllers.SysSec
{
    /// <summary>
    /// 人员
    /// </summary>
    public class UserInfoController : ApiController
    {
        // GET: UserInfo
        public bool Index()
        {
            return false;
        }

        /// <summary>
        /// 验证用户名是否存在
        /// </summary>
        /// <param name="name">用户名</param>
        /// <returns></returns>
        public bool CheckName(string name)
        {

            //bool result = m_BLL.CheckName(name);
            return false;

        }

    }
}