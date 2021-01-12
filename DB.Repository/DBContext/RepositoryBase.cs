using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Repository.DBContext
{
    public class RepositoryBase<T> : SimpleClient<T> where T : class, new()
    {
        public RepositoryBase(string connectStr, ISqlSugarClient context = null) : base(context)//注意这里要有默认值等于null
        {
            if (context == null)
            {
                DbType dbt = DbType.Oracle;
               
                base.Context = new SqlSugarClient(new ConnectionConfig()
                {

                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConnectionString = connectStr,
                    DbType = dbt
                });
            }
        }

        /// <summary>
        /// 扩展方法，自带方法不能满足的时候可以添加新方法
        /// </summary>
        /// <returns></returns>
        public List<T> CommQuery(string json)
        {
            //base.Context.Queryable<T>().ToList();可以拿到SqlSugarClient 做复杂操作
            return null;
        }
    }
}
