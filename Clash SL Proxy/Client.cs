using System;
using System.Net;
using System.Net.Sockets;

namespace CSP
{
    public class Client : ClientCrypto
    {
        public ClientState state = new ClientState();

        public Client(ServerState serverstate)
        {
            state.serverState = serverstate;
            state.clientKey = clientKey;
            state.serverKey = serverKey;
        }

        public void StartClient()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(CSP.Proxy.upstreamHost);
                IPAddress ipAddress = Array.Find(ipHostInfo.AddressList, a => a.AddressFamily == AddressFamily.InterNetwork)
                                      ?? ipHostInfo.AddressList[0];
                IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, CSP.Proxy.upstreamPort);

                Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                state.socket = socket;
                socket.Connect(remoteEndPoint);
                socket.BeginReceive(state.buffer, 0, State.BufferSize, 0, ReceiveCallback, state);

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("[DRAYVENS]");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("[UPSTREAM]");
                Console.ResetColor();
                Console.WriteLine(" Connected to {0}", socket.RemoteEndPoint);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Unable to connect to Drayvens upstream: " + e.Message);
                Console.ResetColor();
            }
        }
    }
}
