using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace CTDDJYDS.DatabaseCommon
{
    public class ProSqlServerDataSource : ProDataSource
    {
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public ProSqlServerDataSource(string connectionString)
        {
            this._connection = new SqlConnection(connectionString);
            this._transaction = null;
        }

        /// <summary>
        /// 得到数据库的连接信息
        /// </summary>
        public override IDbConnection Connection
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
        public override void Open()
        {
            if (this._connection.State != ConnectionState.Open)
                this._connection.Open();

        }

        public override void Close()
        {
            if (this._connection.State == ConnectionState.Open)
                this._connection.Close();
        }

        public override void BeginTransaction()
        {
            this._transaction = this._connection.BeginTransaction();
        }

        public override void CommitTransaction()
        {
            this._transaction.Commit();
        }

        public override void RollBackTransaction()
        {
            this._transaction.Rollback();
        }
        public override DatabasePlatformType instDataSourceType()
        {
            return DatabasePlatformType.SQLServer2008;
        }
        #region ExecuteNonQuery
        /// <summary>
        /// 执行命令，但不返回任何结果
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public override int ExecuteNonQuery(string commandText)
        {

            SqlCommand command = new SqlCommand(commandText, this._connection, this._transaction);
            return command.ExecuteNonQuery();

        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// 执行命令，返回一个类型化的IDataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public override IDataReader ExecuteReader(string commandText)
        {

            SqlCommand command = new SqlCommand(commandText, this._connection, this._transaction);
            return command.ExecuteReader();

        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行命令，返回一个值
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public override object ExecuteScalar(string commandText)
        {

            SqlCommand command = new SqlCommand(commandText, this._connection, this._transaction);
            return command.ExecuteScalar();

        }
        #endregion

        #region ExecuteXmlReader
        public XmlReader ExecuteXmlReader(string commandText)
        {
            SqlCommand command = new SqlCommand(commandText, this._connection, this._transaction);
            return command.ExecuteXmlReader();
        }
        #endregion

        #region GetServerTime
        public override DateTime GetServerTime()
        {
            DateTime DtmServerdate = DateTime.Now;
            SqlDataReader objRead = null;
            string strQuerySql = "";

            strQuerySql = "select CONVERT( datetime, getdate(),121)";

            try
            {
                objRead = (SqlDataReader)ExecuteReader(strQuerySql);
            }
            catch (SqlException ex)
            {
                if (objRead != null)
                {
                    objRead.Close();
                }
                throw new Exception("GetServerTime失败!", ex);
            }
            objRead.Read();
            DtmServerdate = objRead.GetDateTime(0);

            if (objRead != null)
            {
                objRead.Close();
            }
            return DtmServerdate;
        }
        #endregion

        #region GetNewID
        public override int GetNewID(string SeqName)
        {
            int intNewid = GetNewID(SeqName, 1);
            return intNewid;
        }
        public override int GetNewID(string SeqName, int Count)
        {
            int intNewid = 0;
            string strCreateSql = "";
            string strQuerySql = "";
            SqlDataReader objRead = null;

            strQuerySql = "dbo.gqsp_GetNextID '" + SeqName + "' ," + Count + "";//声明存储过程名

            right:
            try
            {

                objRead = (SqlDataReader)ExecuteReader(strQuerySql);
            }
            catch (SqlException ex)
            {
                if (objRead != null)
                {
                    objRead.Close();
                }
                if (ex.Number == -2147217900)
                {

                    strCreateSql = "Insert into T_LANDMAX (IDName,NextVal) Values ('" + SeqName + "',1)";

                    ExecuteNonQuery(strCreateSql);

                    goto right;
                }
                else
                {
                    throw new Exception("GetNewID失败!", ex);
                }

            }

            objRead.Read();
            intNewid = Convert.ToInt32(objRead.GetValue(0));

            if (objRead != null)
            {
                objRead.Close();
            }
            return intNewid;
        }
        #endregion

        public override DataTable SelectBySql(string strSql)
        {
            DataTable datatable = new DataTable();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = new SqlCommand(strSql, (SqlConnection)this.Connection, (SqlTransaction)this.Transaction);
            sqlAdapter.Fill(datatable);
            return datatable;
        }
        public override System.Data.IDbCommand CreateDbCommand(string commandText)
        {
            return new SqlCommand(commandText,this.Connection as SqlConnection , this.Transaction  as SqlTransaction ) as System.Data.IDbCommand;
        }

        public override System.Data.IDbDataAdapter CreateDbDataAdapter()
        {
            return new SqlDataAdapter() as System.Data.IDbDataAdapter;
        }

        public override System.Data.Common.DbCommandBuilder CreateDbCommandBuilder(System.Data.Common.DbDataAdapter adapter)
        {
            return new SqlCommandBuilder(adapter as SqlDataAdapter);
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
