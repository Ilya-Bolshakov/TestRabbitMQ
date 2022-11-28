using System;
using RabbitMQ.Client;
using System.Text;



namespace Send
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var factory = new ConnectionFactory() 
            {
                HostName = "192.168.50.10",
                Port = 5672,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = new TimeSpan(0, 1, 0),
                RequestedHeartbeat = new TimeSpan(0, 10, 0),
                UserName = "guest",
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("PSClientPays", true ? ExchangeType.Fanout : ExchangeType.Direct, true, false, null);

                channel.QueueDeclare(queue: "PSClientPays",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                channel.QueueBind("PSClientPays", "PSClientPays", "");

                string message = "Bolshakov message";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "PSClientPays",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine(" [x] Sent {0}", message);
                Console.ReadLine();
            }
        }
    }
}