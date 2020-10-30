using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            SubscribeRabbitMQ ms = new SubscribeRabbitMQ("10.113.7.234", "chaint", "chaint", "wytExchange", "wytRouteKey", "wytQueue");
            ms.CreateSubscribe();
   
            Console.ReadKey();
        }
    }
}
