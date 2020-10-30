using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Net.DotNettySockets
{
    public class EchoServerHandler : ChannelHandlerAdapter
    {
        private EchoHandlerEvent _EchoEvent = null;
        public EchoServerHandler(EchoHandlerEvent echoEvent) :base()
        {
            _EchoEvent = echoEvent;
        }
        /// Read0是DotNetty特有的对于Read方法的封装
        /// 封装实现了：
        /// 1. 返回的message的泛型实现
        /// 2. 丢弃非该指定泛型的信息
        //protected override async void ChannelRead0(IChannelHandlerContext context, IByteBuffer buffer)
        //{
        //    await Task.Run(() =>
        //    {
        //        if (buffer != null)
        //        {
        //            string msg = buffer.ToString(Encoding.UTF8);
        //            if (string.IsNullOrEmpty(msg))
        //                return;
        //            string barCode = msg.TrimEnd(new char[] { (char)3, (char)2 });
        //            if (string.IsNullOrEmpty(barCode) || barCode == "NoRead")
        //                return;
        //            _EchoEvent.OnMessageReceive((context.Channel.RemoteAddress as IPEndPoint).Address, barCode);
        //        }
        //    });
        //}
        public override async void ChannelRead(IChannelHandlerContext context, object message)
        {
            await Task.Run(() =>
            {
                if (message is IByteBuffer)
                {
                    var buffer = message as IByteBuffer;
                    if (buffer != null)
                    {
                        string msg = buffer.ToString(Encoding.UTF8);
                        if (string.IsNullOrEmpty(msg))
                            return;
                        string barCode = msg.TrimEnd(new char[] { (char)3, (char)2 });
                        if (string.IsNullOrEmpty(barCode) || barCode == "NoRead")
                            return;
                        _EchoEvent.OnMessageReceive((context.Channel.RemoteAddress as IPEndPoint).ToString(), barCode);
                    }
                }
            });
            ReferenceCountUtil.Release(message);
        }
        //管道读取完成 输出到客户端，也可以在上面的方法中直接调用WriteAndFlushAsync方法直接输出
        public override void ChannelReadComplete(IChannelHandlerContext context) 
        { context.Flush(); }

        //捕获 异常，并输出到控制台后断开链接，提示：客户端意外断开链接，也会触发
        public override async void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            await Task.Run(() =>
            {
                //Console.WriteLine("Exception: " + exception);
                IChannel temp;
                if (DotNettyServerHelper.AllClients.TryRemove((context.Channel.RemoteAddress as IPEndPoint).ToString(), out temp))
                {
                    _EchoEvent.OnEchoExceptionCaught((context.Channel.RemoteAddress as IPEndPoint).ToString(), exception);
                }
                context.CloseAsync();
            });
        }
        
        //客户端连接进来时
        public override async void HandlerAdded(IChannelHandlerContext context)
        {
            await Task.Run(() =>
            {
                if (_EchoEvent.OnAllowHandleAddClient((context.Channel.RemoteAddress as IPEndPoint).ToString()) == false)
                    return;
                //Console.WriteLine($"客户端{context}上线.");
                DotNettyServerHelper.AllClients.AddOrUpdate((context.Channel.RemoteAddress as IPEndPoint).ToString(), context.Channel, (k, v) => v);
                base.HandlerAdded(context);

                //char str = (char)4;
                //byte[] dd = new byte[1] { Convert.ToByte(str) };
                //context.WriteAsync(str);
            });
            
        }

        //客户端下线断线时
        public override async void HandlerRemoved(IChannelHandlerContext context)
        {
            await Task.Run(() =>
            {
                //Console.WriteLine($"客户端{context}下线.");
                IChannel temp;
                DotNettyServerHelper.AllClients.TryRemove((context.Channel.RemoteAddress as IPEndPoint).ToString(), out temp);
                base.HandlerRemoved(context);
                _EchoEvent.OnEchoClientRemoved((context.Channel.RemoteAddress as IPEndPoint).ToString());
            });
        }

        ////服务器监听到客户端活动时
        //public override void ChannelActive(IChannelHandlerContext context)
        //{
        //    Console.WriteLine($"客户端{context.Channel.RemoteAddress}在线.");
        //    base.ChannelActive(context);
        //}

        ////服务器监听到客户端不活动时
        //public override void ChannelInactive(IChannelHandlerContext context)
        //{
        //    Console.WriteLine($"客户端{context.Channel.RemoteAddress}离线了.");
        //    base.ChannelInactive(context);
        //}
    }
}
