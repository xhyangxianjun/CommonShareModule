using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.MSMQ
{
    public class SubscribeRabbitMQ
    {
        String exchangeName = "wytExchange";//交换机名称
        String routeKeyName = "wytRouteKey";//路由关键值
        String queueName = "wytQueue";
        public void CreateSubscribe()
        {
            if (SubscribeConsumer != null)
                return;
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "10.113.7.234";
            factory.Port = 5672;
            //factory.VirtualHost = "/chaint";
            factory.UserName = "chaint";
            factory.Password = "chaint";

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    //声明交换机（名称：log，类型：fanout（扇出）,durable是否持久化）
                    channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable: true, autoDelete: false, arguments: null);

                    //声明队列
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //将队列和交换机绑定
                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routeKeyName, arguments: null);
                    //prefetch_count:允许Consumer最多同时处理几个任务;也就是说如果某一个Consumer在收到消息后没有发送ACK确认包，RabbitMQ就会任务Consumer还在处理任务，当有1个消息都没有发送ACK确认包时，RabbitMQ就不会再发送消息给该Consumer。 
                    //当然任务并不会一直卡在这里，当前这1个RabbitMQ任务一直由Consumer在处理。如果Consumer忽然终止，RabbitMQ会重新分发任务。这1条任务被重新分发到了另一个Consumer。
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    //定义接收消息的消费者逻辑
                    SubscribeConsumer = new EventingBasicConsumer(channel);
                    SubscribeConsumer.Received += (model, ea) =>
                    {
                        try
                        {
                            Byte[] body = ea.Body.ToArray();
                            String message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(" [x] {0}", message);

                            //手动发送ACK应答
                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(" [x] {0}", ex.Message);
                        }
                    };
                    //将消费者和队列绑定 autoAck: true 自动应答  false 需手动应答
                    channel.BasicConsume(queue: queueName, autoAck: false, consumer: SubscribeConsumer);
                }
            }
        }

        public EventingBasicConsumer SubscribeConsumer { get; set; }
    }
}
