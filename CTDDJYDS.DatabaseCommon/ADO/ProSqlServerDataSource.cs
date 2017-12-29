using CTDDJYDS.CommonModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

namespace CTDDJYDS.Database.Common
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
            return DatabasePlatformType.SQLServer;
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
        protected readonly string windowsAuthenticationString = "Integrated Security=SSPI";
        protected readonly string serverAuthenticationString = "User Id={0};Password={1}";

        protected static readonly Regex sqlReg = new Regex(@"\AINSERT\s+INTO\s+\S(?<table>\w+)\S", RegexOptions.IgnoreCase);
        protected static readonly Regex sqlIDReg = new Regex(@"\AINSERT\s+INTO\s+\S(?<table>\w+)\S\s+VALUES\s+\(\w+\,", RegexOptions.IgnoreCase);
        protected static readonly Regex tableNameReg = new Regex(@"\A[a-z]+(_[a-z0-9]+)*");
        protected static readonly Dictionary<string, bool> tempTableDic = new Dictionary<string, bool>();

        protected static readonly List<string> localIPList;

        private static Dictionary<string, Tuple<string, DatabasePlatformType>> dicName2PlatformType = new Dictionary<string, Tuple<string, DatabasePlatformType>>();       

        protected override void InternalInitialize(string asm)
        {
            nhConfigTemplate =
                    "<hibernate-configuration  xmlns=\"urn:nhibernate-configuration-2.2\" >" +
                    "<session-factory name=\"NHibernate.Test\">" +
                    //"<property name=\"connection.provider\">NHibernate.Connection.DriverConnectionProvider</property>" +
                    "<property name=\"connection.driver_class\">NHibernate.Driver.SqlClientDriver</property>" +
                    "<property name=\"connection.connection_string\">" +
                    "Server={0};initial catalog={1};{2};Pooling=False" +
                    "</property>" +
                    "<property name=\"dialect\">NHibernate.Dialect.MsSql2008Dialect</property>" +
                    "<property name=\"show_sql\">false</property>" + //skip show sql command
                    "<property name=\"adonet.batch_size\">32</property>" +
                    "<property name=\"command_timeout\">60</property>" +
                    "<property name=\"query.substitutions\">true 1, false 0, yes 'Y', no 'N'</property>" +
                    "<mapping assembly=\"" + asm + "\"/>" +
                    "</session-factory>" +
                    "</hibernate-configuration>";

            dbSys = "master";
        }

        public static bool IsLocalHost(string server, out string localIP)
        {
            localIP = string.Empty;
            if (!string.IsNullOrEmpty(server))
            {
                if (server.StartsWith("localhost") || server.StartsWith(".") ||
                    server.StartsWith(@"\") || server.StartsWith("(local)"))
                {
                    return true;
                }
                else if (server.StartsWith("127.0.0.1"))
                {
                    localIP = "127.0.0.1";
                    return true;
                }
                else if (server.StartsWith(Environment.MachineName))
                {
                    localIP = Environment.MachineName;
                    return true;
                }
                else
                {
                    foreach (string ipaddr in localIPList)
                    {
                        if (server.StartsWith(ipaddr))
                        {
                            localIP = ipaddr;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        protected void CheckConnectionString(bool useMaster = true)
        {
            if (connection != null)
            {
                SqlConnectionStringBuilder connBuilder;
                if (string.IsNullOrEmpty(connection.ConnectionString))
                {
                    connBuilder = new SqlConnectionStringBuilder();
                }
                else
                {
                    connBuilder = new SqlConnectionStringBuilder(connection.ConnectionString);
                }
                connBuilder.DataSource = _Server;
                connBuilder.UserID = _User;
                connBuilder.Password = _Password;
                connBuilder.Pooling = false;
                connBuilder.ConnectTimeout = 10;
                connBuilder.IntegratedSecurity = _TrustedConnection;
                if (!useMaster && !string.IsNullOrEmpty(_Database))
                {
                    connBuilder.InitialCatalog = _Database;
                }

                connection.Close();
                connection.ConnectionString = connBuilder.ConnectionString;
            }
        }

        protected string GetConnectionString(bool useMaster = false, string destDatabase = null)
        {
            var database = destDatabase ?? _Database;
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
            connBuilder.DataSource = _Server;
            connBuilder.UserID = _User;
            connBuilder.Password = _Password;
            if (!useMaster && !string.IsNullOrEmpty(database))
            {
                connBuilder.InitialCatalog = database;
            }
            connBuilder.Pooling = false;
            connBuilder.ConnectTimeout = 10;
            connBuilder.IntegratedSecurity = _TrustedConnection;

            return connBuilder.ConnectionString;
        }

        protected bool IsSupportedSQL(string cmd, out string tableName)
        {
            tableName = string.Empty;
            Match mt = sqlReg.Match(cmd);
            if (mt.Success)
            {
                tableName = mt.Groups["table"].Value;
                if (tempTableDic.ContainsKey(tableName))
                {
                    return false;
                }
            }
            return mt.Success;
        }

        protected string GetTableNameFromFile(string filePath)
        {
            string name = Path.GetFileNameWithoutExtension(filePath);
            Match mt = tableNameReg.Match(name);
            if (mt.Success)
            {
                return mt.ToString();
            }
            return string.Empty;
        }

        public override string _User
        {
            get
            {
                if (nhConfigSection != null)
                {
                    return nhConfigSection.User;
                }
                return "sa";
            }
        }

        public override bool _TrustedConnection
        {
            get
            {
                if (nhConfigSection != null)
                {
                    return nhConfigSection.TrustedConnection;
                }
                return true;
            }
        }

        public override string NHConfigParagraph()
        {
            string cfg = string.Empty;
            if (nhConfigSection == null)
            {
                LoadConfig();
            }
            ConfigurationSection_SQLSever cfgSec = nhConfigSection as ConfigurationSection_SQLSever;
            if (cfgSec != null)
            {
                byte[] pwd = Convert.FromBase64String(cfgSec.Password);
                string password = Encoding.Unicode.GetString(pwd);
                cfg = string.Format(NHibernateConfigTemplate, cfgSec.Server, cfgSec.Database, string.Format(serverAuthenticationString, cfgSec.User, password) + string.Format("; Trusted_Connection={0}", cfgSec.TrustedConnection));
            }
            return cfg;
        }

        public override bool CreateDatabase(string database)
        {
            CheckConnectionString();
            bool result = base.CreateDatabase(database);
            return result;
        }

        public override bool DropDatabase(string database)
        {
            this.ExecuteNonQuery("USE master");
            this.ExecuteNonQuery(String.Format("alter database {0} set single_user with rollback immediate", database));

            bool result = this.ExecuteNonQuery(string.Format("if db_id('{0}') is not null drop database {0}", database), true) >= -1;
            if (!result)
            {               
                //将数据库重新设置成单用户以防发生异常无法访问的情况
                ExecuteNonQuery(string.Format("alter database {0} set multi_user with rollback immediate", database));
            }
            return result;
        }

        protected int ExecuteNonQuery(string cmdText, bool useMaster)
        {
            int ret = -1;
            if (connection != null)
            {
                CheckConnectionString(useMaster);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                try
                {
                    DbCommand cmd = connection.CreateCommand();

                    cmd.CommandText = cmdText;
                    try
                    {
                        ret = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        ret = -2;
                        LogHelper.Log(e);
                    }
                }
                catch (Exception ex)
                {
                    ret = -2;
                    LogHelper.Log(ex);
                }
            }
            return ret;
        }

        public override List<object> ExecuteQuery(string query)
        {
            if (this.connection != null)
            {
                CheckConnectionString();

                return base.ExecuteQuery(query);
            }
            return new List<object>();
        }

        public override IDbConnection GetConnection()
        {
            CheckConnectionString(false);
            return base.GetConnection();
        }

        protected override DBConfigurationSection GetPlatformInstance()
        {
            ConfigurationSection_SQLSever instance = new ConfigurationSection_SQLSever();
            return instance;
        }

        public bool ConnectServer(string server, string sid, string user, string pwd, bool trustedConnection)
        {
            // AutoStartSQLServerService(server);
            errorMessage = string.Empty;
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
            connBuilder.DataSource = server + string.Format("/{0}", sid);
            connBuilder.UserID = user;
            connBuilder.Password = pwd;
            connBuilder.InitialCatalog = "master";
            connBuilder.Pooling = false;
            connBuilder.ConnectTimeout = 5;
            connBuilder.IntegratedSecurity = trustedConnection;
            using (SqlConnection coon = new SqlConnection(connBuilder.ConnectionString))
            {
                try
                {
                    coon.Open();

                    return true;
                }
                catch (SqlException ex)
                {
                    errorMessage = string.Format("Failed connecting to the server. \nPlease type in the correct password!\n\nDetail:\n{0}", ex.Message);
                    return false;
                }
                catch (Exception e)
                {
                    errorMessage = string.Format("Failed connecting to the server. \nPlease type in the correct settings!\n\nDetail:\n{0}", e.Message);
                    return false;
                }
            }
        }       

        protected override void Backup(BackupItemEx bi)
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Temp\");
            if (!Directory.Exists(dir))
            {
                FileDirectoryOperate.CreateDirectoryEx(dir);
            }
            if (bi.IsFullTable)
            {
                bi.BackupFileName = Path.Combine(dir, bi.TableName + ".bak");
            }
            else
            {
                bi.BackupFileName = Path.Combine(dir, bi.TableName + "_" + bi.GuidFieldName + ".bak");
            }

            var columnPropertyList = DatabaseHelper.GetColumnPropertyInfo(bi.TableName, bi.ItemType);

            try
            {
                using (FileStream fs = new FileStream(bi.BackupFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                using (TextWriter tw = new StreamWriter(fs, Encoding.Unicode))
                {
                    Type blobType = typeof(object);
                    Type stringType = typeof(string);
                    Type doubleType = typeof(double);
                    int recIndex = 0;
                    do
                    {
                        foreach (object backupObj in bi.BackupObjects)
                        {
                            int count = 0;
                            foreach (PropertyInfo pi in columnPropertyList)
                            {
                                bool writeContent = true;
                                object val = pi.GetValue(backupObj, null);
                                if (pi.PropertyType == stringType)
                                {
                                    if (val != null && string.IsNullOrEmpty(val as string))
                                    {
                                        val = "\0";
                                    }
                                }
                                else if (pi.PropertyType == doubleType)
                                {
                                    val = string.Format(CultureInfo.InvariantCulture, "{0:R}", val);
                                }
                                else if (pi.PropertyType == blobType && val is byte[])
                                {
                                    byte[] array = val as byte[];
                                    EncodingOperateHelper.ToHexString(array, tw);
                                    writeContent = false;
                                }
                                if (writeContent)
                                {
                                    tw.Write(val);
                                }
                                if (++count < columnPropertyList.Count)
                                {
                                    tw.Write(",\0");
                                }
                            }
                            tw.WriteLine();
                        }

                        bi.RecordsNum -= bi.BackupObjects.Count;
                        recIndex += bi.BackupObjects.Count;
                        bi.BackupObjects.Clear();
                        if (bi.RecordsNum > 0 && !string.IsNullOrEmpty(bi.DumpQuery))
                        {
                            bi.BackupObjects = DatabaseHelper.GetBoundedEntities(bi.DumpQuery, BackupItemEx.BatchNum, recIndex);
                        }
                        else
                        {
                            break;
                        }

                    } while (bi.RecordsNum > 0);
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Log(ex);
            }
        }

        public override bool CanBackupOrRestore()
        {
            // for a remote MSSQL server, cannot backup and restore
            // because the server need access rights to the local folder
            string localIP;
            return IsLocalHost(_Server, out localIP);
        }

        public override bool Backup(string backupfile)
        {
            return Backup(_Database, backupfile);
        }

        public override bool Backup(string dbName, string backupfile, bool packing)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(true)))
                {
                    conn.Open();

                    string dir = string.Empty;
                    dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Temp\");
                    if (!Directory.Exists(dir))
                    {
                        FileDirectoryOperate.CreateDirectoryEx(dir);
                    }
                    else
                    {
                        // SQL Server backup file
                        foreach (string s in Directory.GetFiles(dir, "*.bak"))
                        {
                            FileDirectoryOperate.DeleteFileWithTime(s);
                        }
                    }

                    #region File list to packup
                    string tempFile = Path.Combine(dir, dbName + ".bak");
                    List<string> files = new List<string>(2);
                    files.Add(tempFile);                  
                    #endregion

                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = string.Format("backup database {0} to disk = '{1}'", dbName, tempFile);
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();

                    if (!File.Exists(tempFile))
                    {
                        return false;
                    }

                    if (worker != null && worker.WorkerReportsProgress)
                    {
                        if (packing)
                        {
                            worker.ReportProgress(0, string.Format("DataBase Packing Up Files......", backupfile));
                        }
                        else
                        {
                            worker.ReportProgress(100);
                        }
                    }

                    if (!packing)
                    {
                        return true;
                    }

                    SharpZipHelper.ZipMultiFiles(files.ToArray(), backupfile, DatabaseCommon.ZipPwd, 6, worker);
                    foreach (string file in files)
                    {
                        FileDirectoryOperate.DeleteFileWithTime(file);
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool ImportDatafromDir(string dir, bool redirectNHibernate, string destDatabase = null)
        {
            var database = destDatabase ?? _Database;
            errorMessage = string.Empty;
            bool importSuccess = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString(destDatabase: database)))
                {
                    conn.Open();
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 600;

                    string[] files = Directory.GetFiles(dir, "*.bak");
                    if (files.Length > 0)
                    {                      
                        if (!RestoreDatabase(files, cmd, database))
                        {
                            return false;
                        }                                               
                    }
                    else if ((files = Directory.GetFiles(dir, "*.sql")).Length > 0)
                    {
                        #region SQL process
                        DbTransaction transc = null;
                        try
                        {
                            // use the current database
                            transc = conn.BeginTransaction();
                            cmd.Transaction = transc;

                            cmd.CommandText = string.Format("USE {0}", database);
                            cmd.ExecuteNonQuery();

                            // clear all table contents
                            if (worker != null && worker.WorkerReportsProgress)
                            {
                                worker.ReportProgress(0, "Truncating database...");
                            }

                            // delete all data and reset the identity column
                            cmd.CommandText = @"exec sp_MSforeachtable 'truncate table ?', N'?', 
'if exists(select name from sys.columns where object_id = object_id(''?'') and is_identity = 1) DBCC CHECKIDENT(''?'', reseed, 1)'";
                            cmd.ExecuteNonQuery();

                            if (worker != null && worker.WorkerReportsProgress)
                            {
                                worker.ReportProgress(0, "Executing scripts...");
                            }

                            foreach (string file in files)
                            {
                                using (SQLScriptParser parser = new SQLScriptParser(file))
                                {
                                    string tableName = string.Empty;
                                    string prevTable = string.Empty;
                                    string cmdPrefix = string.Empty;
                                    Dictionary<string, string> cmdDic = new Dictionary<string, string>();
                                    int curProgress = -1;
                                    bool checkTable = false;

                                    string cmdText = parser.NextCommand();
                                    while (!string.IsNullOrEmpty(cmdText))
                                    {
                                        if (IsSupportedSQL(cmdText, out tableName))
                                        {
                                            try
                                            {
                                                if (!cmdDic.TryGetValue(tableName, out cmdPrefix))
                                                {
                                                    #region Restore database
                                                    if (!string.IsNullOrEmpty(prevTable))
                                                    {
                                                        // switch the identity_insert option off
                                                        cmd.CommandText = string.Format("set identity_insert {0} off", prevTable);
                                                        cmd.ExecuteNonQuery();
                                                    }

                                                    #region prepare the command dictionary
                                                    // construct the full column list
                                                    cmd.CommandText = string.Format("select column_name from information_schema.columns where table_name = '{0}'", tableName);
                                                    using (DbDataReader reader = cmd.ExecuteReader())
                                                    {
                                                        StringBuilder sb = new StringBuilder();
                                                        sb.AppendFormat("INSERT INTO {0}(", tableName);
                                                        while (reader.Read())
                                                        {
                                                            sb.AppendFormat(string.Format("{0}, ", reader.GetString(0)));
                                                        }
                                                        sb.Replace(", ", ")", sb.Length - 2, 2);
                                                        cmdPrefix = sb.ToString();
                                                        cmdDic[tableName] = cmdPrefix;
                                                    }
                                                    #endregion

                                                    // handle database, set identity_insert on
                                                    prevTable = tableName;
                                                    cmd.CommandText = string.Format("set identity_insert {0} on", prevTable);
                                                    cmd.ExecuteNonQuery();
                                                    #endregion
                                                }

                                                cmd.CommandText = cmdText;
                                                int affectedRows = cmd.ExecuteNonQuery();
                                                if (affectedRows == 0 && checkTable)
                                                {
                                                    importSuccess = false;
                                                }

                                                if (worker != null && worker.WorkerReportsProgress && curProgress != parser.CurProgress)
                                                {
                                                    curProgress = parser.CurProgress;
                                                    worker.ReportProgress(curProgress);
                                                }
                                            }
                                            catch (Exception innerEx)
                                            {                                              
                                                importSuccess = false;
                                                LogHelper.Log(innerEx);                                                
                                            }
                                        }
                                        cmdText = parser.NextCommand();
                                    }

                                    if (!string.IsNullOrEmpty(prevTable))
                                    {
                                        cmd.CommandText = string.Format("set identity_insert {0} off", prevTable);
                                        cmd.ExecuteNonQuery();
                                    }
                                }

                                FileDirectoryOperate.DeleteFileWithTime(file);
                            }

                            transc.Commit();
                        }
                        catch (System.Exception transactEx)
                        {
                            LogHelper.Log(transactEx);
                            if (transc != null)
                            {
                                transc.Rollback();
                            }
                            importSuccess = false;
                        }
                        finally
                        {
                            if (transc != null)
                            {
                                transc.Dispose();
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception outerEx)
            {
                LogHelper.Log(outerEx);
                return false;
            }

            if (!importSuccess)
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = "Imported Data already exists in the current database!";
                }
                return false;
            }

            string strException = null;
            return DatabaseHelper.RedirectNHibernateConfiguration(NHConfigParagraph(), out strException);
        }

        protected bool RestoreDatabase(string[] dbFiles, DbCommand cmd, string database)
        {
            if (dbFiles == null || dbFiles.Length == 0)
            {
                return false;
            }

            // use the master database
            cmd.CommandText = "USE master";
            cmd.ExecuteNonQuery();

            // set the single_user mode to make sure the following steps work
            cmd.CommandText = string.Format("alter database {0} set single_user with rollback immediate", database);
            cmd.ExecuteNonQuery();

            // get the data directory of the SQL Server
            string dataDir = string.Empty;
            cmd.CommandText = "select physical_name from sys.database_files where type = 0";
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return false;
                }
                dataDir = Path.GetDirectoryName(reader.GetString(0));
            }
            try
            {
                foreach (string file in dbFiles)
                {
                    // get the file list in the back up file
                    cmd.CommandText = string.Format("restore filelistonly from disk = '{0}'", file);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        StringBuilder sb = new StringBuilder();
                        if (!reader.HasRows)
                        {
                            return false;
                        }
                        // construct the command text
                        sb.AppendFormat("restore database {0} from disk = '{1}' with recovery, replace, ", database, file);
                        while (reader.Read())
                        {
                            string logicalName = reader.GetString(0);
                            string fileType = reader.GetString(2);
                            sb.AppendFormat("move '{0}' to '{1}', ", logicalName,
                                Path.Combine(dataDir, database + (fileType == "D" ? ".mdf" : "_log.LDF")));
                        }
                        cmd.CommandText = sb.ToString(0, sb.Length - 2);
                    }
                    cmd.ExecuteNonQuery();

                    FileDirectoryOperate.DeleteFileWithTime(file);
                }
            }
            catch (Exception e)
            {
                LogHelper.Log(e);
                #region
                //将数据库重新设置成多用户模式以修复无法打开的bug
                cmd.CommandText = string.Format("alter database {0} set multi_user with rollback immediate", database);
                cmd.ExecuteNonQuery();
                return false;
                #endregion
            }
            return true;
        }

        public override bool IsDatabaseExists(string dbName)
        {
            if (this.connection != null)
            {
                CheckConnectionString();
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                DbCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format(@"select name from sys.databases where name = '{0}'", dbName);
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
            return false;
        }

        public override void RestoreConnection()
        {
            if (connection != null && nhConfigSection != null)
            {
                string datasource = _Server;
                SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder(connection.ConnectionString);
                if (connBuilder.DataSource != datasource || connBuilder.UserID != _User
                    || connBuilder.Password != _Password || connBuilder.InitialCatalog != _Database)
                {
                    connBuilder.DataSource = datasource;
                    connBuilder.UserID = _User;
                    connBuilder.Password = _Password;
                    string db = _Database;
                    if (!string.IsNullOrEmpty(db))
                    {
                        connBuilder.InitialCatalog = _Database;
                    }

                    connBuilder.Pooling = false;
                    connBuilder.ConnectTimeout = 10;
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.ConnectionString = connBuilder.ConnectionString;
                }

                nhConfigSection = CreateConfigSection(_Server, _InstanceName, _User, _Password, _Database, _TrustedConnection);
                SaveConfig();
            }
        }
    }
}
