using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTDDJYDS.Database.Common
{
    public interface IDBPlatformCommon
    {
        string _Server { get; }
        
        string _InstanceName { get; }

        string _User { get; }

        string _Password { get; }

        string _Database { get; }

        bool _TrustedConnection { get; }

        bool IsStoreProcedureSupport { get; }

        string _ErrorMessage { get; }

        DatabasePlatformType DBPlatformType { get; }

        System.ComponentModel.BackgroundWorker Worker { get; set; }

        bool NHConnect(out string strException);

        bool IsDatabaseAccessible(string server, uint port, string user, string pwd, bool trustedConnection, string database);

        bool CreateDatabase(string database);

        bool CreateDatabase(string database, string dataPathForDB);

        void RestoreConnection();

        void AcceptConfig(string server, string sid, string user, string password, string database, bool trustedConnection = true);

        string NHConfigParagraph();

        void LoadConfig();

        void SaveConfig();

        DBConfigurationSection NHConfigSection { get; }

        bool DropDatabase(string database);

        bool Backup(string backupfile);

        bool Backup(string dbName, string backupfile);

        bool Backup(List<BackupItem> items, string backupfile);

        bool Restore(string restorefile, string destDatabase = null);

        bool ImportDatafromDir(string dir, string destDatabase = null);
    }
}
