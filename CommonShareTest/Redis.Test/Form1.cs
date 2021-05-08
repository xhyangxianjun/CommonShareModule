using CTDDJYDS.CommonModule;
using DB.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redis.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServiceStackRedis dd = new ServiceStackRedis("127.0.0.1");
            List<UserInfo> userinfoList = new List<UserInfo>();
            userinfoList.Add(new UserInfo() { UserName = "pool_daizhj", Age = 1 });
            userinfoList.Add(new UserInfo() { UserName = "pool_daizhj1", Age = 2 });
            dd.Set<List<UserInfo>>("userinfolist", userinfoList);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            ServiceStackRedis dd = new ServiceStackRedis("127.0.0.1");
            var ddd= dd.Get<List<UserInfo>>("userinfolist");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserReposity ff = new UserReposity("");
           //ff.AsDeleteable
        }
    }

    [Serializable]
    public class UserInfo
    {
        public long Id { set; get; }
        public string UserName { get; set; }
        public int Age { get; set; }
    }
}
