using System;

namespace CSC
{
    class Program
    {
        private const string ServerHost = "irautox.ir";
        private const int ServerPort = 7676;

        static void Main(string[] args)
        {
            Console.Title = "Clash Of Drayvens Client";
            Console.WriteLine($"Connecting to {ServerHost}:{ServerPort}...");

            Client client = new Client();
            client.Connect(ServerHost, ServerPort);
        }
    }
}
