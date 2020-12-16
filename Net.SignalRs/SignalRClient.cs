using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.SignalRs
{
    public class SignalRClient
    {
        HubConnection Connection { get; set; }
        public IHubProxy hubProxy = null;
        /// <summary>
        /// 初始化服务连接
        /// </summary>
        private void InitHub(string serverUrl)
        {
            string url = @"http://localhost:8082/signalr";
            if (string.IsNullOrEmpty(serverUrl))
                serverUrl = url;
            //创建连接对象，并实现相关事件
            Connection = new HubConnection(serverUrl);

            //实现相关事件
            Connection.Closed += HubConnection_Closed;
            Connection.Received += HubConnection_Received;
            Connection.Reconnected += HubConnection_Succeed;
            Connection.TransportConnectTimeout = new TimeSpan(3000);

            //绑定一个集线器
            //根据hub名创建代理，一些操作由这个代理来做
            hubProxy = Connection.CreateHubProxy("SignalRHub");
            AddProtocal();
        }

        private void HubConnection_Succeed()
        {
            throw new NotImplementedException();
        }

        private void HubConnection_Received(string obj)
        {
            throw new NotImplementedException();
        }

        private void HubConnection_Closed()
        {
            throw new NotImplementedException();
        }

        private async Task StartConnect()
        {
            try
            {
                //开始连接
                await Connection.Start();
      
                HubConnection_Succeed();//处理连接后的初始化

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return;
            }
        }

        /// <summary>
        /// 对各种协议的事件进行处理
        /// 注册收到数据时的方法名与执行的操作，类似于事件
        /// </summary>
        private void AddProtocal()
        {
            //接收实时信息
            //注册收到数据时的方法名与执行的操作，类似于事件
            hubProxy.On<string>("AddMessage", (tt) =>
            {

            });

            //
            hubProxy.On("logined", () =>
                {

                }
            );

            //服务端拒绝的处理
            hubProxy.On("rejected", () =>
                {

                    CloseHub();
                }
            );

            //客户端收到服务关闭消息
            hubProxy.On("SendClose", () =>
            {
                CloseHub();
            });
        }

        public void CloseHub()
        {
            if (Connection != null)
            {
                Connection.Stop();
                Connection.Dispose();
            }
        }
    }
}
