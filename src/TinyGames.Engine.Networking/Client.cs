using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TinyGames.Engine.Networking
{
    public class Client<T>
    {
        public delegate void ClientListener(Client<T> client);
        public delegate void ClientExceptionListener(Client<T> client, Exception ex);

        public delegate void ClientMessageListener(Client<T> client, T message);

        public event ClientListener OnConnected;
        public event ClientMessageListener OnMessage;
        public event ClientListener OnDisconnected;

        public event ClientExceptionListener OnException;

        public string IP { get; private set; }
        public int Port { get; private set; }
        public TcpClient RawSocket { get; private set; }

        public IMessageParser<T> Parser { get; private set; }

        public bool IsConnected => RawSocket != null && RawSocket.Connected;

        public Client(IMessageParser<T> parser, TcpClient tcp)
        {
            RawSocket = tcp;

            IP = "unknown";
            Port = 0;

            Parser = parser;
        }
        public Client(IMessageParser<T> parser, string ip, int port)
        {
            IP = ip;
            Port = port;

            Parser = parser;
        }

        public void StartListenThread()
        {
            Setup();
            new Thread(Listen).Start();
        }

        private void Setup()
        {
            if (RawSocket == null)
            {
                RawSocket = new TcpClient(IP, Port);
            }
        }

        public void Listen()
        {
            try
            {
                Setup();

                OnConnected?.Invoke(this);

                NetworkStream stream = RawSocket.GetStream();

                foreach(var message in Parser.ParseStream(stream))
                {
                    OnMessage?.Invoke(this, message);
                }
            }
            catch (Exception ex)
            {
                OnException?.Invoke(this, ex);
            }

            OnDisconnected?.Invoke(this);
        }

        public void Send(string line)
        {
            NetworkStream stream = RawSocket.GetStream();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(line + "\n");

            stream.Write(data, 0, data.Length);
        }
    }
}
