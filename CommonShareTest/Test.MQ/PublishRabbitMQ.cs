using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Test.MSMQ
{
    public class PublishRabbitMQ
    {
        String exchangeName = "wytExchange";//交换机名称
        String routeKey = "wytRouteKey";//路由关键值
        String message = "Hello World!";

        private IModel channel = null;
        private IBasicProperties properties = null;
        public void CreatePublish()
        {

            if (channel==null)
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.HostName = "10.113.7.234";
                //factory.Port = 5672;
                //factory.VirtualHost = "/chaint";
                factory.UserName = "chaint";
                factory.Password = "chaint";

                using (IConnection connection = factory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        //声明交换机（名称：log，类型：fanout（扇出）,durable是否持久化）
                        channel.ExchangeDeclare(exchange: exchangeName, type: "direct", durable: true, autoDelete: false, arguments: null);

                        Byte[] body = Encoding.UTF8.GetBytes(message);
                        if (properties == null)
                        {
                            properties = channel.CreateBasicProperties();
                            properties.DeliveryMode = 1;//消息本身也需要被持久化，可以在投递消息前设置AMQP.BasicProperties的属性deliveryMode为2即可
                            properties.Persistent = true;
                        }
                        channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                        //消息推送
                        channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, basicProperties: properties, body: body);

                        Console.WriteLine(" [x] Sent {0}", message);

                    }
                }
            }
            else
            {
                Byte[] body = Encoding.UTF8.GetBytes(message);
                //消息推送
                channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, basicProperties: properties, body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }
        }
    }
}
