using System;
using System.Data;
using System.Xml;
using System.Collections.Generic;

namespace CTDDJYDS.DatabaseCommon
{
	/// <summary>
	/// 数据连接对象类
	/// </summary>
	public abstract class ProDataSource : IDisposable 
	{        
		#region IDisposable 成员
		public virtual void Dispose(bool disposing)
		{
			if(!disposing)
				return;
			
			if(this.Connection != null)			
				this.Connection.Close();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this );
		}
		#endregion		
		/// <summary>
		/// 得到数据库的连接信息
		/// </summary>
		public abstract IDbConnection Connection
		{
			get;
		}
		/// <summary>
		/// 公开事务连接操作
		/// </summary>
		public abstract IDbTransaction Transaction
		{
			get;			
		}
        
        public abstract  ConnectionState State
        {
            get;
        }
		public abstract void Open();		

		public abstract void Close();

		public abstract void BeginTransaction();

		public abstract void CommitTransaction();

		public abstract void RollBackTransaction();	

		public abstract DatabasePlatformType instDataSourceType();

		#region ExecuteNonQuery
		/// <summary>
		/// 执行命令，但不返回任何结果
		/// </summary>
		/// <param name="commandText"></param>
		/// <returns></returns>
		public abstract int ExecuteNonQuery(string commandText);
		#endregion

		#region ExecuteReader
		/// <summary>
		/// 执行命令，返回一个类型化的IDataReader
		/// </summary>
		/// <param name="commandText"></param>
		/// <returns></returns>
		public abstract IDataReader  ExecuteReader(string commandText);
		#endregion

		#region ExecuteScalar
		/// <summary>
		/// 执行命令，返回一个值
		/// </summary>
		/// <param name="commandText"></param>
		/// <returns></returns>
		public abstract  object ExecuteScalar(string commandText);
		#endregion

        #region GetServerTime
        public abstract  DateTime GetServerTime();
		#endregion

		#region GetNewID
		public abstract int GetNewID(string SeqName);

		public abstract  int  GetNewID(string SeqName,int Count);

		#endregion

        public virtual bool ExistTable(string TABLESPACE_NAME, string TABLE_NAME)
        { return true ;}

        public virtual bool ExistField(string table,string field)
        { return true; }

        public abstract DataTable SelectBySql(string strSql);

        public abstract IDbDataAdapter CreateDbDataAdapter();
        public abstract IDbCommand CreateDbCommand(string commandText);
        public abstract System.Data.Common.DbCommandBuilder CreateDbCommandBuilder(System.Data.Common.DbDataAdapter adapter );
        public abstract string UserName { get; }
        public abstract string Service { get; }
        public virtual void WriteByteRow(string tableName, Dictionary<string, object> fieldsNameValueDic) { }
	}
}
