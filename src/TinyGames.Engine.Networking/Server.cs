using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TinyGames.Engine.Networking
{
    public class Server<T>
    {
        public delegate void ServerListener(Server<T> server);
        public delegate void ServerExceptionListener(Server<T> server, Exception ex);
        public delegate void ServerClientListener(Server<T> server, Client<T> client);

        public event ServerListener OnServerStarted;
        public event ServerClientListener OnClientConnected;
        public event ServerClientListener OnClientDisconnected;
        public event ServerExceptionListener OnServerException;
        public IMessageParser<T> Parser { get; private set; }

        public int Port { get; }

        private List<Client<T>> _clients;

        public Server(IMessageParser<T> parser, int port)
        {
            _clients = new List<Client<T>>();

            Port = port;
            Parser = parser;
        }

        public void StartListenThread()
        {
            new Thread(Listen).Start();
        }

        public void Listen()
        {
            try
            {
                TcpListener server = new TcpListener(IPAddress.Any, Port);

                server.Start();

                OnServerStarted?.Invoke(this);

                while (true)
                {
                    TcpClient rawClient = server.AcceptTcpClient();

                    Client<T> client = new Client<T>(Parser, rawClient);

                    client.OnConnected += (c) =>
                    {
                        _clients.Add(c);
                        OnClientConnected?.Invoke(this, c);
                    };

                    client.OnDisconnected += (c) =>
                    {
                        _clients.Remove(c);
                        OnClientDisconnected?.Invoke(this, c);
                    };

                    client.StartListenThread();
                }
            }
            catch (Exception ex)
            {
                OnServerException?.Invoke(this, ex);
            }
        }
    }
}
