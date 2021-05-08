using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQ.Client
{
    public delegate void BasicEventHandler(long tag, bool isMultiple);
    public class PublishRabbitMQ
    {
        private string _IP = "127.0.0.1";
        private string _UserName = "guest";
        private string _PWD = "guest";
        private string _exchangeName = "wytExchange";//交换机名称
        private string _routeKey = "wytRouteKey";//路由关键值
        private string _queueName = "wytQueue";
        private bool _isNeedPriority = false;
        public event BasicEventHandler BasicNacksEventHandler;
        public event BasicEventHandler BasicAcksEventHandler;
        public PublishRabbitMQ(string ip, string usename, string pwd, string exchangeName, string routename,string queueName,bool isNeedPriority=false)
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
        private IBasicProperties _properties = null;
        public void CreateProducer()
        {
            if (_channel == null || _channel.IsClosed)
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.HostName = _IP;
                //factory.Port = 5672;
                //factory.VirtualHost = "/chaint";
                factory.UserName = _UserName;
                factory.Password = _PWD;
                factory.AutomaticRecoveryEnabled = true;
                IConnection connection = factory.CreateConnection();
                {
                    _channel = connection.CreateModel();
                    {
                        _channel.ConfirmSelect();//设为消息确认通道
                        _channel.BasicAcks += (s, e) =>
                        {
                            if (BasicAcksEventHandler != null)
                                BasicAcksEventHandler((long)e.DeliveryTag, e.Multiple);
                        };
                        //否定确认
                        _channel.BasicNacks += (s, e) =>
                        {
                            if (BasicNacksEventHandler != null)
                                BasicNacksEventHandler((long)e.DeliveryTag, e.Multiple);
                        };
                        //设置死信交换,Topic类型，持久化
                        //_channel.ExchangeDeclare("dlx", "topic", true, false, null);

                        //声明交换机（名称：log，类型：fanout（扇出）,durable是否持久化）
                        _channel.ExchangeDeclare(exchange: _exchangeName, type: "direct", durable: true, autoDelete: false, arguments: null);

                        //声明队列
                        if (_isNeedPriority)
                            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: new Dictionary<string, object>() {
                                           //队列优先级最高为10，不加x-max-priority的话，计算发布时设置了消息的优先级也不会生效
                                             {"x-max-priority",10 } });
                        else
                            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                        //将队列和交换机绑定
                        _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: _routeKey, arguments: null);
                        if (_properties == null)
                        {
                            _properties = _channel.CreateBasicProperties();
                            _properties.DeliveryMode = 2;//消息本身也需要被持久化，可以在投递消息前设置AMQP.BasicProperties的属性deliveryMode为2即可
                            //_properties.Persistent = true;
                        }
                        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
                        //byte[] body = Encoding.UTF8.GetBytes("ceshifasong");
                        //_channel.BasicPublish(exchange: _exchangeName, routingKey: _routeKey, basicProperties: _properties, body: body);
                    }
                }
            }
           
        }

        object obj = new object();
        public bool PublishMessage(string msg,int priority=1)
        {
            try
            {
                lock (obj)
                {
                    CreateProducer();
                    if (_channel == null)
                        return false;
                    else
                    {
                        byte[] body = Encoding.UTF8.GetBytes(msg);
                        //消息推送
                        if (_isNeedPriority)
                            _properties.Priority = Convert.ToByte(priority);
                        _channel.BasicPublish(exchange: _exchangeName, routingKey: _routeKey, basicProperties: _properties, body: body);
                        //可以发送一批消息后,调用该方法;也可以每发一条调用一次.　　　　　　 
                        //return true;
                        return _channel.WaitForConfirms();
                    }
                }
            }
            catch(Exception ex)
            {
                if (_channel != null)
                    _channel.Close();
                _channel = null;
                _properties = null;
                throw ex;
            }
        }
        /// <summary>
        /// 返回发送成功或失败
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool PublishBody(byte[] body, int priority = 1)
        {
            try
            {
                lock (obj)
                {
                    CreateProducer();
                    if (_channel == null)
                        return false;
                    else
                    {
                        if (_isNeedPriority)
                            _properties.Priority = Convert.ToByte(priority);
                        //消息推送
                        _channel.BasicPublish(exchange: _exchangeName, routingKey: _routeKey, basicProperties: _properties, body: body);
                        //可以发送一批消息后,调用该方法;也可以每发一条调用一次.　　　　　　 
                        return _channel.WaitForConfirms();
                    }
                }
            }
            catch (Exception ex)
            {
                if (_channel != null)
                    _channel.Close();
                _channel = null;
                _properties = null;
                throw ex;
            }
        }

        public void CloseProducer()
        {
            if (_channel != null)
                _channel.Close();
            _channel = null;
            _properties = null;
        }
    }
}
