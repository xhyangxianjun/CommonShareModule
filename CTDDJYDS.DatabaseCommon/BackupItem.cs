using System;
using System.Collections.Generic;

namespace CTDDJYDS.Database.Common
{
    public class BackupItem
    {
        public string TableName;
        public string PrimaryKey;
        public string GuidFieldName;
        public List<string> Guids;
        public string BackupFileName;
        public bool IsFullTable;
        public string DumpQuery;

        public BackupItem()
        {
            this.Guids = new List<string>();
        }

        public BackupItem(string tableName)
        {
            this.TableName = tableName;
            this.IsFullTable = true;
        }
    }

    public class BackupItemEx
    {
        public string TableName;
        public string PrimaryKey;
        public string GuidFieldName;
        public string BackupFileName;
        public bool IsFullTable;
        public Type ItemType;

        public long RecordsNum = 0; // if this field > 0, means this table may contain huge number of records and should be processed by segments

        public string DumpQuery;
        public List<object> BackupObjects;

        public const int BatchNum = 1000;

        public BackupItemEx()
        {
            ItemType = null;
            BackupObjects = new List<object>();
        }

        public BackupItemEx(string tableName, string guidFieldName, Type objType)
        {
            this.TableName = tableName;
            this.ItemType = objType;
            this.GuidFieldName = guidFieldName;
            BackupObjects = new List<object>();
        }

        public BackupItem ToBackupItem()
        {
            BackupItem bi = new BackupItem();
            bi.TableName = this.TableName;
            bi.PrimaryKey = this.PrimaryKey;
            bi.GuidFieldName = this.GuidFieldName;
            bi.BackupFileName = this.BackupFileName;
            bi.IsFullTable = this.IsFullTable;
            bi.DumpQuery = this.DumpQuery;

            return bi;
        }
    }
}
