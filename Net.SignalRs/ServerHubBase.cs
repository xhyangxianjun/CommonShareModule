using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.SignalRs
{
    //默认访问的hub名是首字母变小写的名称，如果没有HubName的注解的话，这个hub的名是ServerHubBase
    [HubName("ServerHubBase")]
    public class ServerHubBase:Hub
    {
        public List<string> UserIdList { get; } = new List<string>();

        public Action<string> ClientConnectedEvent;
        /// <summary>
        /// 客户端连接上时的操作
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {            
            UserIdList.Add(Context.ConnectionId);
            ///向客户端写入一些数据
            //string dd=string.Format("客户端连接ID:" + Context.ConnectionId);
            //Clients.Client(Context.ConnectionId).
            ClientConnectedEvent?.Invoke(Context.ConnectionId);
            return base.OnConnected();
        }
        public Action<string> ClientDisConnectedEvent;
        public override Task OnDisconnected(bool stopCalled)
        {
            UserIdList.Remove(Context.ConnectionId);
            ClientDisConnectedEvent?.Invoke(Context.ConnectionId);
            ///向服务端写入一些数据
            //string dd = string.Format("客户端退出ID:" + Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
        public Action<string> ClientReConnectedEvent;
        public override Task OnReconnected()
        {
            if(!UserIdList.Contains(Context.ConnectionId))
                UserIdList.Add(Context.ConnectionId);
            ClientReConnectedEvent?.Invoke(Context.ConnectionId);
            return base.OnReconnected();
        }
          

        //客户端发送数据时调用的方法
        [HubMethodName("Hello")]
        public void Hello(string name)
        {
            //这里有很多方法，就不一一写了
            //SayHello 客户端接收时的方法
            Clients.All.AddMessage($"欢迎{name}上线");
        }
        /// <summary>
        /// 发送错误日志广播到指定客户端
        /// </summary>
        /// <param name="identify">唯一标示</param>
        /// <param name="errMessage">错误信息</param>
        [HubMethodName("ErrSend")]
        public void ErrSend(string identify, string errMessage)
        {
            Clients.Client(identify).AddMessage(errMessage);
        }      

        //有时候需要服务端主动调用方法发送给客户端来做到一些广播之类的，但是hub是不能实例化的，每次访问hub的时候，内部会自动给创建实例
        //创建静态方法啦，不过方法内部不能直接调用Client了，需要获取Context
        public static void Broadcast(string name)
        {
            //通过全局变量GlobalHost的ConnectionManager获取一个hub的引用
            var context = GlobalHost.ConnectionManager.GetHubContext<ServerHubBase>();
            context.Clients.All.AddMessage($"欢迎{name}上线_version1.1");
           
        }
    }
}
