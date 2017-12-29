using System;
using System.Data.OleDb;
using System.Data.Odbc;

namespace CTDDJYDS.Database.Common
{
    public static  class ProDataSourceFactory
    {
        public static string connectionString="";
        public static DatabasePlatformType databaseType { get; set; }

		
        #region 实例化数据连接对象

	    public static ProDataSource CreateInstance()
	    {
            return ProDataSourceFactory.CreateInstance(connectionString, databaseType);
	    }

        /// <summary>
        /// 打开数据库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public static ProDataSource CreateInstance(string connectionString, DatabasePlatformType databaseType) 
	    {	
		    ProDataSource dataSource = null;
            switch (databaseType) 
		    {
                case DatabasePlatformType.SQLServer:
                    dataSource = new ProSqlServerDataSource(connectionString);
				    break;
                case DatabasePlatformType.DB_Oracle:
                    dataSource = new ProOracleClientDataSource(connectionString);
				    break;
                case DatabasePlatformType.DB_OLEDB_Access:
                    dataSource = new ProOleDataSource(connectionString);
				    break;			   
			    default:
                    dataSource = new ProSqlServerDataSource(connectionString);
				    break;
		    }

		    return dataSource;
	    }

        public static ProDataSource CreateInstance(DatabaseSite site)
        {
            string conString = "";
            switch (site.dataSourceType)
            {
                case DatabasePlatformType.SQLServer:
                    if (string.IsNullOrEmpty(site.dataSourceName) && string.IsNullOrEmpty(site.userID))//Windows身份验证模式，使用默认数据库实例;
                        conString = string.Format("server=.;database={0};integrated security=SSPI", site.dbName);
                    else if (string.IsNullOrEmpty(site.userID))//如果不使用默认数据库实例，则用 server =./sid;
                        conString = string.Format("server=./{0};database={1};integrated security=SSPI", site.dataSourceName, site.dbName);
                    else//dataSourceName 一般为  ip/sid 模式
                        conString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", site.dataSourceName, site.dbName, site.userID, site.passWord);
                    break;
                case DatabasePlatformType.DB_Oracle:
                    conString = "Data Source=" + site.dataSourceName + ";User ID=" + site.userID + ";Password=" + site.passWord;                    
                    break;
                case DatabasePlatformType.DB_OLEDB_Access:
                    conString = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source=" + site.dataSourceName;                    
                    break;
                default:
                    conString = "Data Source=" + site.dataSourceName + ";Initial Catalog=" + site.dbName + ";User ID=" + site.userID + ";Password=" + site.passWord;
                    break;
            }
            connectionString = conString;
            databaseType = site.dataSourceType;
            return ProDataSourceFactory.CreateInstance(conString, site.dataSourceType);

        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="dataSourceName">运行数据库对应的计算机名,一般为IP地址</param>
        /// <param name="catalog">数据库名称</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static ProDataSource CreateInstance(string dataSourceName, DatabasePlatformType dbType, string catalog="", string username="", string password="")
	    {
            string conString = "";
            switch (dbType)
		    {
                case DatabasePlatformType.SQLServer:
                    if(string.IsNullOrEmpty(dataSourceName) && string.IsNullOrEmpty(username))//Windows身份验证模式，使用默认数据库实例;
                        conString = string.Format("server=.;database={0};integrated security=SSPI", catalog);
                    else if(string.IsNullOrEmpty(username))//如果不使用默认数据库实例，则用 server =./sid;
                        conString = string.Format("server=./{0};database={1};integrated security=SSPI", dataSourceName, catalog);
                    else//dataSourceName 一般为  ip/sid 模式
                        conString =string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", dataSourceName, catalog, username, password);
				    break;
                case DatabasePlatformType.DB_Oracle:
                    conString = "Data Source=" + dataSourceName + ";User ID=" + username + ";Password=" + password;
				    break;
                case DatabasePlatformType.DB_OLEDB_Access:
                    if (username ==null || username =="")//dataSourceName为Access文件路径
                        conString = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source=" + dataSourceName ;
                    else
                        conString = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source=" + dataSourceName + ";User ID=" + username ;
                    if (password !=null && password != string.Empty)
                    {
                        conString = string.Format("{0};Persist Security Info=true;Jet OLEDB:Database Password={1}", conString, password);
                    }
                    break;
                default:
                    conString = "Data Source=" + dataSourceName + ";Initial Catalog=" + catalog + ";User ID=" + username + ";Password=" + password;
                    break;
		    }
            connectionString = conString;
            databaseType = dbType;
            return ProDataSourceFactory.CreateInstance(conString, databaseType);
	    }

	    #endregion	
    }
    
}
