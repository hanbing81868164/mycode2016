using System.Data;
using System.Data.Common;
using System.Collections;

namespace System
{
    public class DataAccess : IDisposable
    {

        public DataAccess(string connectionName)
        {
            var config = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName];
            ConnectionString = config.ConnectionString;
            ProviderName = config.ProviderName;
        }

        /// <param name="configString">app.config 关键字</param>   
        public DataAccess(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;
        }

        /// <summary>   
        /// 属性,设置数据库连接字符串   
        /// </summary>   
        public string ConnectionString
        {
            get;
            set;
        }

        public string ProviderName { get; set; }

        //===========================================GetDbproviderFactory========================  

        #region 返回数据工厂  public DbProviderFactory GetDbProviderFactory()
        /// <summary>   
        /// 返回数据工厂   
        /// </summary>   
        /// <returns></returns>   
        private DbProviderFactory GetDbProviderFactory()
        {
            DbProviderFactory f = GetDbProviderFactory(this.ProviderName);
            return f;
        }

        /// <summary>   
        /// 返回数据工厂   
        /// </summary>   
        /// <param name="providername"></param>   
        /// <returns></returns>   
        private DbProviderFactory GetDbProviderFactory(string providername)
        {
            return DbProviderFactories.GetFactory(providername);
        }
        #endregion

        //===========================================CreateConnection============================  

        #region 创建数据库连接 public DbConnection CreateConnection()
        /// <summary>   
        /// 创建数据库连接   
        /// </summary>   
        /// <returns></returns>   
        private DbConnection CreateConnection()
        {
            DbConnection con = GetDbProviderFactory().CreateConnection();
            con.ConnectionString = ConnectionString;

            return con;
        }
        /// <summary>   
        /// 创建数据库连接   
        /// </summary>   
        /// <param name="provdername"></param>   
        /// <returns></returns>   
        private DbConnection CreateConnection(string provdername)
        {
            DbConnection con = GetDbProviderFactory(provdername).CreateConnection();
            con.ConnectionString = ConnectionString;

            return con;

        }
        #endregion

        //===========================================CreateCommand===============================  

        #region 创建执行命令对象 public override DbCommand CreateCommand(string sql, CommandType cmdType, DbParameter[] parameters)
        /// <summary>   
        /// 创建执行命令对象   
        /// </summary>   
        /// <param name="sql"></param>   
        /// <param name="cmdType"></param>   
        /// <param name="parameters"></param>   
        /// <returns></returns>   
        private DbCommand CreateCommand(string sql, CommandType cmdType, DbParameter[] parameters)
        {
            DbCommand _command = GetDbProviderFactory().CreateCommand();
            _command.Connection = CreateConnection();
            _command.CommandText = sql;
            _command.CommandType = cmdType;
            if (parameters != null && parameters.Length > 0)
            {
                foreach (DbParameter param in parameters)
                {
                    _command.Parameters.Add(param);
                }
            }
            return _command;
        }

        /// <summary>   
        /// 创建执行命令对象   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <returns>执行命令对象实例</returns>   
        private DbCommand CreateCommand(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateCommand(sql, CommandType.Text, parameters);
        }
        /// <summary>   
        /// 创建执行命令对象   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <returns>执行命令对象实例</returns>   
        private DbCommand CreateCommand(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateCommand(sql, cmdtype, parameters);
        }
        /// <summary>   
        /// 创建执行命令对象   
        /// </summary>   
        /// <param name="sql">ＳＱＬ语句</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>执行命令对象实例</returns>   
        private DbCommand CreateCommand(string sql, DbParameter[] parameters)
        {
            return CreateCommand(sql, CommandType.Text, parameters);
        }
        #endregion

        //===========================================CreateAdapter()=============================  

