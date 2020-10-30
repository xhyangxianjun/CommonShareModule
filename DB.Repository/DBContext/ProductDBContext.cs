using DB.Repository.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Repository.DBContext
{
    public class ProductDBContext : DbContext<ProductModel>
    {
        public SimpleClient<ProductModel> StudentDb { get { return new SimpleClient<ProductModel>(Db); } }//用来处理Student表的常用操作
    }
}
