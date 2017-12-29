using CTDDJYDS.CommonModule;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;


namespace CTDDJYDS.Database.Common
{
    public static class DatabaseHelper
    {
        static IDBPlatformCommon platform;
        static readonly object lockObj = new object();
        static bool isInitNHibernate = false;
        static Assembly databaseAssembly;

        static DatabaseHelper()
        {}

        public static void SetDatabaseAssembly(Assembly asm)
        {
            databaseAssembly = asm;
        }

        public static void RollbackTransaction(ITransaction transac)
        {
            try
            {
                if (transac != null && transac.IsActive)
                {
                    transac.Rollback();
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Log(ex);
            }
        }

        public static int SPExecuteNonQuery(string spName, List<ParamPair> ppList)
        {
            ProDataSource pf = platform as ProDataSource;
            if (pf == null)
            {
                return 0;
            }
            IDbConnection conn = pf.GetConnection();

            if (conn == null)
            {
                return 0;
            }
            try
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format("{0}.{1}", conn.Database.ToLower(), GetSPNameDialect(spName));
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (ppList != null && ppList.Count > 0)
                    {
                        foreach (ParamPair pp in ppList)
                        {
                            IDbDataParameter p = cmd.CreateParameter();
                            pp.AssignParameter(p);
                            cmd.Parameters.Add(p);
                        }
                    }

                    if (cmd.Connection == null)
                    {
                        cmd.Connection = conn;
                    }

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    using (IDbTransaction tran = conn.BeginTransaction())
                    {

                        try
                        {
                            cmd.CommandTimeout = 600;//360,240
                            cmd.Transaction = tran;
                            int ret = cmd.ExecuteNonQuery();
                            try
                            {
                                tran.Commit();
                                if (pf.DBPlatformType.IsSupportedSQLServerPlatform())
                                {
                                    ret = 1;
                                }
                            }
                            catch
                            {
                                ret = 0;
                            }

                            return ret;
                        }
                        catch (System.Exception ex)
                        {
                            LogHelper.Log(ex);
                            tran.Rollback();
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return 0;
            }   
        }

        public static List<object> GetEntities(string query, int takeCount = -1)
        {
            List<object> lst = new List<object>();
            lock (lockObj)
            {
                ISession s = NHibernateHelper.GetCurrentSession();
                //using (ITransaction t = s.BeginTransaction())
                //{
                try
                {
                    var q = s.CreateQuery(query);
                    if (takeCount > 0)
                    {
                        q = q.SetMaxResults(takeCount);
                    }
                    q.List(lst);
                    //t.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                    //if(t.IsActive)
                    //{
                    //    t.Rollback();
                    //}
                }
                //}
                return lst;
            }
        }

        public static IList<T> GetEntities<T>(string query, int takeCount = -1)
        {
            IList<T> lst = null;
            lock (lockObj)
            {
                ISession s = NHibernateHelper.GetCurrentSession();
                try
                {
                    var q = s.CreateQuery(query);
                    if (takeCount > 0)
                    {
                        q = q.SetMaxResults(takeCount);
                    }
                    lst = q.List<T>();
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                }
                if (lst == null)
                {
                    lst = new List<T>();
                }
                return lst;
            }
        }

        public static List<object> GetSqlEntities(string query)
        {
            List<object> lst = new List<object>();
            lock (lockObj)
            {
                ISession s = NHibernateHelper.GetCurrentSession();
                //using (ITransaction t = s.BeginTransaction())
                //{
                try
                {
                    s.CreateSQLQuery(query).List(lst);
                    //t.Commit();
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                    //if(t.IsActive)
                    //{
                    //    t.Rollback();
                    //}
                }
                //}
                return lst;
            }
        }

        /// <summary>
        /// Get the bounded results
        /// </summary>
        /// <param name="query">query string</param>
        /// <param name="maxResult">maximum number of results in the result sets</param>
        /// <param name="firstResult">start offset of the results in the result sets</param>
        /// <returns></returns>
        public static List<object> GetBoundedEntities(string query, int maxResult, int firstResult = 0)
        {
            List<object> lst = new List<object>();
            lock (lockObj)
            {
                ISession s = NHibernateHelper.GetCurrentSession();
                try
                {
                    IQuery iQuery = s.CreateQuery(query);
                    iQuery.SetFirstResult(firstResult);
                    iQuery.SetMaxResults(maxResult);
                    iQuery.List(lst);
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                }
                return lst;
            }
        }

        /// <summary>
        /// Get the bounded results
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="query">query string</param>
        /// <param name="maxResult">maximum number of results in the result sets</param>
        /// <param name="firstResult">start offset of the results in the result sets</param>
        /// <returns></returns>
        public static IList<T> GetBoundedEntities<T>(string query, int maxResult, int firstResult = 0)
        {
            IList<T> lst = null;
            lock (lockObj)
            {
                ISession s = NHibernateHelper.GetCurrentSession();
                try
                {
                    IQuery iQuery = s.CreateQuery(query);
                    iQuery.SetFirstResult(firstResult);
                    iQuery.SetMaxResults(maxResult);
                    lst = iQuery.List<T>();
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                }
                if (lst == null)
                {
                    lst = new List<T>();
                }
                return lst;
            }
        }

        public static bool GetOneEntity(string query, out object obj)
        {
            List<object> objList = GetBoundedEntities(query, 1);
            if (objList.Count > 0)
            {
                obj = objList[0];
                return true;
            }
            else
            {
                obj = null;
                return false;
            }
        }

        public static bool GetOneEntity<T>(string query, out T obj)
        {
            IList<T> objList = GetBoundedEntities<T>(query, 1);
            if (objList.Count > 0)
            {
                obj = objList[0];
                return true;
            }
            else
            {
                obj = default(T);
                return false;
            }
        }

        public static void ClearCache()
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            session.Clear();
        }

        public static bool Insert(object obj, out string msgException)
        {
            msgException = string.Empty;
            if (obj == null)
                return false;

            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                ITransaction transc = null;

                try
                {
                    transc = session.BeginTransaction();
                    session.Save(obj);
                    transc.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    RollbackTransaction(transc);
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return false;
                }
                finally
                {
                    if (transc != null)
                    {
                        transc.Dispose();
                    }
                }
            }
        }

