using DotNetty.Buffers;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.DotNettySockets
{
    public class DotNettyServerHelper
    {
        private DotNettyServerHelper()
        { }
        private static DotNettyServerHelper m_Instance = null;

        public static DotNettyServerHelper Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new DotNettyServerHelper();
                return m_Instance;
            }
        }
        IChannel boundChannel;
        MultithreadEventLoopGroup bossGroup = null;
        MultithreadEventLoopGroup workerGroup =null;

        ServerBootstrap bootstrap;
        public async Task RunNettyServerAsync(int port, EchoHandlerEvent handlerEvent)
        {
            AllClients.Clear();
            // 主工作线程组，设置为1个线程
            bossGroup = new MultithreadEventLoopGroup(1);
            // 工作线程组，默认为内核数*2的线程数
            workerGroup = new MultithreadEventLoopGroup();
            //X509Certificate2 tlsCertificate = null;
            //if (IsSsl) //如果使用加密通道
            //{
            //    tlsCertificate = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dotnetty.com.pfx"), "password");
            //}
            try
            {
                //声明一个服务端Bootstrap，每个Netty服务端程序，都由ServerBootstrap控制
                bootstrap = new ServerBootstrap();
                bootstrap
                    .Group(bossGroup, workerGroup) // 设置主和工作线程组
                    .Channel<TcpServerSocketChannel>() // 设置通道模式为TcpSocket
                    .Option(ChannelOption.SoBacklog, 1024) // 设置网络IO参数等，这里可以设置很多参数，当然你对网络调优和参数设置非常了解的话，你可以设置，或者就用默认参数吧      
                    .ChildHandler//设置工作线程参数
                    (new ActionChannelInitializer<ISocketChannel>(//ChannelInitializer 是一个特殊的处理类，他的目的是帮助使用者配置一个新的 Channel
                        channel =>
                        { //工作线程连接器 是设置了一个管道，服务端主线程所有接收到的信息都会通过这个管道一层层往下传输
                          //同时所有出栈的消息 也要这个管道的所有处理器进行一步步处理
                            IChannelPipeline pipeline = channel.Pipeline;
                            //if (tlsCertificate != null) //Tls的加解密
                            //{
                            //    pipeline.AddLast("tls", TlsHandler.Server(tlsCertificate));
                            //}
                            ////日志拦截器
                            //pipeline.AddLast(new LoggingHandler("SRV-CONN"));
                            //出栈消息，通过这个handler 在消息顶部加上消息的长度
                            //pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                            //入栈消息通过该Handler,解析消息的包长信息，并将正确的消息体发送给下一个处理Handler
                            //pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                            //业务handler ，这里是实际处理Echo业务的Handler
                            EchoServerHandler handle = new EchoServerHandler(handlerEvent);
                            pipeline.AddLast("echo", handle);
                        }))
                    .ChildOption(ChannelOption.SoKeepalive, true);//是否启用心跳保活机制

                // bootstrap绑定到指定端口的行为 就是服务端启动服务，同样的Serverbootstrap可以bind到多个端口
                boundChannel = await bootstrap.BindAsync(port);
              
            }
            catch(Exception ex)
            {
                 throw ex;
            }
           
        }
        public static volatile ConcurrentDictionary<string, IChannel> AllClients = new ConcurrentDictionary<string, IChannel>();
        public async Task StopNettyServerAsync()
        {
            //关闭服务
            if (boundChannel != null)
            {
                await boundChannel.CloseAsync();
                await Task.WhenAll(
                       bossGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)),
                       workerGroup.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1)));
                AllClients.Clear();
                boundChannel = null;
                bootstrap = null;
            }
        }

        public async void SendServerData<T>(T obj, string adressPort)
        {
            try
            {
                IByteBuffer initialMessage = null;
                if (obj is IByteBuffer)
                    initialMessage = obj as IByteBuffer;
                else
                {
                    byte[] messageBytes = null;
                    if (obj is byte[])
                        messageBytes = obj as byte[];
                    //else if(obj is char)
                    //{
                    //    initialMessage = Unpooled.Buffer(1);
                    //    initialMessage.WriteChar((char)obj);
                    //}
                    else
                        messageBytes = System.Text.Encoding.UTF8.GetBytes(obj.ToString());
                    initialMessage = Unpooled.Buffer(messageBytes.Length);
                    initialMessage.WriteBytes(messageBytes);
                }
                IChannel curChannel = null;
                if (AllClients.TryGetValue(adressPort, out curChannel))
                {
                    await curChannel.WriteAndFlushAsync(initialMessage);
                }
            }
            catch (Exception ex) 
            { 
                throw ex; 
            }
        }
        public void GroupSendServerData<T>(T obj,List<string> adressPortList=null)
        {
            try
            {
                byte[] messageBytes = null;
                if (obj is byte[])
                    messageBytes =obj as byte[];
                else
                    messageBytes= System.Text.Encoding.UTF8.GetBytes(obj.ToString());
                IByteBuffer initialMessage = Unpooled.Buffer(messageBytes.Length);
                initialMessage.WriteBytes(messageBytes);
                if (adressPortList==null)
                    AllClients.Values.ToList().ForEach(async s => await s.WriteAndFlushAsync(initialMessage));
                else
                {
                    foreach (string adressPort in adressPortList)
                        SendServerData(initialMessage, adressPort);
                }
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
