using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperApp
{
    public class DapperOperate
    {

        public  static void OneToOne(string sqlConnectionString)
        {
            List<Customer> userList = new List<Customer>();
            using (IDbConnection conn = SqlConnectionFactory.CreateSqlConnection(DatabaseType.SqlServer, sqlConnectionString))
            {
                string sqlCommandText = @"SELECT c.UserId,c.Username AS UserName,
c.PasswordHash AS [Password],c.Email,c.PhoneNumber,c.IsFirstTimeLogin,c.AccessFailedCount,
c.CreationDate,c.IsActive,r.RoleId,r.RoleName 
    FROM dbo.CICUser c WITH(NOLOCK) 
INNER JOIN CICUserRole cr ON cr.UserId = c.UserId 
INNER JOIN CICRole r ON r.RoleId = cr.RoleId";
                userList = conn.Query<Customer, Role, Customer>(sqlCommandText,
                                                                (user, role) => { user.Role = role; return user; },
                                                                null,
                                                                null,
                                                                true,
                                                                "RoleId",
                                                                null,
                                                                null).ToList();
            }

            if (userList.Count > 0)
            {
                userList.ForEach((item) => Console.WriteLine("UserName:" + item.UserName +
                                                             "----Password:" + item.Password +
                                                             "-----Role:" + item.Role.RoleName +
                                                             "\n"));
            }
        }



        private static void OneToMany(string sqlConnectionString)
        {
            Console.WriteLine("One To Many");
            List<User> userList = new List<User>();

            using (IDbConnection connection = SqlConnectionFactory.CreateSqlConnection(DatabaseType.SqlServer, sqlConnectionString))
            {

                string sqlCommandText3 = @"SELECT c.UserId,
       c.Username      AS UserName,
       c.PasswordHash  AS [Password],
       c.Email,
       c.PhoneNumber,
       c.IsFirstTimeLogin,
       c.AccessFailedCount,
       c.CreationDate,
       c.IsActive,
       r.RoleId,
       r.RoleName
FROM   dbo.CICUser c WITH(NOLOCK)
       LEFT JOIN CICUserRole cr
            ON  cr.UserId = c.UserId
       LEFT JOIN CICRole r
            ON  r.RoleId = cr.RoleId";

                var lookUp = new Dictionary<int, User>();
                userList = connection.Query<User, Role, User>(sqlCommandText3,
                    (user, role) =>
                    {
                        User u;
                        if (!lookUp.TryGetValue(user.UserId, out u))
                        {
                            lookUp.Add(user.UserId, u = user);
                        }
                        u.Role.Add(role);
                        return user;
                    }, null, null, true, "RoleId", null, null).ToList();
                var result = lookUp.Values;
            }

            if (userList.Count > 0)
            {
                userList.ForEach((item) => Console.WriteLine("UserName:" + item.UserName +
                                             "----Password:" + item.Password +
                                             "-----Role:" + item.Role.First().RoleName +
                                             "\n"));

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("No Data In UserList!");
            }
        }

        public static void InsertObject(string sqlConnectionString)
        {
            string sqlCommandText = @"INSERT INTO CICUser(Username,PasswordHash,Email,PhoneNumber)VALUES(
    @UserName,
    @Password,
    @Email,
    @PhoneNumber
)";
            using (IDbConnection conn = SqlConnectionFactory.CreateSqlConnection(DatabaseType.SqlServer, sqlConnectionString))
            {
                User user = new User();
                user.UserName = "Dapper";
                user.Password = "654321";
                user.Email = "Dapper@infosys.com";
                user.PhoneNumber = "13795666243";
                int result = conn.Execute(sqlCommandText, user);
                if (result > 0)
                {
                    Console.WriteLine("Data have already inserted into DB!");
                }
                else
                {
                    Console.WriteLine("Insert Failed!");
                }

                Console.ReadLine();
            }
        }

        /// <summary>
        /// 执行多条SQL语句
        /// </summary>
        public static void ExcuteMult(string sqlConnectionString)
        {
            IDbConnection conn = SqlConnectionFactory.CreateSqlConnection(DatabaseType.SqlServer, sqlConnectionString);
            string sql = "select Username from CICUser;select RoleName from CICRole;select RoleId from CICRole";
            using (var multipleReader = conn.QueryMultiple(sql))
            {
                //一次执行N条SQL语句则最多只能调用N次Read方法，否则抛异常:the reader has been disposed.
                //Dapper读取查询结果数据的顺序必须要和查询语句中的查询顺序一致，否则可能读取不到数据
                var schoolList = multipleReader.Read<User>();
                foreach (var s in schoolList)
                {
                    Console.Write(s.UserName + " ");
                }
                Console.WriteLine();
                var studentSchools = multipleReader.Read<Role>();
                foreach (var s in studentSchools)
                {
                    Console.Write(s.RoleName + " ");
                }
                Console.WriteLine();
                var studentNames = multipleReader.Read<Role>();
                foreach (var s in studentNames)
                {
                    Console.Write(s.RoleId + " ");
                }
            }
        }

        public static void BulkInsertObject(string sqlConnectionString)
        {
            using (IDbConnection conn = SqlConnectionFactory.CreateSqlConnection(DatabaseType.SqlServer, sqlConnectionString))
            {
                //批量插入数据
                List<User> users = new List<User>()
                {
                    new User() {UserName="China·BeiJing",Email="清华大学",Password = "654321", PhoneNumber = "13795666243"},
                    new User() {UserName="杭州",Email="浙江大学" ,Password = "654321",PhoneNumber = "13795666243"},
                    new User() {UserName="不知道，US?",Email="哈弗大学" ,Password = "654321",PhoneNumber = "13795666243"}
                };
                //在执行参数化的SQL时，SQL中的参数（如@PhoneNumber可以和数据表中的字段不一致，但要和实体类型的属性PhoneNum相对应）
                int result = conn.Execute("insert into CICUser(Username,PasswordHash,Email,PhoneNumber) values( @UserName,@Password,@Email,@PhoneNumber);", users);
                //通过匿名类型批量插入数据
                //int result = conn.Execute("insert into CICUser(Username,PasswordHash,Email,PhoneNumber) values(@UserName,@Password,@Email,@PhoneNum)",
                //new[] {
                //      new {UserName="杨浦区四平路1239号",Email="同济大学",Password = "654321", PhoneNum = "13795666243"},
                //      new {UserName="英国",Email="剑桥",Password = "654321", PhoneNum = "13795666243"},
                //      new {UserName="美国·硅谷",Email="斯坦福大学",Password = "654321", PhoneNum = "13795666243"}
                //});          
                if (result > 0)
                {
                    Console.WriteLine("Data have already inserted into DB!");
                }
                else
                {
                    Console.WriteLine("Insert Failed!");
                }

                Console.ReadLine();
            }
        }

        /// <summary>
        /// Execute StroedProcedure and get result from return value
        /// </summary>
        /// <param name="sqlConnnectionString"></param>
        public static void ExecuteStoredProcedureWithParms(string sqlConnnectionString)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@UserName", "cooper");
            p.Add("@Password", "123456");
            p.Add("@LoginActionType", null, DbType.Int32, ParameterDirection.ReturnValue);
            using (IDbConnection cnn = SqlConnectionFactory.CreateSqlConnection(DatabaseType.SqlServer,sqlConnnectionString))
            {
                cnn.Execute("dbo.p_validateUser", p, null, null, CommandType.StoredProcedure);
                int result = p.Get<int>("@LoginActionType");
                Console.WriteLine(result);
            }

            Console.ReadLine();
        }

        /// <summary>
        /// Execute StoredProcedure and map result to POCO
        /// </summary>
        /// <param name="sqlConnnectionString"></param>
        public static void ExecuteStoredProcedure(string sqlConnnectionString)
        {
            List<User> users = new List<User>();
            using (IDbConnection cnn = SqlConnectionFactory.CreateSqlConnection(DatabaseType.SqlServer, sqlConnnectionString))
            {
                users = cnn.Query<User>("dbo.p_getUsers",
                                        new { UserId = 2 },
                                        null,
                                        true,
                                        null,
                                        CommandType.StoredProcedure).ToList();
            }
            if (users.Count > 0)
            {
                users.ForEach((user) => Console.WriteLine(user.UserName + "\n"));
            }
            Console.ReadLine();
        }
    }
}
