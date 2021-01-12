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
        private HubConnection _Connection { get; set; }
        private IHubProxy hubProxy = null;
        public Action HubConnectionReConnect;
        public Action<string> HubConnectionReceived;
        public Action HubConnectionReClosed;
        /// <summary>
        /// 初始化服务连接
        /// </summary>
        public void InitHub(string serverUrl,string hubName)
        {
            string url = @"http://localhost:12345";
            if (string.IsNullOrEmpty(serverUrl))
                serverUrl = url;
            //创建连接对象，并实现相关事件
            _Connection = new HubConnection(serverUrl);

            //实现相关事件
            _Connection.Closed += HubConnection_Closed;
            _Connection.Received += HubConnection_Received;
            _Connection.Reconnected += HubConnection_Reconnected;
            _Connection.TransportConnectTimeout = new TimeSpan(3000);
           
            //绑定一个集线器
            //根据hub名创建代理，一些操作由这个代理来做
            hubProxy = _Connection.CreateHubProxy(hubName);
           
            AddProtocal();
        }

        public void InvokeMethod(string methodName,string dd)
        {
            hubProxy.Invoke(methodName, dd);
        }

        private void HubConnection_Reconnected()
        {
            HubConnectionReConnect?.Invoke();
        }
        //在关闭连接后尝试重新连接时触发
        private void HubConnection_Received(string obj)
        {
            HubConnectionReceived?.Invoke(obj);
        }
        //重新连接失败时(服务停机时间长于重新连接超时所接受的时间),将调用Closed
        private void HubConnection_Closed()
        {
            if(_Connection.State==ConnectionState.Connected)
            {

            }
            HubConnectionReClosed?.Invoke();

        }

        private async Task StartConnect()
        {
            try
            {
                //开始连接
                await _Connection.Start();    

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddProxyMethod(string methodName, Action ac)
        {
            hubProxy.On(methodName, ac);
        }

        public void AddProxyMethod<T>(string methodName,Action<T> ac)
        {
            hubProxy.On<T>(methodName, ac);
        }

        public void AddProxyMethod<T,T2>(string methodName, Action<T, T2> ac)
        {
            hubProxy.On<T, T2>(methodName, ac);
        }

        public void AddProxyMethod<T, T2,T3>(string methodName, Action<T, T2, T3> ac)
        {
            hubProxy.On<T, T2, T3>(methodName, ac);
        }
        /// <summary>
        /// 对指定协议的事件进行处理
        /// </summary>
        private void AddProtocal()
        {
            //注册收到数据时的方法名与执行的操作，类似于事件
            hubProxy.On<string>("AddMessage", (tt) =>
            {
                Console.WriteLine(tt);
            });

            //服务端拒绝的处理
            hubProxy.On("Rejected", () =>
                {

                    CloseHub();
                }
            );

            //客户端收到服务关闭消息
            hubProxy.On("CloseHub", () =>
            {
                CloseHub();
            });
        }

        public void CloseHub()
        {
            if (_Connection != null)
            {
                _Connection.Stop();
                _Connection.Dispose();
            }
        }
    }
}
