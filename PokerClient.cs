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
        private bool myTurn = false;

        public PokerClient(string serverIP, int port)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(serverIP, port);
            Console.WriteLine("Connected to the server.");
        }

        public void SendMessage(string action, int amount = 0)
        {
            if (!myTurn)
            {
                Console.WriteLine("Wait for your turn!");
                return;
            }
            string message = $"{{\"action\":\"{action}\",\"amount\":{amount}}}";
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
            Console.WriteLine($"Sent: {message}");
            myTurn = false;
        }

        public void ListenForMessages()
        {
            List<Socket> readSockets = new List<Socket>();

            while (true)
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    readSockets.Clear();
                    readSockets.Add(clientSocket);

                    if (readSockets.Count > 0)
                    {
                        Socket.Select(readSockets, null, null, 1000);
                    }

                    if (readSockets.Count > 0)
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = 0;

                        try
                        {
                            bytesRead = clientSocket.Receive(buffer);
                        }
                        catch (SocketException)
                        {
                            Console.WriteLine("Server disconnected.");
                            clientSocket.Close();
                            return;
                        }

                        if (bytesRead > 0)
                        {
                            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            Console.WriteLine($"Server: {response}");
                        }
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
