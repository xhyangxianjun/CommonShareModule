using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.SignalRs
{
    public class ProxyMethodCallBase<T> where T : Hub
    {
        public IHubConnectionContext<dynamic> Clients { get; set; }
        public IGroupManager Groups { get; set; }
        public ProxyMethodCallBase()
        {
            //通过全局变量GlobalHost的ConnectionManager获取一个hub的引用
            var hub = GlobalHost.ConnectionManager.GetHubContext<T>();
            Clients = hub.Clients;
            Groups = hub.Groups;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="name"></param>
        public void AddMessage(string name)
        {
            Clients.All.AddMessage($"欢迎{name}上线");

        }
    }
}
