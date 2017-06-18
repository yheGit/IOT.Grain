using Net66.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Core
{
   public class CommonCore
    {
        /// <summary>
        /// 发送定时消息
        /// </summary>
        public static void DelayedSendingMsg()
        {
            try
            {
                new Data.Context.DbEntity().DelayedSendingMsg();
            }
            catch (Exception ex)
            {
                Utils.ExceptionLog(ex, "DelayedSendingMsg");
            }
        }

    }
}
