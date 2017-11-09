using System;
using System.Data;
using System.Data.OleDb;
using System.Xml;


namespace CTDDJYDS.DatabaseCommon
{
		/// <summary>
	/// 数据连接对象类---仅连接access数据库
	/// </summary>
	public class ProOleDataSource:ProDataSource
	{
		private OleDbConnection	_connection;
		private OleDbTransaction _transaction;

        public ProOleDataSource(string dataSourceName, string userName, string password)
        {
            string conString = "";
            if (string.IsNullOrEmpty(userName))
                conString = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source=" + dataSourceName ;
            else
                conString = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source=" + dataSourceName + ";User ID=" + userName ;
            if (!string.IsNullOrEmpty(password))
                conString = string.Format("{0};Persist Security Info=true;Jet OLEDB:Database Password={1}", conString, password);

            this._connection = new OleDbConnection(conString);
            this.Open();
            this._transaction = null;
        }

        /// <param name="connectionString">规范的连接串</param>
		public ProOleDataSource(string connectionString)
		{
			this._connection	= new  OleDbConnection(connectionString);
            this.Open();
			this._transaction   = null;
		}
        ~ProOleDataSource()
        {
            this._transaction = null;
            //if (this._connection !=null && this._connection.State== ConnectionState.Open )
            //    this._connection.Close();
            this._connection = null;
        }
        public string DataSourceName
        {
            get { return this._connection.DataSource; }
        }
		/// <summary>
		/// 得到数据库的连接信息
		/// </summary>
		public override IDbConnection  Connection
		{
			get
			{
				return this._connection;
			}
		}

		/// <summary>
		/// 公开事务连接操作
		/// </summary>
		public override IDbTransaction Transaction
		{
			get
			{
				return this._transaction;
			}
		}

		public override ConnectionState State
		{
			get
			{
				return this._connection.State;
			}
		}
		public override  void Open()
		{
			if(this._connection.State != ConnectionState.Open)
				this._connection.Open();

		}

		public override  void Close()
		{
            if (this._connection !=null && this._connection.State == ConnectionState.Open)
				this._connection.Close();			
		}

		public override void BeginTransaction()
		{
			this._transaction = this._connection.BeginTransaction();
		}

		public override  void CommitTransaction()
		{
			this._transaction.Commit();
		}

		public override  void RollBackTransaction()
		{
			this._transaction.Rollback();
		}
		public override DatabasePlatformType instDataSourceType() 
		{
			return DatabasePlatformType.DB_OLEDB_Access;
		}

		#region ExecuteNonQuery
		/// <summary>
		/// 执行命令，但不返回任何结果
		/// </summary>
		/// <param name="commandText"></param>
		/// <returns></returns>
		public override int ExecuteNonQuery(string commandText)
		{
           
            OleDbCommand command = new OleDbCommand(commandText, this._connection, this._transaction);
            int i = 0;
            try
            {
                i = command.ExecuteNonQuery();
            }
            finally
            {
                command.Dispose();
                command = null;
            }
            return i;
		}
		#endregion

		#region ExecuteReader
		/// <summary>
		/// 执行命令，返回一个类型化的IDataReader
		/// </summary>
		/// <param name="commandText"></param>
		/// <returns></returns>
		public override IDataReader  ExecuteReader(string commandText)
		{
            
            OleDbCommand command = new OleDbCommand(commandText, this._connection, this._transaction);
            IDataReader v = null;
            try
            {
                v = command.ExecuteReader();
            }
            finally
            {
                command.Dispose();
                command = null;
            }
            return v;
		}
		#endregion
		#region ExecuteScalar
		/// <summary>
		/// 执行命令，返回一个值
		/// </summary>
		/// <param name="commandText"></param>
		/// <returns></returns>
		public override  object ExecuteScalar(string commandText)
		{
            
            OleDbCommand command = new OleDbCommand(commandText, this._connection, this._transaction);
            object v = null;
            try
            {
                v = command.ExecuteScalar();
            }
            finally 
            {
                command.Dispose();
                command = null;
            }

            return v;
		}
		#endregion
		#region GetServerTime
		public override  DateTime GetServerTime()
		{
			DateTime dtnServerTime;
			dtnServerTime=(DateTime)DateTime.Now;
			return dtnServerTime;
		}
		#endregion
		#region GetNewID

		public override int GetNewID(string SeqName)
		{
			int intNewid=GetNewID(SeqName,1);
			return intNewid;
		}
		public override  int  GetNewID(string SeqName,int Count)
		{
			return 0;			
		}
		#endregion

        #region  实现虚拟方法
        public override bool ExistField(string table, string field)
        {
            //判断管线表中是否存在要插入的字段 
            OleDbDataAdapter adapter = null;
            DataSet dataset = new DataSet();
            try
            {
                string sql = string.Format("select * from {0} where 1=0 ", table);
                
                adapter = new OleDbDataAdapter(sql, this._connection);
                adapter.Fill(dataset, table);
                return dataset.Tables[0].Columns.Contains(field);
            }
            catch (System.Exception ex)
            {
                throw ;
            }
            finally
            {
                dataset.Dispose ();
                dataset=null ;
                adapter.Dispose();
                adapter = null;
            }
        }
        public override bool ExistTable(string TABLESPACE_NAME, string TABLE_NAME)
        {
            TABLE_NAME = TABLE_NAME.ToUpper();
            DataTable dt = (this.Connection as OleDbConnection).GetSchema("Tables");
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    string name = row["table_name"].ToString().ToUpper();
                    string type = row["table_type"].ToString().ToUpper();
                    if (type != "TABLE")
                        continue;
                    if (name == TABLE_NAME)
                        return true;
                }
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            
            return false;
        }
        #endregion
        public override DataTable SelectBySql(string strSql)
        {
            DataTable datatable = new DataTable();
            OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
            oleAdapter.SelectCommand = new OleDbCommand(strSql, (OleDbConnection)this.Connection, (OleDbTransaction)this.Transaction);
            oleAdapter.Fill(datatable);
            return datatable;
        }
        public override System.Data.IDbCommand CreateDbCommand(string commandText)
        {
            return new OleDbCommand(commandText, this.Connection as OleDbConnection, this.Transaction  as OleDbTransaction) as System.Data.IDbCommand;
        }
        public override System.Data.IDbDataAdapter CreateDbDataAdapter()
        {
            return new OleDbDataAdapter() as System.Data.Common.DbDataAdapter;
        }
        public override System.Data.Common.DbCommandBuilder CreateDbCommandBuilder(System.Data.Common.DbDataAdapter adapter)
        {
            return new OleDbCommandBuilder(adapter as OleDbDataAdapter);
        }

        public override string UserName
        {
            get { return ""; }
        }
        public override string Service
        {
            get { return this._connection.DataSource; }
        }
	}
}
