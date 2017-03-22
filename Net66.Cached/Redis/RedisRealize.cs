using Net66.Cached.Data;
using Newtonsoft.Json.Converters;
using ServiceStack.Redis;
using ServiceStack.Redis.Support;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Net66.Cached.Redis
{
    /// <summary>
    /// Redis缓存类，直接根据SQL语句返回DataTable
    /// by: yung
    /// date:2016.12.05
    /// </summary>
    public class RedisRealize
    {
        private static IRedisClient Redis = RedisManager.GetClient();
        /// <summary>
        /// 查询Redis是否存在返回数据字典
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataCodeValue() {
            DataTable dt = new DataTable(); 
            var ser = new ObjectSerializer();
            //获取Redis操作接口  
            try
            {
                string redisgbSysCodeValue = "sfcrm_gbSysCodeValue";//数据字典
                if (RedisManager.IsOpenCache)
                {
                    dt = ser.Deserialize(Redis.Get<byte[]>(redisgbSysCodeValue)) as DataTable;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = Data.SqlDbOperator.ExecuteDatatable(CommandType.Text, "SELECT CodeValue, Description, CodeFlag FROM Sys_CodeValueDetail WHERE IsAvailable=1");
                    if (RedisManager.IsOpenCache)
                    {
                        Redis.Set<byte[]>(redisgbSysCodeValue, ser.Serialize(dt), DateTime.Now.AddDays(1));
                    }
                }
            }
            catch(Exception ex) {
                dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "SELECT CodeValue, Description, CodeFlag FROM Sys_CodeValueDetail WHERE IsAvailable=1");
            }
            return dt;
        }
        /// <summary>
        /// 渠道类型，等级
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataChaType() {
            DataTable dt = new DataTable();
            var ser = new ObjectSerializer();
            try
            {
                string redisgbSysCodeValue = "sfcrm_gbChaType";//渠道类型等级
                if (RedisManager.IsOpenCache)
                {
                    dt = ser.Deserialize(Redis.Get<byte[]>(redisgbSysCodeValue)) as DataTable;
                }
                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "SELECT ChannelTypeCode AS CodeValue, ChannelTypeName AS Description, ParentCode AS CodeFlag FROM Sys_ChannelType ");
                    if (RedisManager.IsOpenCache)
                    {
                        Redis.Set<byte[]>(redisgbSysCodeValue, ser.Serialize(dt), DateTime.Now.AddDays(1));
                    }
                }
            }
            catch(Exception ex) {
                dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "SELECT ChannelTypeCode AS CodeValue, ChannelTypeName AS Description, ParentCode AS CodeFlag FROM Sys_ChannelType ");
            }
            return dt;
        }
        /// <summary>
        /// 行业，授权行业
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataTrade()
        {
            DataTable dt = new DataTable();
            var ser = new ObjectSerializer();
            try
            {
                string redisgbSysCodeValue = "sfcrm_gbChaAuthTrade";//渠道行业
                if (RedisManager.IsOpenCache)
                {
                    dt = ser.Deserialize(Redis.Get<byte[]>(redisgbSysCodeValue)) as DataTable;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "SELECT TradeID AS CodeValue, TradeName AS Description, ParentID AS CodeFlag FROM Sys_Trade ");
                    if (RedisManager.IsOpenCache)
                    {
                        Redis.Set<byte[]>(redisgbSysCodeValue, ser.Serialize(dt), DateTime.Now.AddDays(1));
                    }
                }
            }
            catch(Exception ex) {
                //FileHelper.WriterFileDate("LogFileRedis", "Message:" + ex.Message + ";</br>InnerException:" + ex.InnerException + ";</br>Data:" + ex.Data.ToString());
                dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "SELECT TradeID AS CodeValue, TradeName AS Description, ParentID AS CodeFlag FROM Sys_Trade ");
            }

            return dt;
        }
        /// <summary>
        /// 获取在职人员名单
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataUser() {
            DataTable dt = new DataTable();
            var ser = new ObjectSerializer();
            try
            {
                string redisgbSysCodeValue = "sfcrm_gbUser";//渠道类型等级
                if (RedisManager.IsOpenCache)
                {
                    dt = ser.Deserialize(Redis.Get<byte[]>(redisgbSysCodeValue)) as DataTable;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "select  UserCode,UserName, DeptCode, IsLock from Sys_User where IsAvailable=1 and IsDel=0 ");
                    if (RedisManager.IsOpenCache)
                    {
                        Redis.Set<byte[]>(redisgbSysCodeValue, ser.Serialize(dt), DateTime.Now.AddDays(1));
                    }
                }
            }
            catch(Exception ex) {
                dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "select  UserCode,UserName, DeptCode, IsLock from Sys_User where IsAvailable=1 and IsDel=0 ");
            }
            return dt;
        }

        /// <summary>
        /// 获取产品线数据
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDataPL() {
            DataTable dt = new DataTable();
            var ser = new ObjectSerializer();
            try
            {
                string redisgbPrjPL = "sfcrm_gbPrjPL";//产品线
                if (RedisManager.IsOpenCache)
                {
                    dt = ser.Deserialize(Redis.Get<byte[]>(redisgbPrjPL)) as DataTable;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "select PLCode, PLName, ShortName, BakField5 from Sys_ProductLine where IsAvailable=1 AND BakField3=1");
                    if (RedisManager.IsOpenCache)
                    {
                        Redis.Set<byte[]>(redisgbPrjPL, ser.Serialize(dt), DateTime.Now.AddDays(1));
                    }
                }
            }
            catch(Exception ex) {
                dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "select PLCode, PLName, ShortName, BakField5 from Sys_ProductLine where IsAvailable=1 AND BakField3=1");
            }
            return dt;
        }

        public static DataTable GetDataAreaYear() {
            DataTable dt = new DataTable();
            var ser = new ObjectSerializer();
            try
            {
                string redisgb = "sfcrm_gbYear";//年份及个人权限区域
                if (RedisManager.IsOpenCache)
                {
                    dt = ser.Deserialize(Redis.Get<byte[]>(redisgb)) as DataTable;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "select hisyear  from Sys_Area group by hisyear having hisyear>0 order by hisyear desc");
                    if (RedisManager.IsOpenCache)
                    {
                        Redis.Set<byte[]>(redisgb, ser.Serialize(dt), DateTime.Now.AddDays(1));
                    }
                }
            }
            catch(Exception ex) {
                dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, "select hisyear  from Sys_Area group by hisyear having hisyear>0 order by hisyear desc");
            }
            return dt;
        }

        /// <summary>
        /// 直接执行SQL获取DataTable
        /// </summary>
        /// <param name="RedisName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetDataFromSQL(string RedisName, string sql)
        {
            DataTable dt = new DataTable();
            var ser = new ObjectSerializer();
            try
            {
                string redisgbPrjPL = RedisName;//产品线
                if (RedisManager.IsOpenCache)
                {
                    dt = ser.Deserialize(Redis.Get<byte[]>(redisgbPrjPL)) as DataTable;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, sql);
                    if (RedisManager.IsOpenCache)
                    {
                        Redis.Set<byte[]>(redisgbPrjPL, ser.Serialize(dt), DateTime.Now.AddDays(1));
                    }
                }
            }
            catch(Exception ex) {
                dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, sql);
            }
            return dt;
        }

        /// <summary>
        /// 获取下拉框选择项
        /// </summary>
        /// <param name="RedisName">Redis名称</param>
        /// <param name="tablename">表</param>
        /// <param name="codevalue">Value</param>
        /// <param name="description">Text</param>
        /// <param name="strwhere">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="RWType">来源：read, report</param>
        /// <returns></returns>
        public static DataTable GetDataParaValue(string RedisName,string tablename, string codevalue, string description, string strwhere, string orderby, string RWType = "")
        {
            DataTable dt = new DataTable();
            var ser = new ObjectSerializer();
            try
            {
                string redisgbChaType = RedisName;
                if (RedisManager.IsOpenCache)
                {
                    dt = ser.Deserialize(Redis.Get<byte[]>(redisgbChaType)) as DataTable;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    dt = GetAllCode(tablename, codevalue, description, strwhere, orderby, RWType);
                    if (RedisManager.IsOpenCache)
                    {
                        Redis.Set<byte[]>(redisgbChaType, ser.Serialize(dt), DateTime.Now.AddDays(1));
                    }
                }
            }
            catch(Exception ex) {
                dt = GetAllCode(tablename, codevalue, description, strwhere, orderby, RWType);
            }
            return dt;
        }

        #region 获取
        /// <summary>
        /// 获取某一字典的选择项
        /// </summary>
        /// <param name="swhere"></param>
        /// <param name="codeflag"></param>
        /// <param name="strorder"></param>
        /// <returns></returns>        
        private static DataTable GetAllCode(string tablename, string codevalue, string description, string strwhere, string orderby, string RWType = "")
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("select " + codevalue + " as codevalue," + description + " as description from " + tablename);
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql.Append(" where " + strwhere);
            }

            if (!string.IsNullOrEmpty(orderby))
            {
                sql.Append(" order by " + orderby);
            }



            using (DataTable dt = SqlDbOperator.ExecuteDatatable(CommandType.Text, sql.ToString(), RWType))
            {
                return dt;
            }
        }
        #endregion
    }
}
