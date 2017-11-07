using System;
using System.Data;
using System.Data.OracleClient;
using System.Xml;

namespace CTDDJYDS.DatabaseCommon
{
    /// <summary>
    /// 必须安装Oracle客户端,建议Oracle的版本小于11使用此种方式
    /// </summary>
	public class ProOracleClientDataSource : ProDataSource
	{
		private OracleConnection _connection;
		private OracleTransaction _transaction;

        
		public ProOracleClientDataSource(string connectionString)
		{
			this._connection	= new OracleConnection(connectionString);
			this._transaction   = null;
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
			if(this._connection.State == ConnectionState.Open)
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
			return DatabasePlatformType.DB_Oracle;
		}
		#region ExecuteNonQuery
		/// <summary>
		/// 执行命令，但不返回任何结果
		/// </summary>
		/// <param name="commandText"></param>
		/// <returns></returns>
		public override int ExecuteNonQuery(string commandText)
		{
            if (this._connection.State == ConnectionState.Closed)
                this.Open();
            OracleCommand command = new OracleCommand(commandText, this._connection, this._transaction);
            return command.ExecuteNonQuery();
           
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
            
            OracleCommand command = new OracleCommand(commandText, this._connection, this._transaction);
            return command.ExecuteReader();
            
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
            
            OracleCommand command = new OracleCommand(commandText, this._connection, this._transaction);
            object v = null;
            try
            {
                v = command.ExecuteOracleScalar();
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
			DateTime DtmServerdate=DateTime.Now ;
			OracleDataReader  objRead=null;			
			
			string strQuerySql="";
			
			strQuerySql="SELECT SYSDATE FROM DUAL";
		
			try
			{
				objRead=(OracleDataReader)ExecuteReader(strQuerySql);
			}
			catch (OracleException  ex)
			{
				if (objRead!=null)
				{
					objRead.Close();
				}
				throw new Exception("GetServerTime失败!",ex); 
			}
			objRead.Read();
			DtmServerdate=objRead.GetDateTime(0);
			
			if (objRead!=null)
			{
				objRead.Close();
			}
			return DtmServerdate;
		}
		#endregion

		#region GetNewID 获取数据库序列值

		public override int GetNewID(string SeqName)
		{
			int intNewid=GetNewID(SeqName,1);
			return intNewid;
		}
		public override  int  GetNewID(string SeqName,int Count)
		{
			int intNewid=0;			
			string strCreateSql="";
			string strQuerySql="";
			OracleDataReader  objRead=null;
			
			strQuerySql="Select TO_NUMBER(" + SeqName + ".NEXTVAL) From dual";

			right:
				try
				{
			
					objRead=(OracleDataReader)ExecuteReader(strQuerySql);
				}
				catch ( OracleException ex)
				{
					if (objRead!=null)
					{
						objRead.Close();
					}
					if (ex.Code == -2147217900)
					
					{
						
						strCreateSql = "CREATE SEQUENCE " + SeqName + " INCREMENT BY 1 START WITH 1 MINVALUE 1 NOCYCLE NOCACHE NOORDER";
						
						
						ExecuteNonQuery(strCreateSql);
					
						goto right;
					}
					else
					{
						throw new Exception("GetNewID失败!",ex); 
					}
				
				}
		
			objRead.Read();
			intNewid=Convert.ToInt32(objRead.GetValue(0));
			
			if (objRead!=null)
			{
				objRead.Close();
			}
			return intNewid;
		}

		#endregion

        public override bool  ExistTable(string TABLESPACE_NAME, string TABLE_NAME)
        {
            string strSql =string.Format( "select   count(*)  from   sys.user_tables   where   upper(TABLE_NAME )  =  '{0}' and upper(TABLESPACE_NAME)='{1}'",TABLE_NAME.ToUpper(),TABLESPACE_NAME.ToUpper() );
            if ( ((OracleNumber)this.ExecuteScalar(strSql)).Value >0 )
                return true;
            else 
                return false ;
        }
        public override bool  ExistField(string table, string field)
        {
            string strSql = "select count(*) from sys.User_Tab_Columns where upper(table_name) = '" + table.ToUpper() + "' and upper(COLUMN_NAME) = '" + field.ToUpper () + "'";
            if (((OracleNumber)this.ExecuteScalar(strSql)).Value > 0)
                return true;
            else
                return false;
        }
        public override DataTable SelectBySql(string strSql)
        {
            DataTable datatable = new DataTable();

            OracleDataAdapter oraAdapter = new OracleDataAdapter();
            oraAdapter.SelectCommand = new OracleCommand(strSql, (OracleConnection)this.Connection, (OracleTransaction)this.Transaction);
            oraAdapter.Fill(datatable);
            return datatable;
        }
        public override System.Data.IDbCommand CreateDbCommand( string commandText)
        {
            IDbCommand c= new OracleCommand(commandText, this.Connection as OracleConnection, this.Transaction as OracleTransaction) as System.Data.IDbCommand;
            if (c== null)
                throw new ApplicationException("创建OracleCommand失败");
            return c;
        }
        public override System.Data.IDbDataAdapter CreateDbDataAdapter()
        {
            IDbDataAdapter  a= new OracleDataAdapter() as System.Data.IDbDataAdapter;
            if (a==null)
                 throw new ApplicationException ("创建OracleDataAdapter失败");
            return a;
        }
        public override System.Data.Common.DbCommandBuilder CreateDbCommandBuilder(System.Data.Common.DbDataAdapter adapter)
        {
            System.Data.Common.DbCommandBuilder c = new OracleCommandBuilder(adapter as OracleDataAdapter);
            if (c == null)
                throw new ApplicationException("创建OracleCommandBuilder失败");
            return c;
        }
        public override string UserName
        {
            get { throw new NotImplementedException(); }
        }
        public override string Service
        {
            get { throw new NotImplementedException(); }
        }
	}
}
