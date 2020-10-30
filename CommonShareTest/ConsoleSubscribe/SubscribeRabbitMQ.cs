using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Util;
using RabbitMQ.Client.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test.MSMQ
{
    public class SubscribeRabbitMQ
    {
        private readonly TimeSpan _heartbeatTimeout = TimeSpan.FromSeconds(2);
        String exchangeName = "wytExchange";//交换机名称
        String routeKeyName = "wytRouteKey";//路由关键值
        String queueName = "wytQueue";
        public BasicDeliverEventArgs deliver = null;
        public void CreateSubscribe()
        {
            if (SubscribeConsumer != null)
                return;
            ConnectionFactory factory = new ConnectionFactory
            {
                //RequestedHeartbeat = _heartbeatTimeout,//设置=0， 意味着不检测心跳，server端将不会主动断开连接
                AutomaticRecoveryEnabled = true,//自动的错误恢复机制
                DispatchConsumersAsync = false
                
            };
            factory.HostName = "10.113.7.234";
            factory.Port = 5672;
            //factory.VirtualHost = "/chaint";
            factory.UserName = "chaint";
            factory.Password = "chaint";
            IConnection connection = factory.CreateConnection();
            //using (IConnection connection = factory.CreateConnection())
            {
                //using (IModel channel = connection.CreateModel())
                {
                    IModel channel = connection.CreateModel();
                    //声明交换机（名称：log，类型：fanout（扇出）,durable是否持久化）
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable: true, autoDelete: false, arguments: null);

                    //声明队列
                    //channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //将队列和交换机绑定
                    //channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routeKeyName, arguments: null);
                    //prefetch_count:允许Consumer最多同时处理几个任务;也就是说如果某一个Consumer在收到消息后没有发送ACK确认包，RabbitMQ就会任务Consumer还在处理任务，当有1个消息都没有发送ACK确认包时，RabbitMQ就不会再发送消息给该Consumer。 
                    //当然任务并不会一直卡在这里，当前这1个RabbitMQ任务一直由Consumer在处理。如果Consumer忽然终止，RabbitMQ会重新分发任务。这1条任务被重新分发到了另一个Consumer。
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    //定义接收消息的消费者逻辑
                    SubscribeConsumer = new EventingBasicConsumer(channel);

                    SubscribeConsumer.Received += (model, ea) =>
                    {
                        deliver = ea;
                        //Thread.Sleep(1000);
                        //Task.Factory.StartNew(t==>{
                        Byte[] body = ea.Body.ToArray();
                        String message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] {0}", message);
                        //Redelivered属性：如果该消息是第一次交付，它将被设置为false.否则为 true
                        Console.WriteLine($"{message} 是否是重复发送 : " +ea.Redelivered);
                        //手动发送ACK应答
                        (model as EventingBasicConsumer).Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        //丢弃  requeue: false 重新排队 requeue: true
                        //channel.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                    };
                    string CTag = channel.BasicConsume(queue: queueName, autoAck: false, consumer: SubscribeConsumer);
                    //channel.BasicRecover(false);
                    //channel.BasicCancel(CTag);取消
                    //手动获取一个消息
                    //BasicGetResult g = channel.BasicGet(queueName, false);
                    //channel.BasicAck(deliveryTag: g.DeliveryTag, multiple: false);
                    //Byte[] body = g.Body.ToArray();
                    //String message = Encoding.UTF8.GetString(body);
                    //Console.WriteLine(" [x] {0}", message);

                }
            }
        }

        public EventingBasicConsumer SubscribeConsumer { get; set; }
    }
}
