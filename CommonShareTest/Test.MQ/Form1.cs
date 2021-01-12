using MQ.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test.MSMQ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MSMQ服务器在本地
            //MessageQueue Mq = new MessageQueue(@".//private$//jiang");
            //远程访问专用队列
            //直接使用多元素格式名方式，利用IP地址直接对单个或多个目标发送消息
            //MessageQueue rmQMore = new MessageQueue
            //  ("FormatName:Direct=TCP:121.0.0.1\\private$\\queue,Direct=TCP:192.168.1.2\\private$\\queue");
            //rmQMore.Send("sent to regular queue - Atul");
            Createqueue("FormatName:Direct=TCP:10.113.7.234\\private$\\queue");
            SendMessage();
            //MessageQueue rmQ = new MessageQueue("FormatName:Direct=TCP:10.113.7.234\\private$\\queue");
            //rmQ.Send("ceshi,duilie");

        }

        private string Path;

        /// <summary>

        /// 1.通过Create方法创建使用指定路径的新消息队列

        /// </summary>

        /// <param name="queuePath"></param>

        public void Createqueue(string queuePath)

        {

            try

            {
                MessageQueue.Delete(queuePath);
                if (!MessageQueue.Exists(queuePath))

                {

                    MessageQueue.Create(queuePath);

                }

                else

                {

                    Console.WriteLine(queuePath + "已经存在！");

                    //MessageQueue.Delete(queuePath);

                    //MessageQueue.Create(queuePath);

                    //Console.WriteLine(queuePath + "删除重建");

                }

                Path = queuePath;

            }

            catch (MessageQueueException e)

            {

                Console.WriteLine(e.Message);

            }

        }

        /// <summary>

        ///  2.连接消息队列并发送消息到队列

        /// 远程模式：MessageQueue rmQ = new MessageQueue("FormatName:Direct=OS:machinename//private$//queue");

        ///     rmQ.Send("sent to regular queue - Atul");对于外网的MSMQ只能发不能收

        /// </summary>

        public void SendMessage()

        {

            try

            {

                //连接到本地队列

                MessageQueue myQueue = new MessageQueue(Path);

                //MessageQueue myQueue = new MessageQueue("FormatName:Direct=TCP:192.168.12.79//Private$//myQueue1");

                //MessageQueue rmQ = new MessageQueue("FormatName:Direct=TCP:121.0.0.1//private$//queue");--远程格式

                System.Messaging.Message myMessage = new System.Messaging.Message();

                myMessage.Body = "消息内容34kuangbo去死";

                myMessage.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

                //发生消息到队列中

                myQueue.Send(myMessage);

                Console.WriteLine("消息发送成功！");

                Console.ReadLine();

            }

            catch (ArgumentException e)

            {

                Console.WriteLine(e.Message);

            }

        }

        /// <summary>

        /// 3.连接消息队列并从队列中接收消息

        /// </summary>

        public void ReceiveMessage()

        {

            MessageQueue myQueue = new MessageQueue(Path);

            myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            try

            {

                //从队列中接收消息

                System.Messaging.Message myMessage = myQueue.Receive();// myQueue.Peek();--接收后不消息从队列中移除

                string context = myMessage.Body.ToString();

                Console.WriteLine("消息内容：" + context);

                Console.ReadLine();

            }

            catch (MessageQueueException e)

            {

                Console.WriteLine(e.Message);

            }

            catch (InvalidCastException e)

            {

                Console.WriteLine(e.Message);

            }

        }



        /// <summary>

        /// 4.清空指定队列的消息

        /// </summary>

        public void ClealMessage()

        {

            MessageQueue myQueue = new MessageQueue(Path);

            myQueue.Purge();

            Console.WriteLine("已清空对了{0}上的所有消息", Path);

        }



        /// <summary>

        /// 5.连接队列并获取队列的全部消息

        /// </summary>

        public void GetAllMessage()

        {

            MessageQueue myQueue = new MessageQueue(Path);

            System.Messaging.Message[] allMessage = myQueue.GetAllMessages();

            XmlMessageFormatter formatter = new XmlMessageFormatter(new Type[] { typeof(string) });

            for (int i = 0; i < allMessage.Length; i++)

            {

                allMessage[i].Formatter = formatter;

                Console.WriteLine("第{0}机密消息为:{1}", i + 1, allMessage[i].Body.ToString());

            }

            Console.ReadLine();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PublishRabbitMQ mo = new PublishRabbitMQ("10.113.7.59", "chaint", "chaint", "wytExchange", "wytRouteKey", "wytQueue", true);
            string message = string.Format("{0}--Hello World!", 1);
            bool isright = mo.PublishMessage(message, 1);
            return;

            var factory = new ConnectionFactory();
            factory.HostName = "10.113.7.234";//主机名，Rabbit会拿这个IP生成一个endpoint，这个很熟悉吧，就是socket绑定的那个终结点。
            factory.UserName = "chaint";//默认用户名,用户可以在服务端自定义创建，有相关命令行
            factory.Password = "chaint";//默认密码

            using (var connection = factory.CreateConnection())//连接服务器，即正在创建终结点。
            {
                //创建一个通道，这个就是Rabbit自己定义的规则了，如果自己写消息队列，这个就可以开脑洞设计了
                //这里Rabbit的玩法就是一个通道channel下包含多个队列Queue
                using (var channel = connection.CreateModel())
                {
                    //rabbitmq持久化分为三个部分: 交换器的持久化、队列的持久化和消息的持久化
                    //将durable参数设置为true实现交换器的持久化和队列的持久化
                    channel.QueueDeclare("kibaQueue", true, false, false, null);//创建一个名称为kibaqueue的消息队列
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 1;//消息本身也需要被持久化，可以在投递消息前设置AMQP.BasicProperties的属性deliveryMode为2即可
                    string message1 = "I am Kiba518"; //传递的消息内容
                    channel.BasicPublish("", "kibaQueue", properties, Encoding.UTF8.GetBytes(message1)); //生产消息
                    Console.WriteLine($"Send:{message1}");
                }
            }
        }
        private int Ms_SubscribeReceivedEventHandler(byte[] body, bool isRedelivered)
        {
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"{message} 是否是重复发送 : " + isRedelivered);
            return 1;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SubscribeRabbitMQ ms = new SubscribeRabbitMQ("10.113.7.59", "chaint", "chaint", "wytExchange", "wytRouteKey", "wytQueue", true);
            ms.SubscribeReceivedEventHandler += Ms_SubscribeReceivedEventHandler;
            ms.CreateSubscribe();
            return;
            var factory = new ConnectionFactory();
            factory.HostName = "10.113.7.234";
            factory.UserName = "chaint";
            factory.Password = "chaint";

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("kibaQueue", false, false, false, null);

                    /* 这里定义了一个消费者，用于消费服务器接受的消息
                     * C#开发需要注意下这里，在一些非面向对象和面向对象比较差的语言中，是非常重视这种设计模式的。
                     * 比如RabbitMQ使用了生产者与消费者模式，然后很多相关的使用文章都在拿这个生产者和消费者来表述。
                     * 但是，在C#里，生产者与消费者对我们而言，根本算不上一种设计模式，他就是一种最基础的代码编写规则。
                     * 所以，大家不要复杂的名词吓到，其实，并没那么复杂。
                     * 这里，其实就是定义一个EventingBasicConsumer类型的对象，然后该对象有个Received事件，
                     * 该事件会在服务接收到数据时触发。
                     */
                    var consumer = new EventingBasicConsumer(channel);//消费者
                    //消费者订阅队列将autoAck设置为true,自动确认收到消息，并从队列删除消息
                    channel.BasicConsume("kibaQueue", true, consumer);//消费消息
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                    };
                }
            }
        }
       
        private void button4_Click(object sender, EventArgs e)
        {
            
           
        }
      
        private void button5_Click(object sender, EventArgs e)
        {

          
        }
    }
}
