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

        /// <param name="configString">app.config �ؼ���</param>   
        public DataAccess(string connectionString, string providerName)
        {
            ConnectionString = connectionString;
            ProviderName = providerName;
        }

        /// <summary>   
        /// ����,�������ݿ������ַ���   
        /// </summary>   
        public string ConnectionString
        {
            get;
            set;
        }

        public string ProviderName { get; set; }

        //===========================================GetDbproviderFactory========================  

        #region �������ݹ���  public DbProviderFactory GetDbProviderFactory()
        /// <summary>   
        /// �������ݹ���   
        /// </summary>   
        /// <returns></returns>   
        private DbProviderFactory GetDbProviderFactory()
        {
            DbProviderFactory f = GetDbProviderFactory(this.ProviderName);
            return f;
        }

        /// <summary>   
        /// �������ݹ���   
        /// </summary>   
        /// <param name="providername"></param>   
        /// <returns></returns>   
        private DbProviderFactory GetDbProviderFactory(string providername)
        {
            return DbProviderFactories.GetFactory(providername);
        }
        #endregion

        //===========================================CreateConnection============================  

        #region �������ݿ����� public DbConnection CreateConnection()
        /// <summary>   
        /// �������ݿ�����   
        /// </summary>   
        /// <returns></returns>   
        private DbConnection CreateConnection()
        {
            DbConnection con = GetDbProviderFactory().CreateConnection();
            con.ConnectionString = ConnectionString;

            return con;
        }
        /// <summary>   
        /// �������ݿ�����   
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

        #region ����ִ��������� public override DbCommand CreateCommand(string sql, CommandType cmdType, DbParameter[] parameters)
        /// <summary>   
        /// ����ִ���������   
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
        /// ����ִ���������   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <returns>ִ���������ʵ��</returns>   
        private DbCommand CreateCommand(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateCommand(sql, CommandType.Text, parameters);
        }
        /// <summary>   
        /// ����ִ���������   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <returns>ִ���������ʵ��</returns>   
        private DbCommand CreateCommand(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateCommand(sql, cmdtype, parameters);
        }
        /// <summary>   
        /// ����ִ���������   
        /// </summary>   
        /// <param name="sql">�ӣѣ����</param>   
        /// <param name="parameters">����</param>   
        /// <returns>ִ���������ʵ��</returns>   
        private DbCommand CreateCommand(string sql, DbParameter[] parameters)
        {
            return CreateCommand(sql, CommandType.Text, parameters);
        }
        #endregion

        //===========================================CreateAdapter()=============================  

        #region �������������� CreateAdapter(string sql)
        /// <summary>   
        /// ��������������   
        /// </summary>   
        /// <param name="sql">SQL,���</param>   
        /// <returns>����������ʵ��</returns>   
        private DbDataAdapter CreateAdapter(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateAdapter(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ��������������   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <returns>����������ʵ��</returns>   
        private DbDataAdapter CreateAdapter(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return CreateAdapter(sql, cmdtype, parameters);
        }
        /// <summary>   
        /// ��������������   
        /// </summary>   
        /// <param name="connectionString">���ݿ������ַ���</param>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>����������ʵ��</returns>   
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

        #region ���ɲ��� public override SqlParameter CreateParameter(string field, string dbtype, string value)
        /// <summary>   
        /// ��������   
        /// </summary>   
        /// <param name="field">�����ֶ�</param>   
        /// <param name="dbtype">��������</param>   
        /// <param name="value">����ֵ</param>   
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

        #region ִ�зǲ�ѯ���,��������Ӱ��ļ�¼���� ExecuteCommand(string sql)
        /// <summary>   
        /// ִ�зǲ�ѯ���,��������Ӱ��ļ�¼����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <returns>��Ӱ���¼����</returns>   
        public int ExecuteCommand(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�зǲ�ѯ���,��������Ӱ��ļ�¼����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <returns>��Ӱ���¼����</returns>   
        public int ExecuteCommand(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�зǲ�ѯ���,��������Ӱ��ļ�¼����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="parameters">����</param>   
        /// <returns>��Ӱ���¼����</returns>   
        public int ExecuteCommand(string sql, DbParameter[] parameters)
        {
            return ExecuteCommand(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        ///����ִ��SQL���    
        /// </summary>   
        /// <param name="SqlList">SQL�б�</param>   
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
        /// ִ�зǲ�ѯ���,��������Ӱ��ļ�¼����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>��Ӱ���¼����</returns>   
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

        #region ִ�зǲ�ѯ���,�������������е�ֵ ExecuteScalar(string sql)

        /// <summary>   
        /// ִ�зǲ�ѯ���,�������������е�ֵ   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <returns>Object</returns>   
        public object ExecuteScalar(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�зǲ�ѯ���,�������������е�ֵ   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <returns>Object</returns>   
        public object ExecuteScalar(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�зǲ�ѯ���,�������������е�ֵ   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="parameters">����</param>   
        /// <returns>Object</returns>   
        public object ExecuteScalar(string sql, DbParameter[] parameters)
        {
            return ExecuteScalar(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�зǲ�ѯ���,�������������е�ֵ   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
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

        #region ִ�в�ѯ������DataReader���ؽ����  ExecuteReader(string sql)
        /// <summary>   
        /// ִ�в�ѯ������DataReader���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <returns>IDataReader</returns>   
        public DbDataReader ExecuteReader(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�в�ѯ������DataReader���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <returns>IDataReader</returns>   
        public DbDataReader ExecuteReader(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�в�ѯ������DataReader���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="parameters">����</param>   
        /// <returns>IDataReader</returns>   
        public DbDataReader ExecuteReader(string sql, DbParameter[] parameters)
        {
            return ExecuteReader(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�в�ѯ������DataReader���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
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

        #region ִ�в�ѯ������DataSet���ؽ���� GetDataSet(string sql)
        /// <summary>   
        /// ִ�в�ѯ������DataSet���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <returns>DataSet</returns>   
        public DataSet GetDataSet(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�в�ѯ������DataSet���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <returns>DataSet</returns>   
        public virtual DataSet GetDataSet(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�в�ѯ������DataSet���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="parameters">����</param>   
        /// <returns>DataSet</returns>   
        public virtual DataSet GetDataSet(string sql, DbParameter[] parameters)
        {
            return GetDataSet(sql, CommandType.Text, parameters);
        }

        /// <summary>   
        /// ִ�в�ѯ������DataSet���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
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
        /// ִ�в�ѯ,����DataSet����ָ����¼�Ľ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="StartIndex">��ʼ����</param>   
        /// <param name="RecordCount">��ʾ��¼</param>   
        /// <returns>DataSet</returns>   
        public virtual DataSet GetDataSet(string sql, int StartIndex, int RecordCount)
        {
            return GetDataSet(sql, StartIndex, RecordCount);
        }

        #endregion

        //===========================================GetDataView()===============================  

        #region ִ�в�ѯ������DataView���ؽ����   GetDataView(string sql)

        /// <summary>   
        /// ִ�в�ѯ������DataView���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataView dv = GetDataSet(sql, CommandType.Text, parameters).Tables[0].DefaultView;
            return dv;
        }
        /// <summary>   
        /// ִ�в�ѯ������DataView���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataView dv = GetDataSet(sql, cmdtype, parameters).Tables[0].DefaultView;
            return dv;
        }
        /// <summary>   
        /// ִ�в�ѯ������DataView���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql, DbParameter[] parameters)
        {

            DataView dv = GetDataSet(sql, CommandType.Text, parameters).Tables[0].DefaultView;
            return dv;
        }

        /// <summary>   
        /// ִ�в�ѯ������DataView���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql, CommandType cmdtype, DbParameter[] parameters)
        {
            DataView dv = GetDataSet(sql, cmdtype, parameters).Tables[0].DefaultView;
            return dv;
        }

        /// <summary>   
        /// ִ�в�ѯ,����DataView����ָ����¼�Ľ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="StartIndex">��ʼ����</param>   
        /// <param name="RecordCount">��ʾ��¼</param>   
        /// <returns>DataView</returns>   
        public DataView GetDataView(string sql, int StartIndex, int RecordCount)
        {
            return GetDataSet(sql, StartIndex, RecordCount).Tables[0].DefaultView;
        }
        #endregion

        //===========================================GetDataTable()==============================  

        #region ִ�в�ѯ������DataTable���ؽ����   GetDataTable(string sql)

        /// <summary>   
        /// ִ�в�ѯ������DataTable���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataTable dt = GetDataSet(sql, CommandType.Text, parameters).Tables[0];
            return dt;
        }
        /// <summary>   
        /// ִ�в�ѯ������DataTable���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql, CommandType cmdtype)
        {
            DbParameter[] parameters = new DbParameter[0];
            DataTable dt = GetDataSet(sql, cmdtype, parameters).Tables[0];
            return dt;
        }
        /// <summary>   
        /// ִ�в�ѯ������DataTable���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql, DbParameter[] parameters)
        {

            DataTable dt = GetDataSet(sql, CommandType.Text, parameters).Tables[0];
            return dt;
        }

        /// <summary>   
        /// ִ�в�ѯ������DataTable���ؽ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="cmdtype">��������</param>   
        /// <param name="parameters">����</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql, CommandType cmdtype, DbParameter[] parameters)
        {
            DataTable dt = GetDataSet(sql, cmdtype, parameters).Tables[0];
            return dt;
        }

        /// <summary>   
        /// ִ�в�ѯ,����DataTable����ָ����¼�Ľ����   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="StartIndex">��ʼ����</param>   
        /// <param name="RecordCount">��ʾ��¼</param>   
        /// <returns>DataTable</returns>   
        public DataTable GetDataTable(string sql, int StartIndex, int RecordCount)
        {
            return GetDataSet(sql, StartIndex, RecordCount).Tables[0];
        }

        /// <summary>   
        /// ִ�в�ѯ,�����Կ�������ָ��������¼��   
        /// </summary>   
        /// <param name="sql">SQL���</param>   
        /// <param name="SizeCount">��ʾ��¼����</param>   
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