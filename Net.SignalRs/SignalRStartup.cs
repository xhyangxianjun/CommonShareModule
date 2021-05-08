using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

//[assembly: OwinStartup(typeof(Net.SignalRs.SignalRStartup))]

namespace Net.SignalRs
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            //两种允许跨域的策略，一种是JSONP模式，另外一种是Cors模式
            //配置
            HubConfiguration configuration = new HubConfiguration
            {
                EnableDetailedErrors = true,
                EnableJavaScriptProxies=false,//是否启用自动生成js代理代码
                //EnableJSONP=false//仅支持Get请求,需要服务器端配合，传输数据大小有限制
            };
            //1、PersistentConnection 方式配置
            //app.MapSignalR<ChatConnection>("/Connections/ChatConnection", configuration);

            //设置可以跨域访问--需引用 Microsoft.Owin.Cors程序集
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //2.hub方式配置，映射到默认的管理,默认指定一个路径  "/signalr"
            app.MapSignalR(configuration);
        }
    }
}