        public static bool InsertStateless(List<object> objs, out string msgException)
        {
            msgException = string.Empty;
            if (objs == null)
                return false;

            lock (lockObj)
            {
                using (var session = NHibernateHelper.CreateStatelessSession())
                {
                    session.SetBatchSize(objs.Count);
                    ITransaction transc = null;                   
                    try
                    {
                        transc = session.BeginTransaction();
                        var sw = System.Diagnostics.Stopwatch.StartNew();
                        Console.WriteLine($"start insert operation");
                        foreach (object obj in objs)
                        {
                            session.Insert(obj);
                            //if (index % 16 == 0)
                            //{
                            //    session.Flush();
                            //    session.Clear();
                            //}
                        }
                        if (transc != null)
                            transc.Commit();

                        Console.WriteLine($"insert num: {objs.Count}, elapsed: {sw.Elapsed}");

                        return true;
                    }
                    catch (Exception ex)
                    {      
                        RollbackTransaction(transc);
                        msgException = ex.StackTrace;
                        LogHelper.Log(ex);
                        return false;
                    }
                    finally
                    {
                        if (transc != null)
                        {
                            transc.Dispose();
                        }
                    }
                }
                  
            }
        }

        public static bool Insert(List<object> objs, out string msgException)
        {
            msgException = string.Empty;
            if (objs == null)
                return false;

            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                ITransaction transc = null;

                int index = 0;
                try
                {                    
                    transc = session.BeginTransaction();
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    Console.WriteLine($"start insert operation");
                    foreach (object obj in objs)
                    {
                        index++;
                        session.Save(obj);
                        if (index % 32 == 0)
                        {
                            session.Flush();
                            session.Clear();
                        }
                    }
                    if (transc != null)
                        transc.Commit();

                    Console.WriteLine($"insert num: {objs.Count}, elapsed: {sw.Elapsed}");

                    return true;
                }
                catch (Exception ex)
                {
                    RollbackTransaction(transc);
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return false;
                }
                finally
                {
                    if (transc != null)
                    {
                        transc.Dispose();
                    }
                }
            }
        }

