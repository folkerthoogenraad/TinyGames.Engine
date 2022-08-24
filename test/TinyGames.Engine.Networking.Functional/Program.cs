using System;

namespace TinyGames.Engine.Networking.Functional
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Networking test.");
            Console.WriteLine("Do you want to be [server] or [client]?");
           
            while (true)
            {
                Console.Write("> ");

                string line = Console.ReadLine();

                if (line == "server")
                {
                    StartServer();
                }
                else if (line == "client")
                {
                    StartClient();
                }
                else if (line == "exit")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Unknown command: <line>");
                }
            }
        }

        public static void StartServer()
        {
            Server<string> server = new Server<string>(new ASCIILineParser(), 8219);

            server.OnServerStarted += (server) => Console.WriteLine("Pingpong server started!");
            server.OnServerException += (server, exception) => Console.WriteLine("Server exception!");
            server.OnClientDisconnected += (server, client) => Console.WriteLine("Client disconnected!");

            server.OnClientConnected += (server, client) =>
            {
                Console.WriteLine("Client connected!");

                client.OnMessage += (c, message) => c.Send(message);
            };

            server.Listen();
        }

        public static void StartClient()
        {
            Client<string> client = new Client<string>(new ASCIILineParser(), "127.0.0.1", 8219);

            client.OnMessage += (c, message) => Console.WriteLine(message);

            client.StartListenThread();

            while (client.IsConnected)
            {
                string line = Console.ReadLine();

                client.Send(line);
            }
        }
    }
}
