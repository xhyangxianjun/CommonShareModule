using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTDDJYDS.DatabaseCommon
{
    interface IDBPlatformCommon
    {
        string _Server { get; }

        uint _Port { get; }

        string _User { get; }

        string _Password { get; }

        string _Database { get; }

        bool _TrustedConnection { get; }

        string _ErrorMessage { get; }

        DatabasePlatformType DBPlatformType { get; }

        System.ComponentModel.BackgroundWorker Worker { get; set; }

        bool NHConnect(out string strException);

        bool ConnectServer(string server, uint port, string user, string pwd, bool trustedConnection, out string[] databases);

        bool ConnectServer(string server, uint port, string user, string pwd, bool trustedConnection);

        bool IsDatabaseAccessible(string server, uint port, string user, string pwd, bool trustedConnection, string database);

        void TryAutoStartDatabaseService();

        bool CreateDatabase(string database);

        bool CreateDatabase(string database, string dataPathForDB);

        void RestoreConnection();

        void AcceptConfig(string server, uint port, string user, string password, string database, bool trustedConnection = true);

        bool GetDatabaseInfo(string dbName, out string createTime, out string lastAccessTime);
    }
}
