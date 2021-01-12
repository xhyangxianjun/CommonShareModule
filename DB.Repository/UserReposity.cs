using DB.Repository;
using DB.Repository.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Repository
{
    public class UserReposity : RepositoryBase<SE_USER>
    {
        public UserReposity(string connectStr) : base(connectStr)//注意这里要有默认值等于null
        {
            
        }
    }
}
