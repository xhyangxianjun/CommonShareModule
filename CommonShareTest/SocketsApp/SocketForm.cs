using Net.DotNettySockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SocketApp
{
    public partial class SocketForm : Form
    {
        public SocketForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EchoHandlerEvent handlerEvent = new EchoHandlerEvent();
            handlerEvent.IsAllowHandleAdd += HandlerEvent_IsAllowHandleAdd;
            handlerEvent.MessageReceived += HandlerEvent_MessageReceived;
            DotNettyServerHelper.Instance.RunNettyServerAsync(60000, handlerEvent);
        }
        private void HandlerEvent_MessageReceived(string arg1, string arg2)
        {
            Console.WriteLine($"{arg2} 接收 : " + arg1.ToString());
            
        }
        private bool HandlerEvent_IsAllowHandleAdd(string arg)
        {
            //[::ffff:10.113.7.210]:60000]
            Console.WriteLine($"{arg} 连接 ");
            return true;
        }

        private void SocketForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DotNettyServerHelper.Instance.StopNettyServerAsync();
        }
    }
}
