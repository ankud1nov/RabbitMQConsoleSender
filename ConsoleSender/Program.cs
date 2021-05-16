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
            Sender sender = new Sender(factory, message);
            //Console.WriteLine("Press Any key to exit");
            Console.ReadLine();
            return 0;
        }

        

    }
}
