using System;
using System.Text;
using RabbitMQ.Client;

namespace TopicExchangePublisher
{
    class Program
    {
        public static IConnection connection;
        public static IModel channel;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";

            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare("ex.topic", "topic", true, false, null);
            channel.QueueDeclare("my.queue1", true, false, false, null);
            channel.QueueDeclare("my.queue2", true, false, false, null);
            channel.QueueDeclare("my.queue3", true, false, false, null);
            channel.QueueBind("my.queue1", "ex.topic", "*.image.*", null);
            channel.QueueBind("my.queue2", "ex.topic", "#.image", null);
            channel.QueueBind("my.queue3", "ex.topic", "image.#", null);

            channel.BasicPublish("ex.topic", "convert.image.bmp", null, Encoding.UTF8.GetBytes("convert.image.bmp"));
            channel.BasicPublish("ex.topic", "image.bmp", null, Encoding.UTF8.GetBytes("image.bmp"));
            channel.BasicPublish("ex.topic", "convert.image", null, Encoding.UTF8.GetBytes("convert.image"));
            channel.BasicPublish("ex.topic", "image", null, Encoding.UTF8.GetBytes("image"));

            Console.Write("press a key to exit");
            Console.ReadKey();

            channel.QueueDelete("my.queue1");
            channel.QueueDelete("my.queue2");
            channel.QueueDelete("my.queue3");
            channel.ExchangeDelete("ex.topic");

            channel.Close();
            connection.Close();


        }
    }
}
