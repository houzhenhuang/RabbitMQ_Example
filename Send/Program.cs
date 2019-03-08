using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Send
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
                    //声明一个队列，只有在队列不存在时才创建队列
                    //声明队列时是幂等(无论声明多少次同名的队列，都只有一个)的
                    channel.QueueDeclare(queue: "helloworld",
                        durable: false,
                        autoDelete:false,
                        exclusive: false,
                        arguments: null);

                    string message = "Hello World Rabbitmq!";

                    //消息内容必须是一个字节数组
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                        routingKey: "helloworld",
                        basicProperties: null,
                        body: body);
                    Console.WriteLine("Send {0} Successed", message);

                }
            }
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
