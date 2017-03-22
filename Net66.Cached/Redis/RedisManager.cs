using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Net66.Cached.Redis
{
    public class RedisManager
    {
        /// <summary>  
        /// redis配置文件信息  
        /// </summary>  
        private static string RedisReadPath = ConfigurationManager.ConnectionStrings["RedisReadPath"].ToString();
        private static string RedisWritePath = ConfigurationManager.ConnectionStrings["RedisWritePath"].ToString();

        private static PooledRedisClientManager _prcm;

        /// <summary>  
        /// 静态构造方法，初始化链接池管理对象  
        /// </summary>  
        static RedisManager()
        {
            CreateManager();
        }

        /// <summary>  
        /// 创建链接池管理对象  
        /// </summary>  
        private static void CreateManager()
        {
            _prcm = CreateManager(new string[] { RedisWritePath }, new string[] { RedisReadPath });
        }

        private static PooledRedisClientManager CreateManager(string[] readWriteHosts, string[] readOnlyHosts)
        {
            //WriteServerList：可写的Redis链接地址。  
            //ReadServerList：可读的Redis链接地址。  
            //MaxWritePoolSize：最大写链接数。  
            //MaxReadPoolSize：最大读链接数。  
            //AutoStart：自动重启。  
            //LocalCacheTime：本地缓存到期时间，单位:秒。  
            //RecordeLog：是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项。  
            //RedisConfigInfo类是记录redis连接信息，此信息和配置文件中的RedisConfig相呼应  

            // 支持读写分离，均衡负载   
            try
            {
                return new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
                {
                    MaxWritePoolSize = 100, // “写”链接池链接数   
                    MaxReadPoolSize = 100, // “读”链接池链接数   
                    AutoStart = true,
                    //DefaultDb = 0
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>  
        /// 客户端缓存操作对象  
        /// </summary>  
        public static IRedisClient GetClient()
        {
            try
            {
                if (_prcm == null)
                {
                    CreateManager();
                }
                return _prcm.GetClient();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 是否使用缓存开关
        /// </summary>
        public static bool IsOpenCache
        {
            get
            {
                if (ConfigHelper.GetAppSettingValue("IsOpenCache") != "1") return false;
                return true;
            }
        }

    }

    /// <summary>
    /// Redis配置文件帮助类
    /// </summary>
    public class ConfigHelper
    {

        #region 配置文件读取


        public static string GetAppSettingValue(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

    }
}
