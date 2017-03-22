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
    /// ���ݷ��ʿ�����
    /// </summary>
    public class SqlDbOperator : IDisposable
    {
        #region ˽�б���������
        //connectionStringReport:���� connectionString����д��connectionStringRead��ֻ��
        private static readonly string connectionStringReport = ConfigurationManager.ConnectionStrings["connectionStringReport"].ToString();
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["connectionStringAll"].ToString();
        private static readonly string connectionStringRead = ConfigurationManager.ConnectionStrings["connectionStringRead"].ToString();

        private IntPtr _handle;
        private Component _component = new Component();
        private bool _disposed = false;
        #endregion

        #region ���캯������������

        /// <summary>
        /// ���캯��
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

        #region ExecuteDataset ��ѯ���ݣ��������ݼ�

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <returns>�������ݼ�</returns>
        public static DataSet ExecuteDataset(CommandType comType,string cmdText, string RWType="")
        {
            return ExecuteDataset(comType, cmdText, null, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="tableName">���ݱ�����</param>
        /// <returns>�������ݼ�</returns>
        public static DataSet ExecuteDataset(CommandType comType,string cmdText, string tableName, string RWType="")
        {
            return ExecuteDataset(comType, cmdText, tableName, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>�������ݼ�</returns>
        public static DataSet ExecuteDataset(CommandType comType, string cmdText, string RWType="",params SqlParameter[] paras)
        {
            return ExecuteDataset(comType, cmdText, null, RWType, paras);
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="tableName">���ݱ�����</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>�������ݼ�</returns>
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
                    ds.Locale = CultureInfo.InvariantCulture;//��ʽ����DataSet ����������
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
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="startRecord">��ʼ��¼</param>
        /// <param name="maxRecord">Ҫ�����ļ�¼����</param>
        /// <param name="tableName">���ݱ�����</param>
        /// <returns>�������ݼ�</returns>
        public static DataSet ExecuteDataset(CommandType comType, string cmdText, int startRecord, int maxRecord, string tableName, string RWType="")
        {
            return ExecuteDataset(comType, cmdText, startRecord, maxRecord, tableName, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="startRecord">��ʼ��¼</param>
        /// <param name="maxRecord">Ҫ�����ļ�¼����</param>
        /// <param name="tableName">���ݱ�����</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>�������ݼ�</returns>
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
                    ds.Locale = CultureInfo.InvariantCulture;//��ʽ����DataSet ����������
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

        #region ExecuteDatatable ��ѯ���ݣ��������ݱ�

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <returns>�������ݱ�</returns>
        public static DataTable ExecuteDatatable(CommandType comType, string cmdText, string RWType="")
        {
            return ExecuteDatatable(comType, cmdText, null, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="tableName">���ݱ���</param>
        /// <returns>�������ݱ�</returns>
        public static DataTable ExecuteDatatable(CommandType comType, string cmdText, string tableName, string RWType="")
        {
            return ExecuteDatatable(comType, cmdText, tableName, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>�������ݱ�</returns>
        public static DataTable ExecuteDatatable(CommandType comType, string cmdText, string RWType="", params SqlParameter[] paras)
        {
            return ExecuteDatatable(comType, cmdText, null, RWType, paras);
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="tableName">���ݱ���</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>�������ݱ�</returns>
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

        #region ExecuteReader ��ѯ����,����DataReader

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <returns>����DataReader</returns>
        public static SqlDataReader ExecuteReader(CommandType comType, string cmdText, string RWType="")
        {
            return ExecuteReader(comType, cmdText, RWType, (SqlParameter[])null);
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>����DataReader</returns>
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

        #region ExecuteScalar ��ѯ���ݣ����ؽ�����еĵ�һ�е�һ��

        /// <summary>
        /// ִ��ָ�����ݿ����������,ָ������,���ؽ�����еĵ�һ�е�һ��
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
        public static object ExecuteScalar(CommandType comType, string cmdText)
        {
            return ExecuteScalar(comType, cmdText, (SqlParameter)null);
        }

        /// <summary>
        /// ִ��ָ�����ݿ����������,ָ������,���ؽ�����еĵ�һ�е�һ��
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>���ؽ�����еĵ�һ�е�һ��</returns>
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

        #region ExecuteNonQuery �������ݣ�����ִ�н��

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <returns>�����Ƿ�ִ�гɹ�</returns>
        public static bool ExecuteNonQuery(CommandType comType, string cmdText)
        {
            return ExecuteNonQuery(comType, cmdText, false, (SqlParameter[])null);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="isTransaction">�Ƿ�ִ������</param>
        /// <returns>�����Ƿ�ִ�гɹ�</returns>
        public static bool ExecuteNonQuery(CommandType comType, string cmdText, bool isTransaction)
        {
            return ExecuteNonQuery(comType,cmdText , isTransaction, (SqlParameter[])null);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>�����Ƿ�ִ�гɹ�</returns>
        public static bool ExecuteNonQuery(CommandType comType, string cmdText, params SqlParameter[] paras)
        {
            return ExecuteNonQuery(comType, cmdText, false, paras);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="isTransaction">�Ƿ�ִ������</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>�����Ƿ�ִ�гɹ�</returns>
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

        #region GetParameter ����Parameter

        /// <summary>
        /// ��ȡParameter
        /// </summary>
        /// <param name="para">����</param>
        /// <param name="paraValue">����ֵ</param>
        /// <returns>����Parameter</returns>
        public static SqlParameter GetParameter(string para, object paraValue)
        {
            return new SqlParameter(para, paraValue);
        }

        /// <summary>
        /// ��ȡParameter
        /// </summary>
        /// <param name="para">����</param>
        /// <param name="dbType">��������</param>
        /// <param name="paraLength">����</param>
        /// <param name="paraValue">����ֵ</param>
        /// <returns>����Parameter</returns>
        public static SqlParameter GetParameter(string para, SqlDbType dbType, int paraLength, object paraValue)
        {
            SqlParameter parameter = new SqlParameter(para, dbType, paraLength);
            parameter.Value = paraValue;
            return parameter;
        }

        /// <summary>
        /// ��ȡParameter
        /// </summary>
        /// <param name="para">����</param>
        /// <param name="dbType">��������</param>
        /// <param name="paraValue">����ֵ</param>
        /// <returns>����Parameter</returns>
        public static SqlParameter GetParameter(string para, SqlDbType dbType, object paraValue)
        {
            SqlParameter parameter = new SqlParameter(para, dbType);
            parameter.Value = paraValue;
            return parameter;
        }

        /// <summary>
        /// ��ȡParameter
        /// </summary>
        /// <param name="para">����</param>
        /// <param name="dbType">��������</param>
        /// <param name="direction">������������</param>
        /// <returns>����Parameter</returns>
        public static SqlParameter GetParameter(string para, SqlDbType dbType, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter(para, dbType);
            parameter.Direction = direction;
            return parameter;
        }

        /// <summary>
        /// ��ȡParameter
        /// </summary>
        /// <param name="para">����</param>
        /// <param name="dbType">��������</param>
        /// <param name="paraLength">��������</param>
        /// <param name="direction">������������</param>
        /// <returns>����Parameter</returns>
        public static SqlParameter GetParameter(string para, SqlDbType dbType, int paraLength, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter(para, dbType, paraLength);
            parameter.Direction = direction;
            return parameter;
        }
        #endregion

        #region �����ݲ�������(����ֵ)�����SqlCommand����

        /// <summary>
        /// �����ݲ�������(����ֵ)�����SqlCommand����
        /// </summary>
        /// <param name="cmd">��������</param>
        /// <param name="paras">���ݲ�������(����ֵ)</param>
        protected static void PrepareCommand(SqlCommand cmd, params SqlParameter[] paras)
        {
            if (paras != null)
            {
                //cmd.Parameters.Clear();
                foreach (SqlParameter para in paras)
                {
                    if (para != null)
                    {
                        // ���δ����ֵ���������,���������DBNull.Value.
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


        #region ִ��SQL���(�������)
        /// <summary>
        /// ִ��SQL���(�������)
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="isTransaction">�Ƿ�ִ������</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>�����Ƿ�ִ�гɹ�</returns>
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
                            tran = con.BeginTransaction();//��ʼ����
                            cmd.Transaction = tran;
                            cmd.ExecuteNonQuery();
                            tran.Commit();//�ύ����
                        }
                        else
                            cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        if (tran != null)
                            tran.Rollback();//�ع�����

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

        #region ִ��SQL���(�������)
        /// <summary>
        /// ִ��SQL���(�������)
        /// </summary>
        /// <param name="cmdText">�洢��������T-SQL���</param>
        /// <param name="comType">��������</param>
        /// <param name="isTransaction">�Ƿ�ִ������</param>
        /// <param name="paras">���ݲ�����</param>
        /// <returns>�����Ƿ�ִ�гɹ�</returns>
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
                            tran = con.BeginTransaction();//��ʼ����
                            cmd.Transaction = tran;
                            cmd.ExecuteNonQuery();
                            tran.Commit();//�ύ����
                        }
                        else
                            cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch
                    {
                        if (tran != null)
                            tran.Rollback();//�ع�����

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

        //#region ��ҳ
        ///// <summary>
        ///// ���ݷ�ҳ
        ///// </summary>
        ///// <param name="cmdText">Sql���</param>
        ///// <param name="curPage">��ǰΪ�ڼ�ҳ</param>
        ///// <param name="pageSize">ÿҳ����</param>
        ///// <param name="pageCount">���ص���ҳ��</param>
        ///// <param name="rowCount">���ص��ܼ�¼��</param>
        ///// <returns>�������ݱ�</returns>
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

        #region ��������
        public static SqlBulkCopy sqlbullcopy = new SqlBulkCopy(connectionString);
        #endregion
    }
}
