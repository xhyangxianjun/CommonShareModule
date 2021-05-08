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

namespace MQ.Client
{
    public delegate int BasicReceivedEventHandler(byte[] body,bool isRedelivered);
    public class SubscribeRabbitMQ
    {
        private string _IP = "127.0.0.1";
        private string _UserName = "guest";
        private string _PWD = "guest";
        private string _exchangeName = "wytExchange";//交换机名称
        private string _routeKey = "wytRouteKey";//路由关键值
        private string _queueName = "wytQueue";
        private bool _isNeedPriority = false;
        public SubscribeRabbitMQ(string ip, string usename, string pwd, string exchangeName, string routename, string queueName,bool isNeedPriority=false)
        {
            _IP = ip;
            _UserName = usename;
            _PWD = pwd;
            _exchangeName = exchangeName;
            _routeKey = routename;
            _queueName = queueName;
            _isNeedPriority = isNeedPriority;
        }
        private IModel _channel = null;
        public event BasicReceivedEventHandler SubscribeReceivedEventHandler;
        /// <summary>
        /// 创建消费者
        /// </summary>
        /// <param name="isHandle">ture表示需要每次手动调用才能回去一个消息</param>
        public void CreateSubscribe(bool isHandle=false)
        {
            try
            {
                if (_channel == null || _channel.IsClosed)
                {
                    ConnectionFactory factory = new ConnectionFactory
                    {
                        AutomaticRecoveryEnabled = true,//自动的错误恢复机制
                        DispatchConsumersAsync = false
                    };
                    factory.HostName = _IP;
                    //factory.Port = 5672;
                    //factory.VirtualHost = "/chaint";
                    factory.UserName = _UserName;
                    factory.Password = _PWD;
                    IConnection connection = factory.CreateConnection();
                    //using (IConnection connection = factory.CreateConnection())
                    //{
                    //using (IModel channel = connection.CreateModel())
                    //{
                    _channel = connection.CreateModel();
                    //声明交换机（名称：log，类型：fanout（扇出）,durable是否持久化）
                    _channel.ExchangeDeclare(exchange: _exchangeName, type: "direct", durable: true, autoDelete: false, arguments: null);

                    //声明队列
                    if(_isNeedPriority)
                    {
                        //exclusive：是否排外的，有两个作用，一：当连接关闭时connection.close()该队列是否会自动删除；
                        //二：该队列是否是私有的private，如果不是排外的，可以使用两个消费者都访问同一个队列，没有任何问题，如果是排外的，会对当前队列加锁，
                        //其他通道channel是不能访问的，如果强制访问会报异常：com.rabbitmq.client.ShutdownSignalException: channel error; 
                        _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object>() {
                                        {"x-max-priority",10 } });
                    }
                    else
                        _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    //将队列和交换机绑定
                    _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: _routeKey, arguments: null);
                    //prefetch_count:允许Consumer最多同时处理几个任务;也就是说如果某一个Consumer在收到消息后没有发送ACK确认包，RabbitMQ就会任务Consumer还在处理任务，当有1个消息都没有发送ACK确认包时，RabbitMQ就不会再发送消息给该Consumer。 
                    //当然任务并不会一直卡在这里，当前这1个RabbitMQ任务一直由Consumer在处理。如果Consumer忽然终止，RabbitMQ会重新分发任务。这1条任务被重新分发到了另一个Consumer。
                    //在no_ask = false的情况下生效
                    _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);                   
                    if(isHandle==false)
                    {
                        //定义接收消息的消费者逻辑
                        EventingBasicConsumer SubscribeConsumer = new EventingBasicConsumer(_channel);
                        SubscribeConsumer.Received += (model, ea) =>
                        {
                            BasicDeliverEventArgs deliver = ea;
                            byte[] body = ea.Body.ToArray();
                            if (SubscribeReceivedEventHandler != null)
                            {
                                int result = SubscribeReceivedEventHandler(body, ea.Redelivered);
                                if (result == 1)
                                {
                                    //手动发送ACK应答
                                    (model as EventingBasicConsumer).Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                                }
                                else if (result == 2)
                                {
                                    //重新排队 requeue: true  一次可以拒绝多条
                                    (model as EventingBasicConsumer).Model.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                                    //重新排队 requeue: true  一次拒绝一条
                                    //(model as EventingBasicConsumer).Model.BasicReject(deliveryTag: ea.DeliveryTag,requeue: true);
                                }
                                else
                                {
                                    //丢弃  requeue: false
                                    (model as EventingBasicConsumer).Model.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                                }
                            }
                            else
                            {
                                string message = Encoding.UTF8.GetString(body);
                                //Console.WriteLine(" [x] {0}", message);
                                //Redelivered属性：如果该消息是第一次交付，它将被设置为false.否则为 true
                                Console.WriteLine($"{message} 是否是重复发送 : " + ea.Redelivered);
                                //手动发送ACK应答
                                (model as EventingBasicConsumer).Model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                                //Thread.Sleep(500);
                            }

                        };
                        string CTag = _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: SubscribeConsumer);
                        //channel.BasicRecover(true);//重复发送
                        //channel.BasicCancel(CTag);取消                       

                        //}
                    }
                }
                if (_channel!=null && _channel.IsClosed==false && isHandle)
                {
                    //手动获取一个消息
                    BasicGetResult g = _channel.BasicGet(_queueName, false);
                    if (g == null)
                        return;
                    //_channel.BasicAck(deliveryTag: g.DeliveryTag, multiple: false);
                    byte[] body = g.Body.ToArray();
                    if (SubscribeReceivedEventHandler != null)
                    {
                        int result = SubscribeReceivedEventHandler(body, g.Redelivered);
                        if (result == 1)
                        {
                            //手动发送ACK应答
                            _channel.BasicAck(deliveryTag: g.DeliveryTag, multiple: false);
                        }
                        else if (result == 2)
                        {
                            //重新排队 requeue: true  一次可以拒绝多条
                            _channel.BasicNack(deliveryTag: g.DeliveryTag, multiple: false, requeue: true);

                        }
                        else
                        {
                            //丢弃  requeue: false
                            _channel.BasicNack(deliveryTag: g.DeliveryTag, multiple: false, requeue: false);
                        }
                    }
                    else
                    {
                        string message = Encoding.UTF8.GetString(body);
                        //Console.WriteLine(" [x] {0}", message);
                        //Redelivered属性：如果该消息是第一次交付，它将被设置为false.否则为 true
                        Console.WriteLine($"{message} 是否是重复发送 : " + g.Redelivered);
                        //手动发送ACK应答
                        _channel.BasicAck(deliveryTag: g.DeliveryTag, multiple: false);
                        //Thread.Sleep(500);
                    }
                }
            }
            catch(Exception ex)
            { throw ex; }
        }

        public void CloseSubscribe()
        {
            if (_channel != null)
                _channel.Close();
            _channel = null;
        }
    }
}
