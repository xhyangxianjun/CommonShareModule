using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSubscribe
{
    public class RabbitMqMessageConsumer : EventingBasicConsumer
    {
        public RabbitMqMessageConsumer(IModel channel):base(channel)
        {
            
        }
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {

        }
    }
}
