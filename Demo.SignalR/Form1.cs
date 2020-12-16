using Net.SignalRs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo.SignalR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SignalRServer _server = null;
        private void button1_Click(object sender, EventArgs e)
        {
            var signalrUrl = @"http://localhost:12345";
            _server = new SignalRServer();
            _server.ServerStart(signalrUrl);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_server!=null)
                _server.StopServer();
        }
        SignalRClient _Client = null;
        private void button2_Click(object sender, EventArgs e)
        {
            _Client = new SignalRClient();
            _Client.InitHub("");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _Client.InvokeMethod("Hello", "qi");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MyHub.Broadcast("服务端发送数据测试");
        }
    }
}
