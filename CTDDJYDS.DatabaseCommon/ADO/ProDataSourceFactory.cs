using System;
using System.Data.OleDb;
using System.Data.Odbc;

namespace CTDDJYDS.DatabaseCommon
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
                case DatabasePlatformType.SQLServer2008:
                case DatabasePlatformType.SQLServer2012:
                case DatabasePlatformType.SQLServer2014:
                case DatabasePlatformType.SQLServer2016:
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
                case DatabasePlatformType.SQLServer2008:
                case DatabasePlatformType.SQLServer2012:
                case DatabasePlatformType.SQLServer2014:
                case DatabasePlatformType.SQLServer2016:
                    conString = "Data Source=" + site.dataSourceName + ";Initial Catalog=" + site.dbName + ";User ID=" + site.userID + ";Password=" + site.passWord;
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
        /// <param name="dataSourceName"></param>
        /// <param name="catalog"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static ProDataSource CreateInstance(string dataSourceName, string catalog, string username, string password, DatabasePlatformType dbType)
	    {
            string conString = "";
            switch (dbType)
		    {
                case DatabasePlatformType.SQLServer2008:
                case DatabasePlatformType.SQLServer2012:
                case DatabasePlatformType.SQLServer2014:
                case DatabasePlatformType.SQLServer2016:
                    conString = "Data Source=" + dataSourceName + ";Initial Catalog=" + catalog + ";User ID=" + username + ";Password=" + password;
				    break;
                case DatabasePlatformType.DB_Oracle:
                    conString = "Data Source=" + dataSourceName + ";User ID=" + username + ";Password=" + password;
				    break;
                case DatabasePlatformType.DB_OLEDB_Access:
                    if (username ==null || username =="" )
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
