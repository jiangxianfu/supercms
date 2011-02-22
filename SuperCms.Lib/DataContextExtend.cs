using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace SuperCms.Lib
{
    /// <summary>
    /// System.Data.Linq.DataContext 的自定义扩展
    /// </summary>
    public static class DataContextExtend
    {
        /// <summary>
        /// 从给定的Linq To SQL 查询获取 DataTable 对象 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static DataTable ExecDataTable(this System.Data.Linq.DataContext dc, IQueryable source)
        {
            //将 LinqToSQL查询传递给 GetCommand（）以获取DbCommand对象
            using (DbCommand cmd = dc.GetCommand(source))
            {
                //打开数据库链接，这里可以进一步扩展，比如传递进来自己定义的继承自 DbConnection 的对象
                if (dc.Connection.State == ConnectionState.Closed)
                    dc.Connection.Open();
                //声明 DataTable 对象
                DataTable dt = new DataTable();
                //调用DataTable 对象的 Load方法 ，从 DbDataReader 对象加载数据。
                DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(reader);
                dc.Connection.Close();
                reader.Close();
                return dt;
            }
        }
        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecNonQuery(this System.Data.Linq.DataContext dc, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            using (DbCommand cmd = dc.Connection.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                dc.Connection.Close();
                return val;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static DataTable ExecDataTable(this System.Data.Linq.DataContext dc, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            using (IDbCommand cmd = dc.Connection.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, commandParameters);

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader(CommandBehavior.CloseConnection));
                cmd.Parameters.Clear();
                dc.Connection.Close();
                return dt;
            }
        }
        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecScalar(this System.Data.Linq.DataContext dc, CommandType cmdType, string cmdText, params IDbDataParameter[] commandParameters)
        {
            using (IDbCommand cmd = dc.Connection.CreateCommand())
            {
                PrepareCommand(cmd, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                dc.Connection.Close();
                return val;
            }
        }
        public static IDbDataParameter CreateParameter(this System.Data.Linq.DataContext dc, string parameterName)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = parameterName;
            return p;
        }

        public static IDbDataParameter CreateParameter(this System.Data.Linq.DataContext dc, string parameterName, object value)
        {
            if (value == null)
            {
                value = (object)DBNull.Value;
            }
            SqlParameter p = new SqlParameter(parameterName, value);
            return p;
        }

        public static IDbDataParameter CreateParameter(this System.Data.Linq.DataContext dc, string parameterName, SqlDbType dbType, int size, object value)
        {
            if (value == null)
            {
                value = (object)DBNull.Value;
            }
            SqlParameter p = new SqlParameter(parameterName, dbType, size);
            p.Value = value;
            return p;
        }

        public static IDbDataParameter CreateParameter(this System.Data.Linq.DataContext dc, string parameterName, SqlDbType dbType, object value)
        {
            if (value == null)
            {
                value = (object)DBNull.Value;
            }

            SqlParameter p = new SqlParameter(parameterName, dbType);
            p.Value = value;
            return p;
        }

        public static void SetParameter(this System.Data.Linq.DataContext dc, IDataParameter parameter, object value)
        {
            if (value == null)
            {
                value = (object)DBNull.Value;
            }
            parameter.Value = value;
        }

        /// <summary>
        ///  Prepare a command for execution
        /// </summary>
        /// <param name="cmd">IDbCommand Object</param>
        /// <param name="trans">IDbTransaction Object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">IDbDataParameters to use in the command</param>
        private static void PrepareCommand(IDbCommand cmd, CommandType cmdType, string cmdText, IDbDataParameter[] cmdParms)
        {
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            if (cmdParms != null && cmdParms.Length > 0)
            {
                foreach (IDbDataParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
    }
}
