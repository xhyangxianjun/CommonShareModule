using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.MSMQ;

namespace ConsoleSubscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            SubscribeRabbitMQ ms = new SubscribeRabbitMQ();
            ms.CreateSubscribe();
   
            Console.ReadKey();
        }
    }
}
