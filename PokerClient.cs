using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Poker
{
    internal class PokerClient
    {
        private Socket clientSocket;

        public PokerClient(string serverIP, int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverIP, port);
            Console.WriteLine("Connected to the server.");
        }

        public void SendMessage(string action, int amount = 0)
        {
            string message = $"{{\"action\":\"{action}\",\"amount\":{amount}}}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
            Console.WriteLine($"Sent: {message}");
        }

        public void ListenForMessages()
        {
            List<Socket> readSockets = new List<Socket> { clientSocket };

            while (true)
            {
                Socket.Select(readSockets, null, null, 1000); // Wait for data

                if (readSockets.Count > 0)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = clientSocket.Receive(buffer);
                    if (bytesRead > 0)
                    {
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Server: {response}");
                    }
                }
            }
        }

        public void Close()
        {
            clientSocket.Close();
        }
    }
}
