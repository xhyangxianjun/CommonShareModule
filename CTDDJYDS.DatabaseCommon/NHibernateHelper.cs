using NHibernate;
using NHibernate.Cfg;
using System;
using System.Xml;

namespace CTDDJYDS.Database.Common
{
    internal sealed class NHibernateHelper
    {
        private static ISessionFactory sessionFactory;
        private static ISession currentSession;
        internal static bool Connected;

//        static NHibernateHelper ( )
//        {
//            try
//            {   
//                sessionFactory = new Configuration ( ).Configure ( ).BuildSessionFactory ( );

//                /*

//                string xml =
//    @"<?xml version='1.0' encoding='utf-8' ?>
//<hibernate-configuration xmlns='urn:nhibernate-configuration-2.2'>
//    <session-factory name='NHibernate.Test'>
//      <property name='connection.provider'>NHibernate.Connection.DriverConnectionProvider</property>
//      <property name='connection.driver_class'>NHibernate.Driver.MySqlDataDriver</property>
//      <property name='connection.connection_string'>
//        Server=localhost;User Id=root;Password=nothing;Database=v_database;Pooling=False
//      </property>
//      <property name='dialect'>NHibernate.Dialect.MySQLDialect</property>
//      <mapping assembly='EDM.Database'/>
//    </session-factory>
//</hibernate-configuration>";


//                XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);

//                sessionFactory = new Configuration().Configure(xtr).BuildSessionFactory();

//                */
                
//                //sessionFactory = new Configuration ( ).Configure().AddAssembly ( "EDM.Database" ).BuildSessionFactory ( );

//                //Configuration cfg = new Configuration ( );
                
//                //cfg.AddAssembly ( "EDM.Database" );
//                //cfg = cfg.Configure ( );
//                //sessionFactory = cfg.BuildSessionFactory ( );
//            }
//            catch ( Exception  )
//            {
//            }

        //        }

        #region save the nhibernate xml configuration -- for reference
//        <configSections>
//    <!--section name="hibernate-configuration" type="NHibernate.Cfg.ConfigurationSectionHandler, NHibernate" /-->
//  </configSections>
  
//  <!--Config for Firebird-->
//  <!--runtime>
//    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
//      <qualifyAssembly partialName="FirebirdSql.Data.FirebirdClient"
//          fullName="FirebirdSql.Data.FirebirdClient, Version=2.0.1.0, Culture=neutral, PublicKeyToken=3750abcc3150b00c" />
//    </assemblyBinding>
//  </runtime-->
//  <!--hibernate-configuration  xmlns="urn:nhibernate-configuration-2.2" >
//    <session-factory name="NHibernate.Test">
//      <property name="connection.provider">NHibernate.Connection.DriverConnectionProvider</property>
//      <property name="connection.driver_class">NHibernate.Driver.FirebirdClientDriver</property>
//      <property name="connection.isolation">ReadCommitted</property>
//      <property name="connection.connection_string">
//        data source=localhost;
//        user id=sysdba;
//        password=123456;
//        initial catalog=G:\Database\Firebird\V_DATABASE.FDB
//      </property>
//      <property name="show_sql">false</property>
//      <property name="dialect">NHibernate.Dialect.FirebirdDialect</property>
//      <property name="use_outer_join">true</property>
//      <property name="command_timeout">444</property>
//      <property name="query.substitutions">true 1, false 0, yes 1, no 0</property>
//      <mapping assembly="EDM.Database" />
//    </session-factory>
//  </hibernate-configuration-->
  
  
//<!--Config for MySQL-->
//  <!--hibernate-configuration  xmlns="urn:nhibernate-configuration-2.2" >
//    <session-factory name="NHibernate.Test">
//      <property name="connection.provider">NHibernate.Connection.DriverConnectionProvider</property>
//      <property name="connection.driver_class">NHibernate.Driver.MySqlDataDriver</property>
//      <property name="connection.connection_string">
//        Server=localhost;User Id=root;Password=123456;Database=v_database;Pooling=False
//      </property>
//      <property name="dialect">NHibernate.Dialect.MySQLDialect</property>
//      <mapping assembly="EDM.Database"/>
//    </session-factory>
//  </hibernate-configuration-->
        #endregion

        public static bool RedirectSessionFactory( string nhConfig, out string strException)
        {
            XmlTextReader xtr = null;
            try
            {
                strException = string.Empty;
                if (currentSession != null && currentSession.IsOpen )
                {
                    CloseSession();
                    currentSession = null;
                }
                if ( sessionFactory != null )
                {
                    CloseSessionFactory();
                    sessionFactory = null;
                }
                
                
                xtr = new XmlTextReader(nhConfig, XmlNodeType.Document, null );
                sessionFactory = new Configuration().Configure(xtr).BuildSessionFactory();
                return true;
                
            }
            catch ( Exception ex )
            {
                strException = string.Format("Exception in NHibernateHelper::RedirectSessionFactory where \n{0}", ex.StackTrace);
                Connected = false;
                return false;
            }
            finally
            {
                if (xtr != null)
                {
                    xtr.Close();
                }
            }
        }

        public static IStatelessSession CreateStatelessSession()
        {
            if (sessionFactory != null)
            {
                return sessionFactory.OpenStatelessSession();
            }
            else
            {
                return null;
            }
        }

        public static ISession GetCurrentSession ( )
        {
            if (currentSession == null)
            {
                try
                {   
                    if (sessionFactory != null)
                    {
                        currentSession = sessionFactory.OpenSession();
                    }
                }
                catch ( Exception  )
                {
                }
            }
            return currentSession;
        }

        public static void CloseSession ( )
        {
            if (currentSession != null)
            {
                currentSession.Close ( );
                currentSession = null;
            }

        }
        public static void CloseSessionFactory ( )
        {
            if (sessionFactory != null)
            {
                sessionFactory.Close ( );
            }
        }
    }
}
