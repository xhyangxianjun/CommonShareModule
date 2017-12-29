using System;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using CTDDJYDS.CommonModule;
using System.Threading;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Data.Common;
using NHibernate;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CTDDJYDS.Database.Common
{
	/// <summary>
	/// 数据连接对象类
	/// </summary>
	public abstract class ProDataSource : IDisposable,IDBPlatformCommon 
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

        public virtual void WriteByteRow(string tableName, Dictionary<string, object> fieldsNameValueDic) { }

        protected DatabasePlatformType pfType;

        protected DbConnection connection;

        protected DBConfigurationSection nhConfigSection;

        protected string nhConfigTemplate;
        protected string dbSys;

        protected bool isStoreProcedureSupport;

        protected static readonly Regex primaryKeyRegx = new Regex(@"\(\d+,");

        protected static readonly Regex recordLineStartRegx = new Regex(@"INSERT\s+INTO\s+\S+\s+VALUES\s*\(\s*\d+\s*\,\s*\'");
        protected static readonly string sqlStart = "INSERT INTO";

        protected string errorMessage;

        protected BackgroundWorker worker;
        public BackgroundWorker Worker
        {
            get { return worker; }
            set { worker = value; }
        }

        protected static bool restoreTemplateDB = false;
        protected static string dataPathForDB = null;

        protected string NewConfigurationFilePath
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GetDatabaseConfigFile());
            }
        }

        public DatabasePlatformType DBPlatformType
        {
            get { return this.pfType; }
        }

        public virtual string _Server
        {
            get
            {
                if (nhConfigSection != null)
                {
                    return nhConfigSection.Server;
                }
                else if (connection != null)
                {
                    return connection.DataSource;
                }
                return "localhost";
            }
        }

        public virtual string _InstanceName
        {
            get
            {
                if (nhConfigSection != null)
                {
                    return nhConfigSection.SID;
                }
                return "";
            }
        }

        public virtual string _User
        {
            get
            {
                if (nhConfigSection != null)
                {
                    return nhConfigSection.User;
                }
                return "root";
            }
        }

        public virtual string _Password
        {
            get
            {
                string pwdStr = string.Empty;
                if (nhConfigSection != null)
                {
                    pwdStr = nhConfigSection.Password;
                }
                byte[] pwd = Convert.FromBase64String(pwdStr);
                string password = Encoding.Unicode.GetString(pwd);
                return password;
            }
        }

        public virtual string _Database
        {
            get
            {
                if (nhConfigSection != null)
                {
                    return nhConfigSection.Database;
                }
                return string.Empty;
            }
        }

        public virtual bool _TrustedConnection
        {
            get
            {
                if (nhConfigSection != null)
                {
                    return nhConfigSection.TrustedConnection;
                }
                return false;
            }
        }

        public virtual string _ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
        }

        public DBConfigurationSection NHConfigSection
        {
            get { return nhConfigSection; }
        }


        public virtual bool IsStoreProcedureSupport
        {
            get
            {
                return isStoreProcedureSupport;
            }
            internal set
            {
                isStoreProcedureSupport = value;
            }
        }

        protected virtual string ConfigurationKey
        {
            get { return string.Empty; }
        }

        protected string LastDBPlatformKey
        {
            get
            {
                return string.Empty;
            }
        }

        protected virtual void InternalInitialize(string asm)
        {
        }

        protected virtual string LocalizeScript(string script)
        {
            return script;
        }

        protected abstract DBConfigurationSection GetPlatformInstance();

        protected DBConfigurationSection CreateConfigSection(string server, string sid, string user, string password, string database, bool trustedConnection = true)
        {
            DBConfigurationSection dbCfg = GetPlatformInstance();
            dbCfg.Server = server;
            dbCfg.SID = sid;
            dbCfg.User = user;
            password = Convert.ToBase64String(Encoding.Unicode.GetBytes(password), Base64FormattingOptions.None);
            dbCfg.Password = password;
            dbCfg.Database = database;
            dbCfg.TrustedConnection = trustedConnection;
            return dbCfg;
        }

        public virtual string NHibernateConfigTemplate
        {
            get
            {
                if (string.IsNullOrEmpty(nhConfigTemplate))
                {
                    InternalInitialize(GetDatabaseAssembly());
                }
                return nhConfigTemplate;
            }
        }

        private string GetDatabaseConfigFile()
        {
            string preFix = "ctddjyds.Database.";

            string postFix = "Common";

            return $"{preFix}{postFix}.dll.config";
        }

        private string GetDatabaseAssembly()
        {
            string preFix = "ctddjyds.Database.";
            string postFix = string.Empty;

            return $"{preFix}{postFix}";
        }

        public string GetDatabaseKeyTableName()
        {
            return string.Empty;
        }

        public void AcceptConfig(string server, string sid, string user, string password, string database, bool trustedConnection = true)
        {
            this.nhConfigSection = CreateConfigSection(server, sid, user, password, database, trustedConnection);
            SaveConfig();
        }

        public void UpdateConfig(string server, string sid, string user, string password, string database, bool trustedConnection = true)
        {
            if (this.nhConfigSection == null)
            {
                AcceptConfig(server, sid, user, password, database, trustedConnection);
            }
            else
            {
                this.nhConfigSection.Server = server;
                this.nhConfigSection.SID = sid;
                this.nhConfigSection.User = user;
                this.nhConfigSection.Password = Convert.ToBase64String(Encoding.Unicode.GetBytes(password), Base64FormattingOptions.None);
                this.nhConfigSection.TrustedConnection = trustedConnection;
            }
        }

        protected DatabasePlatformType GetLastAccessPlatformType()
        {
            Configuration cfg = GetDBConfiguration();
            if (cfg != null)
            {
                LastDBPlatformSection sec = cfg.GetSection(LastDBPlatformKey) as LastDBPlatformSection;
                if (sec != null)
                {
                    return (DatabasePlatformType)sec.LastDBPlatform;
                }
            }
            return DatabasePlatformType.SQLServer;
        }

        #region IDBPlatform Members

        public virtual string NHConfigParagraph()
        {
            return null;
        }

        public virtual void SaveConfig()
        {
            Configuration cfg = GetDBConfiguration();
            if (cfg != null && nhConfigSection != null)
            {
                cfg.Sections.Remove(ConfigurationKey);
                cfg.Sections.Add(ConfigurationKey, nhConfigSection);

                cfg.Sections.Remove(LastDBPlatformKey);
                cfg.Sections.Add(LastDBPlatformKey, new LastDBPlatformSection(DBPlatformType));

                cfg.Save();
                ConfigurationManager.RefreshSection(ConfigurationKey);
                ConfigurationManager.RefreshSection(LastDBPlatformKey);
            }
        }

        public virtual void LoadConfig()
        {
            Configuration config = GetDBConfiguration();
            if (config != null)
            {
                DBConfigurationSection cfgSec = config.GetSection(ConfigurationKey) as DBConfigurationSection;
                nhConfigSection = cfgSec;
            }
        }

        public virtual bool NHConnect(out string strException)
        {
            strException = string.Empty;
            if (DatabaseHelper.RedirectNHibernateConfiguration(NHConfigParagraph(), out strException))
            {
                ISession s = NHibernateHelper.GetCurrentSession();
                if (this.connection != null && !Object.ReferenceEquals(this.connection, s.Connection))
                    this.connection.Dispose();
                this.connection = s.Connection as DbConnection;
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool IsDatabaseExists(string dbName)
        {
            return false;
        }

        #endregion

        public virtual bool IsDatabaseAccessible(string server, uint port, string user, string pwd, bool trustedConnection, string database)
        {
            return false;
        }

        public virtual bool CreateDatabase(string database)
        {
            errorMessage = string.Empty;
            if (connection != null)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    DbCommand cmd = connection.CreateCommand();
                    cmd.CommandText = string.Format("CREATE DATABASE {0}", database);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        LogHelper.Log(e);
                        return false;
                    }

                    cmd.CommandText = string.Format("USE {0}", database);
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception e)
                {
                    LogHelper.Log(e);
                    return false;
                }
            }
            return false;
        }

        public virtual bool CreateDatabase(string database, string dataPath)
        {
            restoreTemplateDB = !string.IsNullOrEmpty(dataPath);
            dataPathForDB = dataPath;
            bool result = CreateDatabase(database);
            restoreTemplateDB = false;
            dataPathForDB = null;

            return result;
        }

        public virtual bool DropDatabase(string database)
        {
            return false;
        }

        public virtual bool CanBackupOrRestore()
        {
            return true;
        }

        public virtual bool Backup(string backupfile)
        {
            return false;
        }

        public virtual bool Backup(string dbName, string backupfile)
        {
            return Backup(dbName, backupfile, true);
        }

        public virtual bool Backup(string dbName, string backupfile, bool packing)
        {
            return false;
        }

        public virtual bool Backup(List<BackupItem> items, string backupfile)
        {
            return false;
        }

        public virtual bool Restore(string restorefile, string destDatabase = null)
        {
            return false;
        }

        public virtual List<object> ExecuteQuery(string query)
        {
            List<object> result = new List<object>();
            if (connection != null)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    DbCommand cmd = connection.CreateCommand();
                    cmd.CommandText = query;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            int columns = reader.FieldCount;
                            while (reader.Read())
                            {
                                if (columns == 1)
                                {
                                    result.Add(reader.GetValue(0));
                                }
                                else
                                {
                                    object[] values = new object[columns];
                                    reader.GetValues(values);
                                    result.Add(values);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                }
            }
            return result;
        }

        public virtual IDbConnection GetConnection()
        {
            if (this.connection != null)
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                return this.connection;
            }
            else
            {
                return null;
            }
        }

        public virtual IDbCommand CreateCommand()
        {
            return this.connection.CreateCommand();
        }

        public virtual void RestoreConnection()
        {
        }

        public virtual bool ImportDatafromDir(string dir, string destDatabase = null)
        {
            return ImportDatafromDir(dir, true, destDatabase);
        }

        public virtual bool ImportDatafromDir(string dir, bool redirectNHibernate, string destDatabase = null)
        {
            return true;
        }

        protected Configuration GetDBConfiguration()
        {
            string location = Assembly.GetCallingAssembly().Location + ".config";
            if (!File.Exists(NewConfigurationFilePath) && File.Exists(location))
            {
                File.Copy(location, NewConfigurationFilePath);
            }
            location = NewConfigurationFilePath;
            if (File.Exists(location))
            {
                try
                {
                    ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                    fileMap.ExeConfigFilename = location;
                    return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                }
                catch (System.Exception ex)
                {
                    LogHelper.Log(ex);
                    return null;
                }
            }
            else
            {
                try
                {
                    // create the configuration file if needed
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml("<?xml version='1.0' encoding='utf-8' ?>" + "<configuration></configuration>");
                    doc.Save(location);
                }
                catch (System.Exception ex)
                {
                    LogHelper.Log(ex);
                }

                return null;
            }
        }

        public virtual bool Backup(List<BackupItemEx> items, string backupfile)
        {
            try
            {
                if (items != null && items.Count > 0)
                {
                    string dir = AppDomain.CurrentDomain.BaseDirectory;
                    // make sure the capacity is big enough
                    List<string> files = new List<string>(items.Count);
                    int index = 0;
                    int count = 0;
                    AutoResetEvent autoResetEvent = new AutoResetEvent(false);
                    foreach (BackupItemEx bi in items)
                    {
                        if ((bi.BackupObjects == null || bi.BackupObjects.Count == 0) && string.IsNullOrEmpty(bi.DumpQuery))
                        {
                            continue;
                        }

                        ++count;
                        ThreadPool.QueueUserWorkItem((obj) =>
                        {
                            BackupItemEx tmpBi = obj as BackupItemEx;
                            Backup(tmpBi);
                            if (File.Exists(tmpBi.BackupFileName))
                            {
                                FileInfo fi = new FileInfo(tmpBi.BackupFileName);
                                if (fi.Length > 2)
                                {
                                    lock (this) //need synchronization block
                                    {
                                        files.Add(tmpBi.BackupFileName);
                                    }
                                }
                                else
                                {
                                    FileDirectoryOperate.DeleteFileWithTime(tmpBi.BackupFileName);
                                }
                            }
                            Interlocked.Increment(ref index);
                            if (index == count)
                            {
                                autoResetEvent.Set();
                            }

                            try
                            {
                                if (worker != null && worker.WorkerReportsProgress && worker.IsBusy)
                                {
                                    worker.ReportProgress(index * 100 / items.Count);
                                }
                            }
                            catch
                            {

                            }
                        }, bi);
                    }
                    autoResetEvent.WaitOne(60000, false);


                    if (files.Count == 0)
                    {
                        return false;
                    }

                    if (worker != null && worker.WorkerReportsProgress && worker.IsBusy)
                    {
                        worker.ReportProgress(50, string.Format("Back up  database:", backupfile));
                    }

                    SharpZipHelper.ZipMultiFiles(files.ToArray(), backupfile, DatabaseCommon.ZipPwd, 3, worker);
                    foreach (string file in files)
                    {
                        FileDirectoryOperate.DeleteFileWithTime(file);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return false;
            }
        }

        protected virtual void Backup(BackupItemEx bi)
        {

        }
    }
}