        #region 创建数据适配器 CreateAdapter(string sql)
        /// <summary>   
        /// 创建数据适配器   
        /// </summary>   
        /// <param name="sql">SQL,语句</param>   
        /// <returns>数据适配器实例</returns>   
        private DbDataAdapter CreateAdapter(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateAdapter(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 创建数据适配器   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <returns>数据适配器实例</returns>   
        private DbDataAdapter CreateAdapter(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateAdapter(sql, cmdtype, parameters);
        }
        /// <summary>   
        /// 创建数据适配器   
        /// </summary>   
        /// <param name="connectionString">数据库连接字符串</param>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>数据适配器实例</returns>   
        private DbDataAdapter CreateAdapter(string sql, CommandType cmdtype, DbParameter[] parameters)
        {
            DbConnection _connection = CreateConnection();
            DbCommand _command = GetDbProviderFactory().CreateCommand();
            _command.Connection = _connection;
            _command.CommandText = sql;
            _command.CommandType = cmdtype;
            if (parameters != null && parameters.Length > 0)
            {
                foreach (DbParameter _param in parameters)
                {
                    _command.Parameters.Add(_param);
                }
            }
            DbDataAdapter da = GetDbProviderFactory().CreateDataAdapter();
            da.SelectCommand = _command;

            return da;
        }

        #endregion

        //===========================================CreateParameter=============================  

        #region 生成参数 public override SqlParameter CreateParameter(string field, string dbtype, string value)
        /// <summary>   
        /// 创建参数   
        /// </summary>   
        /// <param name="field">参数字段</param>   
        /// <param name="dbtype">参数类型</param>   
        /// <param name="value">参数值</param>   
        /// <returns></returns>   
        public DbParameter CreateParameter(string field, string dbtype, string value)
        {
            DbParameter p = GetDbProviderFactory().CreateParameter();
            p.ParameterName = field;
            p.Value = value;
            return p;
        }

        public DbParameter CreateParameter(string field, DbType dbType, object value)
        {
            DbParameter p = GetDbProviderFactory().CreateParameter();
            p.ParameterName = field;
            p.DbType = dbType;
            p.Value = value;
            return p;
        }

        #endregion

        //===========================================ExecuteCommand()============================  

        #region 执行非查询语句,并返回受影响的记录行数 ExecuteCommand(string sql)
        /// <summary>   
        /// 执行非查询语句,并返回受影响的记录行数   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <returns>受影响记录行数</returns>   
        public int ExecuteCommand(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行非查询语句,并返回受影响的记录行数   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <returns>受影响记录行数</returns>   
        public int ExecuteCommand(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行非查询语句,并返回受影响的记录行数   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>受影响记录行数</returns>   
        public int ExecuteCommand(string sql, DbParameter[] parameters)
        {
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        ///批量执行SQL语句    
        /// </summary>   
        /// <param name="SqlList">SQL列表</param>   
        /// <returns></returns>   
        public bool ExecuteCommand(ArrayList SqlList)
        {
            DbConnection con = CreateConnection();
            con.Open();
            bool iserror = false;
            string strerror = "";
            DbTransaction SqlTran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < SqlList.Count; i++)
                {

                    DbCommand _command = GetDbProviderFactory().CreateCommand();
                    _command.Connection = con;
                    _command.CommandText = SqlList[i].ToString();
                    _command.Transaction = SqlTran;
                    _command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                iserror = true;
                strerror = ex.Message;

            }
            finally
            {

                if (iserror)
                {
                    SqlTran.Rollback();
                    throw new Exception(strerror);
                }
                else
                {
                    SqlTran.Commit();
                }
                con.Close();
            }
            if (iserror)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>   
        /// 执行非查询语句,并返回受影响的记录行数   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>受影响记录行数</returns>   
        public int ExecuteCommand(string sql, CommandType cmdtype, DbParameter[] parameters)
        {
            int _result = 0;
            DbCommand _command = CreateCommand(sql, cmdtype, parameters);
            try
            {
                _command.Connection.Open();
                _result = _command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _command.Connection.Close();
            }
            return _result;
        }


        #endregion

        //===========================================ExecuteScalar()=============================  

        #region 执行非查询语句,并返回首行首列的值 ExecuteScalar(string sql)

        /// <summary>   
        /// 执行非查询语句,并返回首行首列的值   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <returns>Object</returns>   
        public object ExecuteScalar(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行非查询语句,并返回首行首列的值   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <returns>Object</returns>   
        public object ExecuteScalar(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行非查询语句,并返回首行首列的值   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>Object</returns>   
        public object ExecuteScalar(string sql, DbParameter[] parameters)
        {
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行非查询语句,并返回首行首列的值   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>Object</returns>   
        public object ExecuteScalar(string sql, CommandType cmdtype, DbParameter[] parameters)
        {
            object _result = null;
            DbCommand _command = CreateCommand(sql, cmdtype, parameters);
            try
            {
                _command.Connection.Open();
                _result = _command.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                _command.Connection.Close();
            }
            return _result;
        }
        #endregion

        //===========================================ExecuteReader()=============================  

        #region 执行查询，并以DataReader返回结果集  ExecuteReader(string sql)
        /// <summary>   
        /// 执行查询，并以DataReader返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <returns>IDataReader</returns>   
        public DbDataReader ExecuteReader(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行查询，并以DataReader返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <returns>IDataReader</returns>   
        public DbDataReader ExecuteReader(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行查询，并以DataReader返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>IDataReader</returns>   
        public DbDataReader ExecuteReader(string sql, DbParameter[] parameters)
        {
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行查询，并以DataReader返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>IDataReader</returns>   
        public DbDataReader ExecuteReader(string sql, CommandType cmdtype, DbParameter[] parameters)
        {
            DbDataReader _result;
            DbCommand _command = CreateCommand(sql, cmdtype, parameters);
            try
            {
                _command.Connection.Open();
                _result = _command.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                throw;
            }
            finally
            {

            }
            return _result;
        }
        #endregion

        //===========================================GetDataSet()================================  

        #region 执行查询，并以DataSet返回结果集 GetDataSet(string sql)
        /// <summary>   
        /// 执行查询，并以DataSet返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <returns>DataSet</returns>   
        public DataSet GetDataSet(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行查询，并以DataSet返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <returns>DataSet</returns>   
        public virtual DataSet GetDataSet(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行查询，并以DataSet返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataSet</returns>   
        public virtual DataSet GetDataSet(string sql, DbParameter[] parameters)
        {
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// 执行查询，并以DataSet返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataSet</returns>   
        public virtual DataSet GetDataSet(string sql, CommandType cmdtype, DbParameter[] parameters)
        {
            DataSet _result = new DataSet();
            IDataAdapter _dataAdapter = CreateAdapter(sql, cmdtype, parameters);
            try
            {
                _dataAdapter.Fill(_result);
            }
            catch
            {
                throw;
            }
            finally
            {
            }
            return _result;
        }
        /// <summary>   
        /// 执行查询,并以DataSet返回指定记录的结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="StartIndex">开始索引</param>   
        /// <param name="RecordCount">显示记录</param>   
        /// <returns>DataSet</returns>   
        public virtual DataSet GetDataSet(string sql, int StartIndex, int RecordCount)
        {
            return GetDataSet(sql, StartIndex, RecordCount);
        }

        #endregion

        //===========================================GetDataView()===============================  

        #region 执行查询，并以DataView返回结果集   GetDataView(string sql)

        /// <summary>   
        /// 执行查询，并以DataView返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataView dv = GetDataSet(sql, CommandType.Text, parameters).Tables[0].DefaultView;
            return dv;
        }
        /// <summary>   
        /// 执行查询，并以DataView返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataView dv = GetDataSet(sql, cmdtype, parameters).Tables[0].DefaultView;
            return dv;
        }
        /// <summary>   
        /// 执行查询，并以DataView返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql, DbParameter[] parameters)
        {

            DataView dv = GetDataSet(sql, CommandType.Text, parameters).Tables[0].DefaultView;
            return dv;
        }

        /// <summary>   
        /// 执行查询，并以DataView返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql, CommandType cmdtype, DbParameter[] parameters)
        {
            DataView dv = GetDataSet(sql, cmdtype, parameters).Tables[0].DefaultView;
            return dv;
        }

        /// <summary>   
        /// 执行查询,并以DataView返回指定记录的结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="StartIndex">开始索引</param>   
        /// <param name="RecordCount">显示记录</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql, int StartIndex, int RecordCount)
        {
            return GetDataSet(sql, StartIndex, RecordCount).Tables[0].DefaultView;
        }
        #endregion

        //===========================================GetDataTable()==============================  

        #region 执行查询，并以DataTable返回结果集   GetDataTable(string sql)

        /// <summary>   
        /// 执行查询，并以DataTable返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataTable dt = GetDataSet(sql, CommandType.Text, parameters).Tables[0];
            return dt;
        }
        /// <summary>   
        /// 执行查询，并以DataTable返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataTable dt = GetDataSet(sql, cmdtype, parameters).Tables[0];
            return dt;
        }
        /// <summary>   
        /// 执行查询，并以DataTable返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql, DbParameter[] parameters)
        {

            DataTable dt = GetDataSet(sql, CommandType.Text, parameters).Tables[0];
            return dt;
        }

        /// <summary>   
        /// 执行查询，并以DataTable返回结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="cmdtype">命令类型</param>   
        /// <param name="parameters">参数</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql, CommandType cmdtype, DbParameter[] parameters)
        {
            DataTable dt = GetDataSet(sql, cmdtype, parameters).Tables[0];
            return dt;
        }

        /// <summary>   
        /// 执行查询,并以DataTable返回指定记录的结果集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="StartIndex">开始索引</param>   
        /// <param name="RecordCount">显示记录</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql, int StartIndex, int RecordCount)
        {
            return GetDataSet(sql, StartIndex, RecordCount).Tables[0];
        }

        /// <summary>   
        /// 执行查询,返回以空行填充的指定条数记录集   
        /// </summary>   
        /// <param name="sql">SQL语句</param>   
        /// <param name="SizeCount">显示记录条数</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql, int SizeCount)
        {
            DataTable dt = GetDataSet(sql).Tables[0];
            int b = SizeCount - dt.Rows.Count;
            if (dt.Rows.Count < SizeCount)
            {
                for (int i = 0; i < b; i++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        #endregion

        public void Dispose()
        {
            GC.Collect();
        }
    }
}