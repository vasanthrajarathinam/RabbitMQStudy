using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FanOutConsumer
{
    class Program
    {
        static IConnection connection;
        static IModel channel;
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


            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;


            var consumerTag = channel.BasicConsume("my.queue1", false, consumer);

            Console.WriteLine("press any key to exit");
            Console.ReadKey();




            //Console.ReadLine();
        }

        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Body.ToArray());
            Console.WriteLine("Message:" + message);
            channel.BasicNack(e.DeliveryTag, false,false);
        }
    }
}
