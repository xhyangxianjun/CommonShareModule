using System;
using System.Configuration;


namespace CTDDJYDS.Database.Common
{
    public class DBConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty ( "Server", DefaultValue = "localhost" )]
        public string Server
        {
            get{ return (string)this["Server"];}
            set{ this["Server"] = value;}
        }

        [ConfigurationProperty ("SID", DefaultValue = "" )]
        public string SID
        {
            get{ return (string)this["SID"];}
            set{ this["SID"] = value;}
        }

        [ConfigurationProperty ( "User", DefaultValue = "root" )]
        public string User
        {
            get { return (string)this["User"]; }
            set { this["User"] = value; }
        }
        [ConfigurationProperty ( "Password", DefaultValue = "" )]
        public string Password
        {
            get { return (string)this["Password"]; }
            set { this["Password"] = value; }
        }
        [ConfigurationProperty ( "Database", DefaultValue = "" )]
        public string Database
        {
            get { return (string)this["Database"]; }
            set { this["Database"] = value; }
        }

        [ConfigurationProperty("CharSet", DefaultValue = "utf8")]
        public string CharSet
        {
            get { return (string)this["CharSet"]; }
            set { this["CharSet"] = value; }
        }

        [ConfigurationProperty("TrustedConnection", DefaultValue = false)]
        public virtual bool TrustedConnection
        {
            get { return false; }
            set { }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class LastDBPlatformSection : ConfigurationSection
    {
        [ConfigurationProperty("LastDBPlatform", DefaultValue = DatabasePlatformType.SQLServer)]
        public DatabasePlatformType LastDBPlatform
        {
            get
            {
                DatabasePlatformType dpt;
                if (!Enum.TryParse(this["LastDBPlatform"]?.ToString(), out dpt))
                {
                    dpt = DatabasePlatformType.SQLServer;
                }
                return dpt;
            }
            set { this["LastDBPlatform"] = value; }
        }

        public LastDBPlatformSection(DatabasePlatformType platformType) : base()
        {
            LastDBPlatform = platformType;
        }
    }
}
