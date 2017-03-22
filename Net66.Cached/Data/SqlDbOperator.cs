using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.ComponentModel;
using System.Text;
using System.IO;

namespace Net66.Cached.Data
{
    /// <summary>
    /// 数据访问控制类
    /// </summary>
    public class SqlDbOperator : IDisposable
    {
        #region 私有变量，属性
        //connectionStringReport:报表； connectionString：读写；connectionStringRead：只读
        private static readonly string connectionStringReport = ConfigurationManager.ConnectionStrings["connectionStringReport"].ToString();
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["connectionStringAll"].ToString();
        private static readonly string connectionStringRead = ConfigurationManager.ConnectionStrings["connectionStringRead"].ToString();

        private IntPtr _handle;
        private Component _component = new Component();
        private bool _disposed = false;
        #endregion

        #region 构造函数，析构函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlDbOperator()
        {
        }

        ~SqlDbOperator()
        {
            this.Dispose(false);
        }

        #endregion

        #region Disposable

        /// <summary>
        /// Implement IDisposable.Do not make this method virtual.A derived class should not be able to override this method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources can be disposed.If disposing equals false, the method has been called by the runtime from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._component.Dispose();
                }
                CloseHandle(_handle);
                this._handle = IntPtr.Zero;

                this._disposed = true;
            }
        }

        /// <summary>
        /// Use interop to call the method necessary to clean up the unmanaged resource.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        #endregion

        #region ExecuteDataset 查询数据，返回数据集

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteDataset(CommandType comType,string cmdText, string RWType="")
        {
            return ExecuteDataset(comType, cmdText, null, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="tableName">数据表名称</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteDataset(CommandType comType,string cmdText, string tableName, string RWType="")
        {
            return ExecuteDataset(comType, cmdText, tableName, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteDataset(CommandType comType, string cmdText, string RWType="",params SqlParameter[] paras)
        {
            return ExecuteDataset(comType, cmdText, null, RWType, paras);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="tableName">数据表名称</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteDataset(CommandType comType, string cmdText, string tableName, string RWType="",params SqlParameter[] paras)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                if (RWType == "report")
                {
                    con.ConnectionString = connectionStringReport;
                }
                else if (RWType == "read")
                {
                    con.ConnectionString = connectionStringRead;
                }
                else {
                    con.ConnectionString = connectionString;
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmdText, con))
                {
                    da.SelectCommand.CommandType = comType;
                    DataSet ds = new DataSet();
                    ds.Locale = CultureInfo.InvariantCulture;//显式设置DataSet 的区域设置
                    if (paras != null)
                        PrepareCommand(da.SelectCommand, paras);
                    try
                    {
                        con.Open();
                        if (tableName == null)
                            da.Fill(ds);
                        else
                            da.Fill(ds, tableName);
                        return ds;
                    }
                    catch (Exception ex)
                    {
                        //WriteException(ex.Message, cmdText);
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="startRecord">起始记录</param>
        /// <param name="maxRecord">要检索的记录条数</param>
        /// <param name="tableName">数据表名称</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteDataset(CommandType comType, string cmdText, int startRecord, int maxRecord, string tableName, string RWType="")
        {
            return ExecuteDataset(comType, cmdText, startRecord, maxRecord, tableName, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="startRecord">起始记录</param>
        /// <param name="maxRecord">要检索的记录条数</param>
        /// <param name="tableName">数据表名称</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteDataset(CommandType comType, string cmdText, int startRecord, int maxRecord, string tableName, string RWType="", params SqlParameter[] paras)
        {
            using (SqlConnection con = new SqlConnection())
            {
                if (RWType == "report")
                {
                    con.ConnectionString = connectionStringReport;
                }
                else if (RWType == "read")
                {
                    con.ConnectionString = connectionStringRead;
                }
                else
                {
                    con.ConnectionString = connectionString;
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmdText, con))
                {
                    da.SelectCommand.CommandType = comType;
                    DataSet ds = new DataSet();
                    ds.Locale = CultureInfo.InvariantCulture;//显式设置DataSet 的区域设置
                    if (paras != null)
                        PrepareCommand(da.SelectCommand, paras);
                    try
                    {
                        con.Open();
                        if (tableName == null)
                            da.Fill(ds);
                        else
                            da.Fill(ds, startRecord, maxRecord, tableName);
                        return ds;
                    }
                    catch (Exception ex)
                    {
                        //WriteException(ex.Message, cmdText);
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        #endregion

        #region ExecuteDatatable 查询数据，返回数据表

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <returns>返回数据表</returns>
        public static DataTable ExecuteDatatable(CommandType comType, string cmdText, string RWType="")
        {
            return ExecuteDatatable(comType, cmdText, null, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="tableName">数据表名</param>
        /// <returns>返回数据表</returns>
        public static DataTable ExecuteDatatable(CommandType comType, string cmdText, string tableName, string RWType="")
        {
            return ExecuteDatatable(comType, cmdText, tableName, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回数据表</returns>
        public static DataTable ExecuteDatatable(CommandType comType, string cmdText, string RWType="", params SqlParameter[] paras)
        {
            return ExecuteDatatable(comType, cmdText, null, RWType, paras);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="tableName">数据表名</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回数据表</returns>
        public static DataTable ExecuteDatatable(CommandType comType, string cmdText, string tableName, string RWType="", params SqlParameter[] paras)
        {
            using (SqlConnection con = new SqlConnection())
            {
                if (RWType == "report")
                {
                    con.ConnectionString = connectionStringReport;
                }
                else if (RWType == "read")
                {
                    con.ConnectionString = connectionStringRead;
                }
                else
                {
                    con.ConnectionString = connectionString;
                }
                using (SqlDataAdapter da = new SqlDataAdapter(cmdText, con))
                {
                    da.SelectCommand.CommandType = comType;
                    DataTable dt = new DataTable();
                    if (tableName != null)
                        dt.TableName = tableName;
                    if (paras != null)
                        PrepareCommand(da.SelectCommand, paras);
                    try
                    {
                        con.Open();
                        dt.BeginLoadData();
                        da.Fill(dt);
                        dt.EndLoadData();
                        return dt;
                    }
                    catch (Exception ex)
                    {
                        //WriteException(ex.Message, cmdText);
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        #endregion

        #region ExecuteReader 查询数据,返回DataReader

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <returns>返回DataReader</returns>
        public static SqlDataReader ExecuteReader(CommandType comType, string cmdText, string RWType="")
        {
            return ExecuteReader(comType, cmdText, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回DataReader</returns>
        public static SqlDataReader ExecuteReader(CommandType comType, string cmdText, params SqlParameter[] paras)
        {
            SqlConnection con = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand(cmdText, con))
            {
                cmd.CommandType = comType;
                if (paras != null)
                    PrepareCommand(cmd, paras);
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return dr;
                }
                catch (Exception ex)
                {
                    con.Close();
                    //WriteException(ex.Message, cmdText);
                    throw new Exception(ex.Message);
                }
            }


        } 

        public static SqlDataReader ExecuteReader(CommandType comType, string cmdText, string RWType="", params SqlParameter[] paras)
        {
            SqlConnection con = new SqlConnection();
            if (RWType == "report")
            {
                con.ConnectionString = connectionStringReport;
            }
            else if (RWType == "read")
            {
                con.ConnectionString = connectionStringRead;
            }
            else
            {
                con.ConnectionString = connectionString;
            }
            using (SqlCommand cmd = new SqlCommand(cmdText, con))
            {
                cmd.CommandType = comType;
                if (paras != null)
                    PrepareCommand(cmd, paras);
                try
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return dr;
                }
                catch (Exception ex)
                {
                    con.Close();
                    //WriteException(ex.Message, cmdText);
                    throw new Exception(ex.Message);
                }
            }
            
            
        }
        #endregion

        #region ExecuteScalar 查询数据，返回结果集中的第一行第一列

        /// <summary>
        /// 执行指定数据库事务的命令,指定参数,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(CommandType comType, string cmdText)
        {
            return ExecuteScalar(comType, cmdText, (SqlParameter)null);
        }

        /// <summary>
        /// 执行指定数据库事务的命令,指定参数,返回结果集中的第一行第一列
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回结果集中的第一行第一列</returns>
        public static object ExecuteScalar(CommandType comType, string cmdText, params SqlParameter[] paras)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.CommandType = comType;

                    if (paras != null)
                        PrepareCommand(cmd, paras);

                    try
                    {
                        con.Open();
                        return cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        //WriteException(ex.Message, cmdText);
                        throw (new Exception(ex.Message));
                    }
                }
            }
        }
        #endregion

        #region ExecuteNonQuery 更新数据，返回执行结果

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <returns>返回是否执行成功</returns>
        public static bool ExecuteNonQuery(CommandType comType, string cmdText)
        {
            return ExecuteNonQuery(comType, cmdText, false, (SqlParameter[])null);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="isTransaction">是否执行事务</param>
        /// <returns>返回是否执行成功</returns>
        public static bool ExecuteNonQuery(CommandType comType, string cmdText, bool isTransaction)
        {
            return ExecuteNonQuery(comType,cmdText , isTransaction, (SqlParameter[])null);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回是否执行成功</returns>
        public static bool ExecuteNonQuery(CommandType comType, string cmdText, params SqlParameter[] paras)
        {
            return ExecuteNonQuery(comType, cmdText, false, paras);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="isTransaction">是否执行事务</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回是否执行成功</returns>
        public static bool ExecuteNonQuery(CommandType comType, string cmdText, bool isTransaction, params SqlParameter[] paras)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.CommandType = comType;
                    cmd.CommandTimeout = 120;
                    SqlTransaction tran = null;

                    if (paras != null)
                        PrepareCommand(cmd, paras);

                    try
                    {
                        con.Open();
                        if (isTransaction)
                        {
                            tran = con.BeginTransaction();
                            cmd.Transaction = tran;
                            cmd.ExecuteNonQuery();
                            tran.Commit();
                        }
                        else
                            cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        if (tran != null)
                            tran.Rollback();
                        //WriteException(ex.Message, cmdText);
                        throw (ex);
                       // return false;
                    }
                }
            }
        }
        #endregion

        #region GetParameter 返回Parameter

        /// <summary>
        /// 获取Parameter
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="paraValue">参数值</param>
        /// <returns>返回Parameter</returns>
        public static SqlParameter GetParameter(string para, object paraValue)
        {
            return new SqlParameter(para, paraValue);
        }

        /// <summary>
        /// 获取Parameter
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="paraLength">长度</param>
        /// <param name="paraValue">参数值</param>
        /// <returns>返回Parameter</returns>
        public static SqlParameter GetParameter(string para, SqlDbType dbType, int paraLength, object paraValue)
        {
            SqlParameter parameter = new SqlParameter(para, dbType, paraLength);
            parameter.Value = paraValue;
            return parameter;
        }

        /// <summary>
        /// 获取Parameter
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="paraValue">参数值</param>
        /// <returns>返回Parameter</returns>
        public static SqlParameter GetParameter(string para, SqlDbType dbType, object paraValue)
        {
            SqlParameter parameter = new SqlParameter(para, dbType);
            parameter.Value = paraValue;
            return parameter;
        }

        /// <summary>
        /// 获取Parameter
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="direction">参数返回类型</param>
        /// <returns>返回Parameter</returns>
        public static SqlParameter GetParameter(string para, SqlDbType dbType, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter(para, dbType);
            parameter.Direction = direction;
            return parameter;
        }

        /// <summary>
        /// 获取Parameter
        /// </summary>
        /// <param name="para">参数</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="paraLength">参数长度</param>
        /// <param name="direction">参数返回类型</param>
        /// <returns>返回Parameter</returns>
        public static SqlParameter GetParameter(string para, SqlDbType dbType, int paraLength, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter(para, dbType, paraLength);
            parameter.Direction = direction;
            return parameter;
        }
        #endregion

        #region 将数据参数数组(参数值)分配给SqlCommand命令

        /// <summary>
        /// 将数据参数数组(参数值)分配给SqlCommand命令
        /// </summary>
        /// <param name="cmd">命令名称</param>
        /// <param name="paras">数据参数数组(参数值)</param>
        protected static void PrepareCommand(SqlCommand cmd, params SqlParameter[] paras)
        {
            if (paras != null)
            {
                //cmd.Parameters.Clear();
                foreach (SqlParameter para in paras)
                {
                    if (para != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((para.Direction == ParameterDirection.InputOutput || para.Direction == ParameterDirection.Input || para.Direction == ParameterDirection.Output) && para.Value == null)
                        {
                            para.Value = DBNull.Value;
                        }
                        cmd.Parameters.Add(para);
                    }
                }
            }
        }

        #endregion


        #region 执行SQL语句(事务操作)
        /// <summary>
        /// 执行SQL语句(事务操作)
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="isTransaction">是否执行事务</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回是否执行成功</returns>
        public static bool ExecuteTransNonQuery(CommandType comType, string cmdText, bool isTransaction)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.CommandType = comType;
                    cmd.CommandTimeout = 120;
                    SqlTransaction tran = null;
                    try
                    {
                        con.Open();
                        if (isTransaction)
                        {
                            tran = con.BeginTransaction();//开始事物
                            cmd.Transaction = tran;
                            cmd.ExecuteNonQuery();
                            tran.Commit();//提交事物
                        }
                        else
                            cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        if (tran != null)
                            tran.Rollback();//回滚事物

                        return false;
                    }
                    finally
                    {
                        con.Close();
                        tran.Dispose();
                        con.Dispose();
                    }
                }
            }
        }
        #endregion

        #region 执行SQL语句(事务操作)
        /// <summary>
        /// 执行SQL语句(事务操作)
        /// </summary>
        /// <param name="cmdText">存储过程名或T-SQL语句</param>
        /// <param name="comType">命令类型</param>
        /// <param name="isTransaction">是否执行事务</param>
        /// <param name="paras">数据参数集</param>
        /// <returns>返回是否执行成功</returns>
        public static bool ExecuteTransNonQuery(CommandType comType, string cmdText, bool isTransaction, params SqlParameter[] paras)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, con))
                {
                    cmd.CommandType = comType;
                    cmd.CommandTimeout = 120;
                    SqlTransaction tran = null;

                    if (paras != null)
                        PrepareCommand(cmd, paras);

                    try
                    {
                        con.Open();
                        if (isTransaction)
                        {
                            tran = con.BeginTransaction();//开始事物
                            cmd.Transaction = tran;
                            cmd.ExecuteNonQuery();
                            tran.Commit();//提交事物
                        }
                        else
                            cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        if (tran != null)
                            tran.Rollback();//回滚事物

                        return false;
                    }
                    finally
                    {
                        con.Close();
                        tran.Dispose();
                        con.Dispose();
                    }
                }
            }
        }
        #endregion

        //#region 分页
        ///// <summary>
        ///// 数据分页
        ///// </summary>
        ///// <param name="cmdText">Sql语句</param>
        ///// <param name="curPage">当前为第几页</param>
        ///// <param name="pageSize">每页行数</param>
        ///// <param name="pageCount">返回的总页数</param>
        ///// <param name="rowCount">返回的总记录数</param>
        ///// <returns>返回数据表</returns>
        //public static DataTable PagerList(string cmdText, int curPage, int pageSize, out int pageCount, out int rowCount)
        //{
        //    SqlParameter[] paras = new SqlParameter[]
        //    {
        //        GetParameter("@sqlstr",SqlDbType.NVarChar,4000,cmdText),
        //        GetParameter("@currentpage",SqlDbType.Int,curPage),
        //        GetParameter("@pagesize",SqlDbType.Int,pageSize)
        //    };
        //    DataSet ds = ExecuteDataset(CommandType.StoredProcedure, "sp_PageList", paras);
        //    pageCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
        //    rowCount = Convert.ToInt32(ds.Tables[3].Rows[0][0]);
        //    return ds.Tables[2];
        //}
        //#endregion

        #region 批量更新
        public static SqlBulkCopy sqlbullcopy = new SqlBulkCopy(connectionString);
        #endregion
    }
}
