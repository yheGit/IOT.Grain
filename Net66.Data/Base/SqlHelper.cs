using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net66.Data.Base
{
    public class SqlHelper
    {
        public string connectionString = "";
        public SqlHelper(string _connectionString)
        {
            this.connectionString = _connectionString;
        }
        public int ExecuteSql(string SQLString)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        result = sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException arg_29_0)
                    {
                        sqlConnection.Close();
                        return -1;
                    }
                }
            }
            return result;
        }
        public int ExecuteSqlByTime(string SQLString, int Times)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandTimeout = Times;
                        result = sqlCommand.ExecuteNonQuery();
                    }
                    catch (SqlException arg_30_0)
                    {
                        sqlConnection.Close();
                        throw arg_30_0;
                    }
                }
            }
            return result;
        }
        public int ExecuteSqlTran(List<SqlComm> list, List<SqlComm> oracleCmdSqlList)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    foreach (SqlComm current in list)
                    {
                        string commandText = current.CommandText;
                        SqlParameter[] cmdParms = (SqlParameter[])current.Parameters;
                        SqlHelper.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, commandText, cmdParms);
                        if (current.EffentNextType == EffentNextType.SolicitationEvent)
                        {
                            if (current.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                sqlTransaction.Rollback();
                                throw new Exception("Î¥±³ÒªÇó" + current.CommandText + "±ØÐë·ûºÏselect count(..µÄ¸ñÊ½");
                            }
                            object obj = sqlCommand.ExecuteScalar();
                            if (obj == null)
                            {
                                DBNull arg_BD_0 = DBNull.Value;
                            }
                            if (Convert.ToInt32(obj) > 0)
                            {
                                current.OnSolicitationEvent();
                            }
                        }
                        if (current.EffentNextType == EffentNextType.WhenHaveContine || current.EffentNextType == EffentNextType.WhenNoHaveContine)
                        {
                            if (current.CommandText.ToLower().IndexOf("count(") == -1)
                            {
                                sqlTransaction.Rollback();
                                throw new Exception("SQL:Î¥±³ÒªÇó" + current.CommandText + "±ØÐë·ûºÏselect count(..µÄ¸ñÊ½");
                            }
                            object obj2 = sqlCommand.ExecuteScalar();
                            if (obj2 != null || obj2 == DBNull.Value)
                            {
                            }
                            bool flag = Convert.ToInt32(obj2) > 0;
                            if (current.EffentNextType == EffentNextType.WhenHaveContine && !flag)
                            {
                                sqlTransaction.Rollback();
                                throw new Exception("SQL:Î¥±³ÒªÇó" + current.CommandText + "·µ»ØÖµ±ØÐë´óÓÚ0");
                            }
                            if (current.EffentNextType == EffentNextType.WhenNoHaveContine & flag)
                            {
                                sqlTransaction.Rollback();
                                throw new Exception("SQL:Î¥±³ÒªÇó" + current.CommandText + "·µ»ØÖµ±ØÐëµÈÓÚ0");
                            }
                        }
                        else
                        {
                            int num = sqlCommand.ExecuteNonQuery();
                            if (current.EffentNextType == EffentNextType.ExcuteEffectRows && num == 0)
                            {
                                sqlTransaction.Rollback();
                                throw new Exception("SQL:Î¥±³ÒªÇó" + current.CommandText + "±ØÐëÓÐÓ°ÏìÐÐ");
                            }
                            sqlCommand.Parameters.Clear();
                        }
                    }
                    sqlTransaction.Commit();
                    result = 1;
                }
                catch (SqlException arg_21C_0)
                {
                    sqlTransaction.Rollback();
                    throw arg_21C_0;
                }
                catch (Exception arg_223_0)
                {
                    sqlTransaction.Rollback();
                    throw arg_223_0;
                }
            }
            return result;
        }
        public int ExecuteSqlTran(List<string> SQLStringList)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.Transaction = sqlTransaction;
                try
                {
                    int num = 0;
                    for (int i = 0; i < SQLStringList.Count; i++)
                    {
                        string text = SQLStringList[i];
                        if (text.Trim().Length > 1)
                        {
                            sqlCommand.CommandText = text;
                            num += sqlCommand.ExecuteNonQuery();
                        }
                    }
                    sqlTransaction.Commit();
                    result = num;
                }
                catch
                {
                    sqlTransaction.Rollback();
                    result = 0;
                }
            }
            return result;
        }
        public int ExecuteSql(string SQLString, string content)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection);
                SqlParameter sqlParameter = new SqlParameter("@content", SqlDbType.NText);
                sqlParameter.Value = content;
                sqlCommand.Parameters.Add(sqlParameter);
                try
                {
                    sqlConnection.Open();
                    result = sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException arg_44_0)
                {
                    throw arg_44_0;
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
            return result;
        }
        public object ExecuteSqlGet(string SQLString, string content)
        {
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection);
                SqlParameter sqlParameter = new SqlParameter("@content", SqlDbType.NText);
                sqlParameter.Value = content;
                sqlCommand.Parameters.Add(sqlParameter);
                try
                {
                    sqlConnection.Open();
                    object obj = sqlCommand.ExecuteScalar();
                    if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                    {
                        result = null;
                    }
                    else
                    {
                        result = obj;
                    }
                }
                catch (SqlException arg_62_0)
                {
                    throw arg_62_0;
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
            return result;
        }
        public int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
                SqlParameter sqlParameter = new SqlParameter("@fs", SqlDbType.Image);
                sqlParameter.Value = fs;
                sqlCommand.Parameters.Add(sqlParameter);
                try
                {
                    sqlConnection.Open();
                    result = sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException arg_43_0)
                {
                    throw arg_43_0;
                }
                finally
                {
                    sqlCommand.Dispose();
                    sqlConnection.Close();
                }
            }
            return result;
        }
        public object GetSingle(string SQLString)
        {
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        object obj = sqlCommand.ExecuteScalar();
                        if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                        {
                            result = null;
                        }
                        else
                        {
                            result = obj;
                        }
                    }
                    catch (SqlException arg_45_0)
                    {
                        sqlConnection.Close();
                        throw arg_45_0;
                    }
                }
            }
            return result;
        }
        public object GetSingle(string SQLString, int Times)
        {
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(SQLString, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();
                        sqlCommand.CommandTimeout = Times;
                        object obj = sqlCommand.ExecuteScalar();
                        if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                        {
                            result = null;
                        }
                        else
                        {
                            result = obj;
                        }
                    }
                    catch (SqlException arg_4C_0)
                    {
                        sqlConnection.Close();
                        throw arg_4C_0;
                    }
                }
            }
            return result;
        }
        public SqlDataReader ExecuteReader(string strSQL)
        {
            SqlConnection sqlConnection = new SqlConnection(this.connectionString);
            SqlCommand sqlCommand = new SqlCommand(strSQL, sqlConnection);
            SqlDataReader result;
            try
            {
                sqlConnection.Open();
                result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (SqlException arg_25_0)
            {
                throw arg_25_0;
            }
            return result;
        }
        public DataSet Query(string SQLString)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    sqlConnection.Open();
                    new SqlDataAdapter(SQLString, sqlConnection).Fill(dataSet, "ds");
                }
                catch (SqlException arg_2D_0)
                {
                    throw new Exception(arg_2D_0.Message);
                }
                result = dataSet;
            }
            return result;
        }
        public DataTable QueryByTablde(string SQLString)
        {
            DataTable result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                DataTable dataTable = new DataTable();
                try
                {
                    sqlConnection.Open();
                    new SqlDataAdapter(SQLString, sqlConnection).Fill(dataTable);
                }
                catch (SqlException arg_28_0)
                {
                    throw new Exception(arg_28_0.Message);
                }
                result = dataTable;
            }
            return result;
        }
        public DataSet Query(string SQLString, int Times)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                DataSet dataSet = new DataSet();
                try
                {
                    sqlConnection.Open();
                    new SqlDataAdapter(SQLString, sqlConnection)
                    {
                        SelectCommand =
                        {
                            CommandTimeout = Times
                        }
                    }.Fill(dataSet, "ds");
                }
                catch (SqlException arg_39_0)
                {
                    throw new Exception(arg_39_0.Message);
                }
                result = dataSet;
            }
            return result;
        }
        public int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        SqlHelper.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                        int arg_2D_0 = sqlCommand.ExecuteNonQuery();
                        sqlCommand.Parameters.Clear();
                        result = arg_2D_0;
                    }
                    catch (SqlException arg_30_0)
                    {
                        throw arg_30_0;
                    }
                }
            }
            return result;
        }
        public void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        foreach (DictionaryEntry dictionaryEntry in SQLStringList)
                        {
                            string cmdText = dictionaryEntry.Key.ToString();
                            SqlParameter[] cmdParms = (SqlParameter[])dictionaryEntry.Value;
                            SqlHelper.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, cmdText, cmdParms);
                            sqlCommand.ExecuteNonQuery();
                            sqlCommand.Parameters.Clear();
                        }
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public int ExecuteSqlTran(List<SqlComm> cmdList)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        int num = 0;
                        foreach (SqlComm current in cmdList)
                        {
                            string commandText = current.CommandText;
                            SqlParameter[] cmdParms = (SqlParameter[])current.Parameters;
                            SqlHelper.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, commandText, cmdParms);
                            if (current.EffentNextType == EffentNextType.WhenHaveContine || current.EffentNextType == EffentNextType.WhenNoHaveContine)
                            {
                                if (current.CommandText.ToLower().IndexOf("count(") == -1)
                                {
                                    sqlTransaction.Rollback();
                                    result = 0;
                                    return result;
                                }
                                object obj = sqlCommand.ExecuteScalar();
                                if (obj != null || obj == DBNull.Value)
                                {
                                }
                                bool flag = Convert.ToInt32(obj) > 0;
                                if (current.EffentNextType == EffentNextType.WhenHaveContine && !flag)
                                {
                                    sqlTransaction.Rollback();
                                    result = 0;
                                    return result;
                                }
                                if (current.EffentNextType == EffentNextType.WhenNoHaveContine & flag)
                                {
                                    sqlTransaction.Rollback();
                                    result = 0;
                                    return result;
                                }
                            }
                            else
                            {
                                int num2 = sqlCommand.ExecuteNonQuery();
                                num += num2;
                                if (current.EffentNextType == EffentNextType.ExcuteEffectRows && num2 == 0)
                                {
                                    sqlTransaction.Rollback();
                                    result = 0;
                                    return result;
                                }
                                sqlCommand.Parameters.Clear();
                            }
                        }
                        sqlTransaction.Commit();
                        result = num;
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
            return result;
        }
        public void ExecuteSqlTranWithIndentity(List<SqlComm> SQLStringList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        int num = 0;
                        foreach (SqlComm expr_35 in SQLStringList)
                        {
                            string commandText = expr_35.CommandText;
                            SqlParameter[] array = (SqlParameter[])expr_35.Parameters;
                            SqlParameter[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter = array2[i];
                                if (sqlParameter.Direction == ParameterDirection.InputOutput)
                                {
                                    sqlParameter.Value = num;
                                }
                            }
                            SqlHelper.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, commandText, array);
                            sqlCommand.ExecuteNonQuery();
                            array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter2 = array2[i];
                                if (sqlParameter2.Direction == ParameterDirection.Output)
                                {
                                    num = Convert.ToInt32(sqlParameter2.Value);
                                }
                            }
                            sqlCommand.Parameters.Clear();
                        }
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void ExecuteSqlTranWithIndentity(Hashtable SQLStringList)
        {
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        int num = 0;
                        foreach (DictionaryEntry dictionaryEntry in SQLStringList)
                        {
                            string cmdText = dictionaryEntry.Key.ToString();
                            SqlParameter[] array = (SqlParameter[])dictionaryEntry.Value;
                            SqlParameter[] array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter = array2[i];
                                if (sqlParameter.Direction == ParameterDirection.InputOutput)
                                {
                                    sqlParameter.Value = num;
                                }
                            }
                            SqlHelper.PrepareCommand(sqlCommand, sqlConnection, sqlTransaction, cmdText, array);
                            sqlCommand.ExecuteNonQuery();
                            array2 = array;
                            for (int i = 0; i < array2.Length; i++)
                            {
                                SqlParameter sqlParameter2 = array2[i];
                                if (sqlParameter2.Direction == ParameterDirection.Output)
                                {
                                    num = Convert.ToInt32(sqlParameter2.Value);
                                }
                            }
                            sqlCommand.Parameters.Clear();
                        }
                        sqlTransaction.Commit();
                    }
                    catch
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {
            object result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        SqlHelper.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                        object obj = sqlCommand.ExecuteScalar();
                        sqlCommand.Parameters.Clear();
                        if (object.Equals(obj, null) || object.Equals(obj, DBNull.Value))
                        {
                            result = null;
                        }
                        else
                        {
                            result = obj;
                        }
                    }
                    catch (SqlException arg_4C_0)
                    {
                        throw arg_4C_0;
                    }
                }
            }
            return result;
        }
        public SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            SqlConnection conn = new SqlConnection(this.connectionString);
            SqlCommand sqlCommand = new SqlCommand();
            SqlDataReader result;
            try
            {
                SqlHelper.PrepareCommand(sqlCommand, conn, null, SQLString, cmdParms);
                SqlDataReader arg_2F_0 = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                sqlCommand.Parameters.Clear();
                result = arg_2F_0;
            }
            catch (SqlException arg_32_0)
            {
                throw arg_32_0;
            }
            return result;
        }
        public DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand();
                SqlHelper.PrepareCommand(sqlCommand, sqlConnection, null, SQLString, cmdParms);
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    DataSet dataSet = new DataSet();
                    try
                    {
                        sqlDataAdapter.Fill(dataSet, "ds");
                        sqlCommand.Parameters.Clear();
                    }
                    catch (SqlException arg_43_0)
                    {
                        throw new Exception(arg_43_0.Message);
                    }
                    result = dataSet;
                }
            }
            return result;
        }
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                for (int i = 0; i < cmdParms.Length; i++)
                {
                    SqlParameter sqlParameter = cmdParms[i];
                    if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
                    {
                        sqlParameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(sqlParameter);
                }
            }
        }
        public SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            SqlDataReader result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();
                SqlCommand expr_1A = SqlHelper.BuildQueryCommand(sqlConnection, storedProcName, parameters);
                expr_1A.CommandType = CommandType.StoredProcedure;
                result = expr_1A.ExecuteReader(CommandBehavior.CloseConnection);
            }
            return result;
        }
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                DataSet dataSet = new DataSet();
                sqlConnection.Open();
                new SqlDataAdapter
                {
                    SelectCommand = SqlHelper.BuildQueryCommand(sqlConnection, storedProcName, parameters)
                }.Fill(dataSet, tableName);
                sqlConnection.Close();
                result = dataSet;
            }
            return result;
        }
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            DataSet result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                DataSet dataSet = new DataSet();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter();
                sqlDa.SelectCommand = BuildQueryCommand(sqlConnection, storedProcName, parameters);
                sqlDa.SelectCommand.CommandTimeout = Times;
                sqlDa.Fill(dataSet, tableName);
                sqlConnection.Close();
                result = dataSet;
            }
            return result;
        }
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand sqlCommand = new SqlCommand(storedProcName, connection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < parameters.Length; i++)
            {
                SqlParameter sqlParameter = (SqlParameter)parameters[i];
                if (sqlParameter != null)
                {
                    if ((sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Input) && sqlParameter.Value == null)
                    {
                        sqlParameter.Value = DBNull.Value;
                    }
                    sqlCommand.Parameters.Add(sqlParameter);
                }
            }
            return sqlCommand;
        }
        public int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            int result;
            using (SqlConnection sqlConnection = new SqlConnection(this.connectionString))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = SqlHelper.BuildIntCommand(sqlConnection, storedProcName, parameters);
                rowsAffected = sqlCommand.ExecuteNonQuery();
                result = (int)sqlCommand.Parameters["ReturnValue"].Value;
            }
            return result;
        }
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand expr_08 = SqlHelper.BuildQueryCommand(connection, storedProcName, parameters);
            expr_08.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int, 4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return expr_08;
        }
    }
}
