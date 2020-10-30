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
    public class MyHub:Hub
    {


        public List<string> UserIdList1 { get; } = new List<string>();
        /// <summary>
        /// 在连接上时
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            UserIdList1.Add(Context.ConnectionId);
            ///向服务端写入一些数据
            string dd=string.Format("客户端连接ID:" + Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserIdList1.Remove(Context.ConnectionId);
            ///向服务端写入一些数据
            string dd = string.Format("客户端退出ID:" + Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            UserIdList1.Add(Context.ConnectionId);
            return base.OnReconnected();
        }
    
        /// <summary>
        /// 信息广播
        /// 客户端发送数据时调用的方法
        /// </summary>
        /// <param name="identify">iot唯一标识</param>
        /// <param name="model">数据模型</param>
        [HubMethodName("Send")]
        public void Send(string identify, string model)
        {
            Clients.All.addMessage(identify, model);
        }
        /// <summary>
        /// 错误日志广播
        /// </summary>
        /// <param name="identify">唯一标示</param>
        /// <param name="errMessage">错误信息</param>
        [HubMethodName("ErrSend")]
        public void ErrSend(string identify, string errMessage)
        {
            Clients.All.addMessage(identify, errMessage);
        }
        /// <summary>
        /// 向首次连接的客户端返回实时数据，用于展示
        /// </summary>
        /// <param name="identify"></param>
        /// <param name="list"></param>
        [HubMethodName("initSend")]
        public void Send(string identify, List<string> list)
        {
            Clients.All.initData(identify, list);
        }

        //有时候需要服务端主动调用方法来做到一些广播之类的，但是hub是不能实例化的，每次访问hub的时候，内部会自动给创建实例
        //创建静态方法啦，不过方法内部不能直接调用Client了，需要获取Context
        public static void Broadcast(string name)
        {
            //通过全局变量GlobalHost的ConnectionManager获取一个hub的引用
            var context = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            context.Clients.All.SayHello($"欢迎{name}上线_version1.1");
        }
    }
}
