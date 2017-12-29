using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTDDJYDS.Database.Common
{
    public enum DatabasePlatformType
    {
        Unknown = 0,
        MySQL,
        Firebird,
        SQLServer,
        DB_Oracle,
        DB_OLEDB_Access,
    }

    /// <summary>
    /// 数据连接属性
    /// </summary>
    public class DatabaseSite
    {
        public string dataSourceName = "";
        public string instanceName = "";//for sqlserver
        public DatabasePlatformType dataSourceType = 0;
        public string dbName = "";
        public string userID = "";
        public string passWord = "";
    }

    public class DatabaseCommon
    {
        public static readonly string ZipPwd = "15f48fe1-f922-4b8e-98e7-d998f460cee1";
    }
}