        public static bool Delete(object obj, out string msgException)
        {
            msgException = string.Empty;
            if (obj == null)
                return false;

            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                ITransaction transc = null;

                try
                {
                    transc = session.BeginTransaction();
                    session.Delete(obj);
                    transc.Commit();
                    return true;
                }
                catch (NHibernate.StaleStateException ssex)
                {
                    RollbackTransaction(transc);
                    msgException = ssex.StackTrace;
                    LogHelper.Log(ssex);
                    return true;
                }
                catch (Exception ex)
                {
                    RollbackTransaction(transc);
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return false;
                }
                finally
                {
                    if (transc != null)
                    {
                        transc.Dispose();
                    }
                }
            }
        }

        public static bool Delete(List<object> objs, out string msgException)
        {
            msgException = string.Empty;
            if (objs == null)
                return false;

            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                ITransaction transc = null;

                try
                {
                    transc = session.BeginTransaction();
                    foreach (object obj in objs)
                    {
                        session.Delete(obj);
                    }
                    transc.Commit();
                    return true;
                }
                catch (NHibernate.StaleStateException ssex)
                {
                    RollbackTransaction(transc);
                    msgException = ssex.StackTrace;
                    LogHelper.Log(ssex);
                    return true;
                }
                catch (Exception ex)
                {
                    RollbackTransaction(transc);
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return false;
                }
                finally
                {
                    if (transc != null)
                    {
                        transc.Dispose();
                    }
                }
            }
        }

        public static bool Update(object obj, out string msgException, bool isException = false)
        {
            msgException = string.Empty;
            if (obj == null)
                return false;

            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                ITransaction transc = null;

                try
                {
                    transc = session.BeginTransaction();
                    if (isException)
                        obj = session.Merge(obj);
                    session.Update(obj);
                    transc.Commit();
                    return true;
                }
                catch (Exception ex)
                {       
                    RollbackTransaction(transc);
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return false;
                }
                finally
                {
                    if (transc != null)
                    {
                        transc.Dispose();
                    }
                }
            }
        }

        public static bool Update(List<object> objs, out string msgException)
        {
            msgException = string.Empty;
            if (objs == null)
                return false;

            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                ITransaction transc = null;

                try
                {
                    transc = session.BeginTransaction();
                    foreach (object obj in objs)
                    {
                        session.Merge(obj);
                    }
                    transc.Commit();
                    return true;
                }
                catch (Exception ex)
                {     
                    RollbackTransaction(transc);
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return false;
                }
                finally
                {
                    if (transc != null)
                    {
                        transc.Dispose();
                    }
                }
            }
        }

        public static bool Refresh(object obj, out string msgException)
        {
            msgException = string.Empty;
            if (obj == null)
                return false;

            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();

                try
                {
                    session.Refresh(obj);
                    return true;
                }
                catch (Exception ex)
                {
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return false;
                }
                finally
                {

                }
            }
        }

        public static bool Refresh(List<object> objs, out string msgException)
        {
            msgException = string.Empty;
            if (objs == null)
                return false;

            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();

                try
                {
                    foreach (object obj in objs)
                    {
                        session.Refresh(obj);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return false;
                }
                finally
                {

                }
            }
        }

        public static bool HybridOps(List<object> deletes, List<object> updates, List<object> inserts, out string msgException)
        {
            msgException = string.Empty;
            if (deletes == null && updates == null && inserts == null)
                return false;
            ISession session = NHibernateHelper.GetCurrentSession();
            ITransaction transc = null;

            try
            {
                transc = session.BeginTransaction();
                if (deletes != null)
                {
                    foreach (object delete in deletes)
                    {
                        session.Delete(delete);
                    }
                }

                if (updates != null)
                {
                    foreach (object update in updates)
                    {
                        session.Update(update);
                    }
                }

                if (inserts != null)
                {
                    foreach (object insert in inserts)
                    {
                        session.Save(insert);
                    }
                }

                transc.Commit();
                return true;
            }
            catch (Exception ex)
            {
                RollbackTransaction(transc);
                msgException = ex.StackTrace;
                LogHelper.Log(ex);
                return false;
            }
            finally
            {
                if (transc != null)
                {
                    transc.Dispose();
                }
            }
        }

        public static object Load(Type type, object id, out string msgException)
        {
            ISession session = NHibernateHelper.GetCurrentSession();
            object obj = null;
            msgException = string.Empty;
            try
            {
                obj = session.Load(type, id);
                return obj;
            }
            catch (Exception ex)
            {
                msgException = ex.StackTrace;
                LogHelper.Log(ex);
                return null;
            }
        }

        /// <summary>
        ///  IsObjectExist
        /// </summary>
        /// <param name="queryString"> SELECT VRF_ID FROM v_recordingfile WHERE VRF_GUID = @guid</param>
        /// <param name="idColumnName">VRF_ID</param>
        /// <param name="msgException"></param>
        /// <returns></returns>
        public static bool IsObjectExist(string queryString, string idColumnName, out string msgException)
        {
            IList result = null;
            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                msgException = string.Empty;
                try
                {
                    ISQLQuery q = session.CreateSQLQuery(queryString);
                    q.AddScalar(idColumnName, NHibernateUtil.Int32);
                    result = q.List();
                    return result != null && result.Count > 0;
                }
                catch (Exception ex)
                {
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return false;
                }
       
            }
        }

        /// <summary>
        ///  IsObjectExist
        /// </summary>
        /// <param name="queryString"> SELECT VRF_ID FROM v_recordingfile WHERE VRF_GUID = @guid</param>
        /// <param name="idColumnName">VRF_ID</param>
        /// <param name="msgException"></param>
        /// <returns></returns>
        public static bool IsObjectExist(string queryString, string idColumnName)
        {
            IList result = null;
            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                try
                {
                    ISQLQuery q = session.CreateSQLQuery(queryString);
                    q.AddScalar(idColumnName, NHibernateUtil.Int32);
                    result = q.List();
                    return result != null && result.Count > 0;
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                    return false;
                }
            }
        }

        public static IList GetObjectList(string sql, string filedname, out string msgException)
        {
            IList result = null;
            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                msgException = string.Empty;
                try
                {
                    ISQLQuery q = session.CreateSQLQuery(sql);
                    q.AddScalar(filedname, NHibernateUtil.String);
                    result = q.List();
                    return result;
                }
                catch (Exception ex)
                {
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                    return null;
                }
            }
        }


        public static IList CreateSQLQuery(string queryString, List<ScalarPair> spList, out string msgException)
        {
            IList result = null;
            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                msgException = string.Empty;
                try
                {
                    ISQLQuery q = session.CreateSQLQuery(queryString);
                    if (spList != null)
                    {
                        foreach (ScalarPair sp in spList)
                        {
                            q.AddScalar(sp.ScalarName, sp.SType);
                        }
                    }
                    result = q.List();
                }
                catch (Exception ex)
                {
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                }
                finally
                {
                }
                return result;
            }
        }


        public static List<object> CreateSQLQuery(string query)
        {
            List<object> lst = new List<object>();
            lock (lockObj)
            {
                ISession s = NHibernateHelper.GetCurrentSession();
                try
                {
                    s.CreateSQLQuery(query).List(lst);
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                }
                return lst;
            }
        }

        public static List<object> CreateSQLQuery(string query, Type type)
        {
            List<object> lst = new List<object>();
            lock (lockObj)
            {
                ISession s = NHibernateHelper.GetCurrentSession();
                try
                {
                    s.CreateSQLQuery(query).AddEntity(type).List(lst);
                }
                catch (Exception ex)
                {
                    LogHelper.Log(ex);
                }
                return lst;
            }
        }

        public static IList CreateSQLQuery(string queryString, Type type, out string msgException)
        {
            IList result = null;
            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                msgException = string.Empty;
                try
                {      
                    ISQLQuery q = session.CreateSQLQuery(queryString).AddEntity(type);
                    result = q.List();
                }
                catch (Exception ex)
                {
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                }            
                return result;
            }
        }

        public static IList CreateMultiQuery(List<string> queryString, out string msgException)
        {
            IList result = null;
            lock (lockObj)
            {
                ISession session = NHibernateHelper.GetCurrentSession();
                msgException = string.Empty;
                try
                {               
                    IMultiQuery mq = session.CreateMultiQuery();
                    foreach (string query in queryString)
                    {
                        IQuery q = session.CreateQuery(query);
                        mq.Add(q);
                    }
                    result = mq.List();
                }
                catch (Exception ex)
                {
                    msgException = ex.StackTrace;
                    LogHelper.Log(ex);
                }
                return result;
            }
        }

        public static bool RedirectNHibernateConfiguration(string nhConfig, out string strException)
        {
            strException = string.Empty;
            if (!NHibernateHelper.RedirectSessionFactory(nhConfig, out strException))
                return false;
            bool result = true;
            ISession session = NHibernateHelper.GetCurrentSession();
            DbTransaction transc = null;
            try
            {              
                DbConnection connection = session.Connection as DbConnection;
                transc = connection.BeginTransaction();
                DbCommand cmd = connection.CreateCommand();
                cmd.Transaction = transc;

                if (!string.IsNullOrEmpty(connection.Database))
                {                  
                    if (connection is SqlConnection)
                    {
                        #region create the information table on master database for convenience
                     
                        RunEmbeddedScript("ScriptPath", cmd);
                        #endregion
                    }
                }

                transc.Commit();

                NHibernateHelper.Connected = result;

                return result;
            }
            catch (Exception ex)
            {
                strException = string.Format("Exception in DatabaseHelper::RedirectNHibernateConfiguration where \n{0}", ex.StackTrace);
                NHibernateHelper.Connected = false;
                LogHelper.Log("Redirect NHibernate Configuration failed");
                LogHelper.Log(ex);
            }
            finally
            {
                if (transc != null)
                {
                    transc.Dispose();
                }
            }
            return false;
        }      

        public static void NHFinalize()
        {
            NHibernateHelper.CloseSession();
        }

        // the returned connection seems to be verified internal, need to be treated carefully
        public static IDbConnection GlobalConnection
        {
            get
            {
                try
                {
                    if (NHibernateHelper.GetCurrentSession() != null &&
                        NHibernateHelper.Connected &&
                        NHibernateHelper.GetCurrentSession().Connection != null)
                        return NHibernateHelper.GetCurrentSession().Connection;
                    else
                        return null;
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
        }

        public static IDBPlatformCommon Platform
        {
            get
            {
                return platform;
            }
            internal set
            {
                if (null != value)
                {
                    platform = value;
                }
            }
        }

        public static bool IsInitializingNHibernate
        {
            get
            {
                return isInitNHibernate;
            }
        }

        public static string GetDBNameDialect(string dbName)
        {
            if (platform != null && platform.DBPlatformType.IsSupportedSQLServerPlatform())
            {
                return string.Format("{0}.dbo", dbName);
            }
            return dbName;
        }

        public static string GetSPNameDialect(string spName)
        {
            if (platform != null && platform.DBPlatformType.IsSupportedSQLServerPlatform())
            {
                return string.Format("dbo.u{0}", spName);
            }
            return spName;
        }

        public static string GetSPParamNameDialet(string paramName)
        {
            if (platform != null && platform.DBPlatformType.IsSupportedSQLServerPlatform())
            {
                return string.Format("@{0}", paramName);
            }
            return paramName;
        }

        public static void UpdateFromDatatable(string sql, DataTable dt)
        {
            if (platform == null || dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            try
            {
                IDbConnection conn = (platform as ProDataSource).GetConnection();
                if (conn is SqlConnection)
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn as SqlConnection))
                    {
                        SqlCommandBuilder builder = new SqlCommandBuilder(da);
                        builder.GetInsertCommand();
                        foreach (DataRow r in dt.Rows)
                        {
                            r.SetAdded();
                        }
                        da.Update(dt);
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Log(ex);
            }
        }

        public static void RunEmbeddedScript(string path, IDbCommand cmd, bool executeStatementOneByOne = true)
        {
            using (SQLScriptParser parser = new SQLScriptParser())
            {
                parser.LoadScriptEx(path);

                if (executeStatementOneByOne)
                {
                    string cmdText = parser.NextCommand();
                    while (!string.IsNullOrEmpty(cmdText))
                    {
                        cmd.CommandText = cmdText;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                        }
                        cmdText = parser.NextCommand();
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    string cmdText = parser.NextCommand();
                    while (!string.IsNullOrEmpty(cmdText))
                    {
                        sb.AppendFormat("{0};", cmdText);
                        cmdText = parser.NextCommand();
                    }

                    try
                    {
                        if (sb.Length > 0)
                        {
                            cmd.CommandText = sb.ToString();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        #region General interface, database operation irrelavant
        public static BackupItemEx FindBackupItemEx(List<BackupItemEx> items, Type objectType)
        {
            BackupItemEx bi = null;
            if (items != null)
            {
                foreach (BackupItemEx item in items)
                {
                    if (item.ItemType == objectType)
                    {
                        bi = item;
                        break;
                    }
                }
            }
            return bi;
        }

        public static List<PropertyInfo> GetColumnPropertyInfo(string tableName, Type dbObjType)
        {
            PropertyInfo[] infoArray = dbObjType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            var multiMapping = dbObjType.GetCustomAttributes(typeof(MultiMappingAttribute), false).Length > 0;
            if (multiMapping)
            {
                SortedDictionary<int, PropertyInfo> propertyInfoDict = new SortedDictionary<int, PropertyInfo>();
                foreach (var pi in infoArray)
                {
                    var colIndexAttr = pi.GetCustomAttributes(typeof(DataColumnIndexAttribute), false).FirstOrDefault() as DataColumnIndexAttribute;
                    if (colIndexAttr != null)
                    {
                        propertyInfoDict[colIndexAttr.Index] = pi;
                    }
                }

                return propertyInfoDict.Values.ToList();
            }

            Dictionary<int, PropertyInfo> columnPropertyInfo = new Dictionary<int, PropertyInfo>();
            string prefix = "CTDDJYDS.Database.";
            string location = "Common";

            location = prefix + location + tableName + ".hbm.xml";

            Dictionary<string, PropertyInfo> propertyDic = new Dictionary<string, PropertyInfo>(infoArray.Length);
            foreach (PropertyInfo info in infoArray)
            {
                propertyDic[info.Name] = info;
            }

            try
            {
                Assembly assem = databaseAssembly ?? Assembly.GetExecutingAssembly();
                using (Stream hbmStream = assem.GetManifestResourceStream(location))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(hbmStream);

                    int colIndex = 0;
                    XmlNode classNode = xmlDoc.DocumentElement.ChildNodes[0];
                    foreach (XmlNode propertyNode in classNode.ChildNodes)
                    {
                        string propertyName = propertyNode.Attributes[0].Value;
                        PropertyInfo info = null;
                        if (propertyDic.TryGetValue(propertyName, out info))
                        {
                            XmlNode columnNode = propertyNode.ChildNodes[0];
                            columnPropertyInfo[colIndex++] = info;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Log(ex);
            }

            return columnPropertyInfo.Values.ToList();
        }
        #endregion

        public static bool IsSupportedSQLServerPlatform(this DatabasePlatformType type)
        {
            return type == DatabasePlatformType.SQLServer;
        }
    }

    public class ParamPair
    {
        private string Name;
        private object Param;
     /*   private DbType DbType;*/
        private ParameterDirection Direction;

        public ParamPair(string name, object val, /*DbType type, */ParameterDirection dir)
        {
            this.Name = name;
            this.Param = val;
      /*      this.DbType = type;*/
            this.Direction = dir;
        }

        public void AssignParameter(IDbDataParameter parameter)
        {
            parameter.Value = Param;
            parameter.ParameterName = DatabaseHelper.GetSPParamNameDialet(Name);
            parameter.Direction = Direction;
          /*  parameter.DbType = DbType;*/
        }
    }

    public static class SPSupport
    {
        public static readonly string sp_delete_entry = "sp_delete_entry";
        public static readonly string sp_delete_factory = "sp_delete_factory";
        public static readonly string sp_delete_machine = "sp_delete_machine";
        public static readonly string sp_delete_point = "sp_delete_point";
        public static readonly string sp_delete_route = "sp_delete_route";
        public static readonly string sp_delete_machine_by_factory = "sp_delete_machine_by_factory";
        public static readonly string sp_copy_entry = "sp_copy_entry";
        public static readonly string sp_copy_factory = "sp_copy_factory";
        public static readonly string sp_copy_machine = "sp_copy_machine";
        public static readonly string sp_copy_point = "sp_copy_point";
        public static readonly string sp_copy_route = "sp_copy_route";

        public static readonly string sp_create_tmptable_by_factory = "sp_create_tmptable_by_factory";
        public static readonly string sp_create_tmptable_by_machine = "sp_create_tmptable_by_machine";
        public static readonly string sp_create_tmptable_by_point = "sp_create_tmptable_by_point";
        public static readonly string sp_create_tmptable_by_entry = "sp_create_tmptable_by_entry";

        public static readonly string sp_param_guid = "guid";
        public static readonly string sp_param_factory_guid = "factoryguid";
        public static readonly string sp_param_machine_guid = "machineguid";
        public static readonly string sp_param_point_guid = "pointguid";
        public static readonly string sp_param_entry_guid = "entryguid";
        public static readonly string sp_param_route_guid = "routeguid";
        public static readonly string sp_param_isAppendCopyTag = "isAppendCopyTag";

        public static readonly string sp_clean_param_tacho_set = "sp_clean_param_tacho_set";
    }

    public class ScalarPair 
    {
        public string ScalarName;
        public NHibernate.Type.NullableType SType;

        internal ScalarPair(string name, NHibernate.Type.NullableType stype)
        {
            this.ScalarName = name;
            this.SType = stype;
        }

        private static Dictionary<string, List<ScalarPair>> dicSPCache;

        public const string keyRecInfo = "RecInfo";
        public const string keyPadRecInfo = "PadRecInfo";
        public const string keyBalanceInfo = "BalanceInfo";
        public const string keySRecInfo = "SRecInfo";


        public const string scalarGuid = "Guid";
        public const string scalarIsTriAxis = "IsTriAxis";
        public const string scalarRecCount = "RecCount";
        public const string scalarMaxDate= "MaxDate";
        public const string scalarMinDate = "MinDate";
        public const string scalarIsRoute = "IsRoute";

        public static List<ScalarPair> GetScalarPair(string keyword)
        {
            if(dicSPCache == null)
            {
                dicSPCache = new Dictionary<string, List<ScalarPair>>();
            }

            List<ScalarPair> spList = null;

            if(dicSPCache.TryGetValue(keyword,out spList))
            {
                return spList;
            }
            else 
            {
                spList = new List<ScalarPair>();
            }

            switch(keyword)
            {
                case keyRecInfo:
                    spList.AddRange(
                        new ScalarPair[]{
                            new ScalarPair(scalarGuid, NHibernateUtil.String),
                            new ScalarPair(scalarIsTriAxis,NHibernateUtil.Int16),
                            new ScalarPair(scalarRecCount,NHibernateUtil.Int32),
                            new ScalarPair(scalarMaxDate,NHibernateUtil.DateTime),
                            new ScalarPair(scalarMinDate,NHibernateUtil.DateTime),
                            new ScalarPair(scalarIsRoute,NHibernateUtil.Int16)
                            }
                        );
                    break;
                case keyPadRecInfo:
                case keyBalanceInfo:
                case keySRecInfo:
                    spList.AddRange(
                           new ScalarPair[]{
                            new ScalarPair(scalarGuid, NHibernateUtil.String),
                            new ScalarPair(scalarRecCount,NHibernateUtil.Int32),
                            new ScalarPair(scalarMaxDate,NHibernateUtil.DateTime),
                            new ScalarPair(scalarMinDate,NHibernateUtil.DateTime)
                            }
                           );
                    break;
                default:
                    break;
            }

            dicSPCache[keyword] = spList;

            return spList;
        }
    }
    
    [AttributeUsage(AttributeTargets.Property)]
    public class DataColumnIndexAttribute : Attribute
    {
        public int Index { get; private set; }

        public DataColumnIndexAttribute(int index)
        {
            this.Index = index;
        }
    }

    /// <summary>
    /// 此特性表明一个表除了本身对应的映射文件外，还有另外的blob映射文件。
    /// </summary>
    /// <remarks>当一个表有这个Attribute时，应该给它的每个数据库列属性添加<see cref="DataColumnIndexAttribute"/>，
    /// 因为数据库列定义在多个映射文件里，需要强制设置列的index，
    /// 否则save test时保存的数据列顺序会有问题。</remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class MultiMappingAttribute : Attribute
    {
    }
}
