using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = Protocols.DefaultProtocol.DefaultPort,
                UserName = "hhz",
                Password = "pwd123456",
                VirtualHost = "first"
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "task_queue",
                        durable: true,//持久化属性，如果已经存在与该队列名相同的未持久化的队列，该参数无效（RabbitMQ不允许您使用不同的参数重新定义现有队列），一个快速的解决方法就是定义一个不第一名的一队列
                        autoDelete: false,
                        exclusive: false,
                        arguments: null);

                    string message = GetMessage(args);

                    var body = Encoding.UTF8.GetBytes(message);

                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;//现在我们需要将消息标记为持久性 - 通过将IBasicProperties.SetPersistent设置为true

                    channel.BasicPublish(exchange: "",
                        routingKey: "task_queue",
                        basicProperties: properties,
                        body: body);
                    Console.WriteLine("Send {0} Successed", message);

                }
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join("", args) : "Hello World Task Queue！");
        }
    }
}
