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
            //可以在这里做一些配置，向跨域什么的  app.usecors 或者 configuration中配置usejsonp
            HubConfiguration configuration = new HubConfiguration
            {
                EnableDetailedErrors = true
            };
            //设置可以跨域访问--需引用 Microsoft.Owin.Cors程序集
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //映射到默认的管理
            app.MapSignalR();
        }
    }
}
