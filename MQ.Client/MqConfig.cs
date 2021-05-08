

namespace MQ.Client
{
    public class MqConfig
    {
        public string Host
        {
            get;
            set;
        } = "localhost";


        public string VirtualHost
        {
            get;
            set;
        } = "/";


        public int Port
        {
            get;
            set;
        } = 5672;


        public string UserName
        {
            get;
            set;
        } = "guest";


        public string Password
        {
            get;
            set;
        } = "guest";


        public string ExchangeName
        {
            get;
            set;
        } = "Topic";


        public string ExchangeTypeName
        {
            get;
            set;
        } = "topic";
        /// <summary>
        /// 消息有效时间 秒
        /// </summary>
        public int MessageEffectiveTime
        {
            get;
            set;
        }
        /// <summary>
        /// 队列存在时间(当前无消息或无消费者的情况下清除队列) 分钟
        /// </summary>
        public string QueueEffectiveTime
        {
            get;
            set;
        }
    }
}
