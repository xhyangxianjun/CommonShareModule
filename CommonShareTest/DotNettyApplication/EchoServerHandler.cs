using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyApplication
{
    public class EchoServerHandler : ChannelHandlerAdapter //管道处理基类，较常用
    {
        //  重写基类的方法，当消息到达时触发，这里收到消息后，在控制台输出收到的内容，并原样返回了客户端
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {

            var buffer = message as IByteBuffer;
            if (buffer != null)
            {
                Console.WriteLine("Received from client: " + buffer.ToString(Encoding.UTF8));
            }
            //消息通过context.WriteAsync写回到客户端
            //只是将流缓存到上下文中，并没执行真正的写入操作，通过执行Flush将流数据写入管道，并通过context传回给传来的客户端
            //编码成IByteBuffer,发送至客户端
            string msg = "服务端从客户端接收到内容后返回，我是服务端";
            byte[] messageBytes = Encoding.UTF8.GetBytes(msg);
            IByteBuffer initialMessage = Unpooled.Buffer(messageBytes.Length);
            initialMessage.WriteBytes(messageBytes);
            context.WriteAsync(initialMessage);//写入输出流
        }

        //管道读取完成 输出到客户端，也可以在上面的方法中直接调用WriteAndFlushAsync方法直接输出
        public override void ChannelReadComplete(IChannelHandlerContext context) => context.Flush();

        //捕获 异常，并输出到控制台后断开链接，提示：客户端意外断开链接，也会触发
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            Console.WriteLine("Exception: " + exception);
            context.CloseAsync();
        }
        //客户端连接进来时
        public override void HandlerAdded(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端{context}上线.");
            base.HandlerAdded(context);
        }

        //客户端下线断线时
        public override void HandlerRemoved(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端{context}下线.");
            base.HandlerRemoved(context);
        }

        //服务器监听到客户端活动时
        public override void ChannelActive(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端{context.Channel.RemoteAddress}在线.");
            base.ChannelActive(context);
        }

        //服务器监听到客户端不活动时
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            Console.WriteLine($"客户端{context.Channel.RemoteAddress}离线了.");
            base.ChannelInactive(context);
        }
    }
}
