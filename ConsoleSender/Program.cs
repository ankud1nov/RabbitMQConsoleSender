using System;
using System.Threading;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;

namespace ConsoleSender
{
    class Program
    {
        static int Main(string[] args)
        {
            Message message = new Message("Comment");
            var factory = new ConnectionFactory() { HostName = "localhost"};
            while (true)
            {
                try
                {
                    using (var connection = factory.CreateConnection())
                        using (var channel = connection.CreateModel())//Инициализация подключения
                    {
                        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);//Описание очереди

                        var text = "";
                        while (text != "0")
                        {
                            var messageText = message.GetMessage();
                            var body = Encoding.UTF8.GetBytes(messageText);
                            try
                            {
                                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                                Console.WriteLine($" [x] Sent {messageText}");
                                message.Next(GetMessage(args));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($" [x] Sent {ex.Message}");
                                //throw;
                            }
                            Thread.Sleep(1000);
                        }

                    }
                }
                catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException ex)
                {
                    Console.WriteLine($" [x] Error {{{ex.Message}}}");
                }
            }
            return 0;
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "No comments");
        }

    }
}
