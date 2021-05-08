using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.SignalRs
{
    public class SignalRHelper
    {
        public static void SetHubConfiguration()
        {
            //表示客户端在转而使用其他传输或连接失败之前应允许连接的时间。默认值为 5 秒。(传输超时时间)
            GlobalHost.Configuration.TransportConnectTimeout = TimeSpan.FromSeconds(5);
            //表示连接在超时之前保持打开状态的时间
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(5);
            //用于表示在连接停止之后引发断开连接事件之前要等待的时间。 （强制关闭时间）
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(5);
            //表示两次发送保持活动消息之间的时间长度。如果启用，此值必须至少为两秒。设置为 null 可禁用。
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(2);
            //Websocket模式下允许传输数据的最大值，默认为64kb
            GlobalHost.Configuration.MaxIncomingWebSocketMessageSize = 64;
            //设置消息缓冲区大小,默认情况下，SignalR 将保留在内存中的每个中心的每个连接的 1000 条消息。
            //如果使用大型消息时，这可能会造成内存问题，这可以通过减小此值来缓解压力
            GlobalHost.Configuration.DefaultMessageBufferSize = 500;
        }
    }
}
