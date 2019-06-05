using DB.Sqlsugar.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Sqlsugar
{
    public class SqlSugarHelper
    {
        private SqlSugarHelper()
        {
        }
        private static SqlSugarHelper m_Instance = null;
        public static SqlSugarHelper Instance
        {
            get {
                if (m_Instance == null)
                    m_Instance = new SqlSugarHelper();
                return m_Instance;
            }
        }

        public void StartSqlSugarClient()
        {
            SugarClient = new SqlSugarClient(
            new ConnectionConfig()
            {
                ConnectionString = "server=.;uid=sa;pwd=@jhl85661501;database=SqlSugar4XTest",
                DbType = DbType.SqlServer,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放连接，如果存在事务，在事务结束后释放
                InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
            });
            SugarClient.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                SugarClient.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }
        public SqlSugarClient SugarClient { get; set; }

        public void OperateMethod()
        {
            /*查询*/
            var list = SugarClient.Queryable<ProductModel>().ToList();//查询所有
            var getById = SugarClient.Queryable<ProductModel>().InSingle(1);//根据主键查询
            var getByWhere = SugarClient.Queryable<ProductModel>().Where(it => it.ProductOID == 1).ToList();//根据条件查询
            var total = 0;
            var getPage = SugarClient.Queryable<ProductModel>().Where(it => it.ProductOID == 1).ToPageList(1, 2, ref total);//根据分页查询
            //多表查询用法 http://www.codeisbug.com/Doc/8/1124

            /*插入*/
            var data = new ProductModel() { ProductID = "jack" };
            SugarClient.Insertable(data).ExecuteCommand();
            //更多插入用法 http://www.codeisbug.com/Doc/8/1130

            /*更新*/
            var data2 = new ProductModel() { ProductOID = 1, ProductID = "jack" };
            SugarClient.Updateable(data2).ExecuteCommand();
            //更多更新用法 http://www.codeisbug.com/Doc/8/1129

            /*删除*/
            SugarClient.Deleteable<ProductModel>(1).ExecuteCommand();

            //执行完数据库就有这个表了
            SugarClient.CodeFirst.SetStringDefaultLength(200/*设置varchar默认长度为200*/).InitTables(typeof(ProductModel));
        }
    }
}
