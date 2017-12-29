using System.Configuration;

namespace CTDDJYDS.Database.Common
{
    public class ConfigurationSection_SQLSever : DBConfigurationSection
    {
        public ConfigurationSection_SQLSever()
        {
            this.User = "sa";
            this.SID = "MSSQLSERVER";
        }

        [ConfigurationProperty("TrustedConnection", DefaultValue = true)]
        public override bool TrustedConnection
        {
            get { return (bool)this["TrustedConnection"]; }
            set { this["TrustedConnection"] = value; }
        }
    }
}