using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ServerTest
{
    class Server
    {
        TcpListener listener;
        Thread t;
        bool running;

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
            listener.Start();
        }

        public void Start()
        {
            t = new Thread(new ThreadStart(AsyncStart));
            t.Start();
        }

        private void AsyncStart()
        {
            while (running)
            {
                TcpClient client = listener.AcceptTcpClient();
                new Thread(() => StartConnection(client)).Start();
            }
        }

        private void StartConnection(TcpClient client)
        {
            while (true)
            {
                byte[] buffer = new byte[client.ReceiveBufferSize];
                client.GetStream().Read(buffer, 0, client.ReceiveBufferSize);
                string text = Encoding.ASCII.GetString(buffer);
                text = text.Substring(0, text.LastIndexOf("%"));
                Console.WriteLine(text);
            }
        }

        public void Stop()
        {
            t.Join();
        }
    }
}