using Microsoft.AspNet.SignalR.Client;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Console;

namespace Demo.SignalR
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //自定义持久类调用方式
            //var connection = new Connection("http://localhost:65309/Connections/ChatConnection");
            //connection.Received += WriteLine;
            //connection.Start().Wait();
            //string line;
            //while ((line = ReadLine()) != null)
            //{
            //    connection.Send(line).Wait();
            //}
        }
    }
}
