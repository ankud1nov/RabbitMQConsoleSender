using System;
using System.Threading;
using System.Text;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsoleSender
{
    class Sender
    {
        ConnectionFactory factory;
        string queueName;
        bool durable;
        bool exclusive;
        bool autoDelete;
        IDictionary<string, object> arguments;
        Message message;

        public Sender(ConnectionFactory factory, Message message)
        {
            this.factory = factory;
            this.message = message;
            queueName = "hello";
            durable = false;
            exclusive = false;
            autoDelete = true;
            arguments = null;
            Thread myThread = new Thread(new ThreadStart(InitInNewThread));
            myThread.Start();
        }
        public Sender(ConnectionFactory factory, string queueName, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments = null)
        {
            this.factory = factory;
            this.queueName = queueName;
            this.durable = durable;
            this.exclusive = exclusive;
            this.autoDelete = autoDelete;
            this.arguments = arguments;
            Thread myThread = new Thread(new ThreadStart(InitInNewThread));
            myThread.Start();
        }

        void InitInNewThread()
        {
            while (true)
            {
                try
                {
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())//Инициализация подключения
                    {
                        channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);//Описание очереди

                        while (true)
                        {
                            var messageText = message.GetMessage();
                            var body = Encoding.UTF8.GetBytes(messageText);
                            try
                            {
                                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
                                Console.WriteLine($" [x] Sent {messageText}");
                                message.Next(GetMessage(new string[0]));
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
                    Init();
                }
            }
        }
        void Init()
        {
            Console.WriteLine("Перезапуск");
            Thread myThread = new Thread(new ThreadStart(InitInNewThread));
            myThread.Start();
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "No comments");
        }
    }
}
