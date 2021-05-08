
using MQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoFactory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ms.SubscribeReceivedEventHandler += Ms_SubscribeReceivedEventHandler;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DataUtils.InitialData();
            //ILSUtils.InitialData();
            //IRow taskRow = TaskAccess.Instance.Task_FindTask_TaskCacheByTaskCacheOID(10290);
            //XPODataRowAdapter adapter = new XPODataRowAdapter(taskRow);
            //TaskInfo info = TaskInfo.Parse(adapter);
        }
        SubscribeRabbitMQ ms = new SubscribeRabbitMQ("10.113.7.59", "chaint", "chaint", "wytExchange", "wytRouteKey", "wytQueue",true);
        private void button2_Click(object sender, EventArgs e)
        {
            
            ms.CreateSubscribe();
        }

        private int Ms_SubscribeReceivedEventHandler(byte[] body, bool isRedelivered)
        {
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"{message} 是否是重复发送 : " + isRedelivered);
            return 1;
        }

        PublishRabbitMQ mo = new PublishRabbitMQ("10.113.7.59","chaint","chaint", "wytExchange", "wytRouteKey", "wytQueue",true);

        int num = 0;
        object obj = new object();
        private void button3_Click(object sender, EventArgs e)
        {
            num++;
            List<int> ff = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            //foreach(int item in ff)
            ParallelLoopResult result = Parallel.ForEach(ff, item =>
            {
                //lock (obj)
                {
                    string message = string.Format("{0}--Hello World!", item * num);
                    bool isright = mo.PublishMessage(message,num);
                }
            }
            );
            //string message = string.Format("{0}--Hello World!",num);
            //mo.CreateProducer();
            //bool isring= mo.PublishMessage(message);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ms.CreateSubscribe(true);
        }
    }
}
