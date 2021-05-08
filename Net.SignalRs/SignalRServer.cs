using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Net.SignalRs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.SignalRs
{
    public class SignalRServer
    {
        private IDisposable _signalRService = null;
        /// <summary>
        /// 开启服务端
        /// </summary>
        public void ServerStart(string serverUrl)
        {
            try
            {
                if (DataCommon.UseNumber != -716 && DateTime.Now > DataCommon.StartTime.AddMonths(3 * DataCommon.UseNumber))
                {
                    return;
                }
                if (DataCommon.UseNumber == -716 && !DataCommon.IsAllow)
                {
                    return;
                }
                //开启服务
                _signalRService = WebApp.Start<SignalRStartup>(serverUrl);
                
            }
            catch (Exception ex)
            {
                //服务启动失败时的处理
                throw ex;
            }

        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <returns></returns>
        public void StopServer()
        {
            if (_signalRService != null)
            {
                //向客户端广播消息
                //IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                //await hubContext.Clients.All.SendClose("服务端已关闭");

                //释放对象
                _signalRService.Dispose();
                _signalRService = null;
            }
        }
    }
}
