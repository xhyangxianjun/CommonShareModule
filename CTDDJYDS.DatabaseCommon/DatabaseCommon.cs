using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTDDJYDS.DatabaseCommon
{
    public enum DatabasePlatformType
    {
        Unknown = 0,
        MySQL,
        Firebird,
        SQLServer2008,
        SQLServer2012,
        SQLServer2014,
        SQLServer2016,
        DB_Oracle,
        DB_OLEDB_Access,
    }

    /// <summary>
    /// 数据连接属性
    /// </summary>
    public class DatabaseSite
    {
        public string dataSourceName = "";
        public DatabasePlatformType dataSourceType = 0;
        public string dbName = "";
        public string userID = "";
        public string passWord = "";
    }

    public class DatabaseCommon
    {
    }
}
