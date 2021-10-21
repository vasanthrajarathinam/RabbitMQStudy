using System;
using System.Text;
using RabbitMQ.Client;

namespace DirectExchangeDemo
{
    class Program
    {
        static IConnection connection;
        static IModel channel;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare("ex.direct", "direct", true, false, null);
            channel.QueueDeclare("my.errors", true, false, false, null);
            channel.QueueDeclare("my.infos", true, false, false, null);
            channel.QueueDeclare("my.warnings", true, false, false, null);
            channel.QueueBind("my.errors","ex.direct","error",null);
            channel.QueueBind("my.infos", "ex.direct", "info", null);
            channel.QueueBind("my.warnings", "ex.direct", "warning", null);

            channel.BasicPublish("ex.direct", "info", null, Encoding.UTF8.GetBytes("Information Message from Code"));
            channel.BasicPublish("ex.direct", "warning", null, Encoding.UTF8.GetBytes("Warning Message from Code"));
            channel.BasicPublish("ex.direct", "error", null, Encoding.UTF8.GetBytes("Error Message from Code"));


            channel.QueueDelete("my.errors");
            channel.QueueDelete("my.warnings");
            channel.QueueDelete("my.infos");
            channel.ExchangeDelete("ex.direct");

            channel.Close();
            connection.Close();


        }
    }
}
