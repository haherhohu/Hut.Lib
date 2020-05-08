using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace Hut
{
    public class HutTCPServer
    {
        private TcpListener server;

        public HutTCPServer()
        {
        }

        public void Start(string address, int port)
        {
            try
            {
                IPAddress localaddr = IPAddress.Parse(address);
                server = new TcpListener(localaddr, port);
                server.Start();

                byte[] bytes = new byte[64];
                string data = null;

                while (true)
                {
                    Console.Write(@"Waiting for a connection... ");
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine(@"Connected!");

                    while (client.Connected == true)
                    {
                        NetworkStream stream = client.GetStream();
                        if (client.Connected == false)
                            break;

                        stream.Read(bytes, 0, 64);

                        Console.WriteLine(Encoding.UTF8.GetString(bytes));

                        data = @"0RC0A0A12345678C";
                        byte[] message = Encoding.ASCII.GetBytes(data);
                        stream.Write(message, 0, message.Length);

                        if (client.Connected == false)
                            break;

                        Array.Clear(bytes, 0, 64);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(@"SocketException: {0}", e);
            }
            catch (IOException ex)
            {
                Console.WriteLine(@"SocketIOException: {0}", ex);
            }
            finally
            {
                server.Stop();
            }
        }
    }
}

//    public delegate void TestTCPServerConnectionChanged(TestTCPServerConnection connection);
//    public delegate void TestTCPServerError(TestTCPServer server, Exception e);

//   public class TestTCPServer : Component
//{
//	public TestTCPServer
//	{
//	}

//}

/*
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace test
{
    public class StateObject
    {
        public Socket worksocket = null;
        public const int BUFFERSIZE = 1024;
        public byte[] buffer = new byte[BUFFERSIZE];
        public StringBuilder sb = new StringBuilder();
    }

    public class TestAsynchronousSocketControl
    {
        private const string ADDRESS = @"localhost";
        private const int PORT = 5555;
        private static ManualResetEvent connectdone = new ManualResetEvent(false);
        private static ManualResetEvent receivedone = new ManualResetEvent(false);
        private static ManualResetEvent senddone = new ManualResetEvent(false);
        private static ManualResetEvent alldone = new ManualResetEvent(false);
        private static string response = string.Empty;
        private static Socket handler;

        public TestAsynchronousSocketControl()
        {
        }

        #region server

        public static void startServer()
        {
            byte[] bytes = new byte[1024];
            IPHostEntry iphostinfo = Dns.GetHostEntry(@"localhost");
            IPAddress ipaddress = iphostinfo.AddressList[1];
            IPEndPoint localendpoint = new IPEndPoint(ipaddress, 5555);

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localendpoint);
                listener.Listen(100);

                while (true)
                {
                    alldone.Reset();

                    Console.WriteLine(@"waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(acceptCallback), listener);

                    alldone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void acceptCallback(IAsyncResult ar)
        {
            alldone.Set();

            Socket listener = (Socket)ar.AsyncState;
            handler = listener.EndAccept(ar);

            StateObject state = new StateObject();
            state.worksocket = handler;
        }

        #endregion server

        #region client

        public static void startClient()
        {
            try
            {
                byte[] bytes = new byte[1024];
                IPHostEntry iphostinfo = Dns.GetHostEntry("localhost");
                IPAddress ipaddress = iphostinfo.AddressList[1];
                IPEndPoint localendpoint = new IPEndPoint(ipaddress, 5555);

                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                client.BeginConnect(localendpoint, new AsyncCallback(connectCallback), client);
                connectdone.WaitOne();

                Receive(client);
                receivedone.WaitOne();

                Console.WriteLine(@"received : {0}", response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void connectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                client.EndConnect(ar);

                Console.WriteLine(@"Socket Connected to {0}", client.RemoteEndPoint.ToString());
                connectdone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion client

        public static void Send(String data)
        {
            byte[] bytedata = Encoding.ASCII.GetBytes(data);
            handler.BeginSend(bytedata, 0, bytedata.Length, 0, new AsyncCallback(sendCallback), handler);
        }

        private static void sendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;

                int sentbytes = handler.EndSend(ar);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Receive(Socket socket)
        {
            try
            {
                StateObject state = new StateObject();
                state.worksocket = socket;

                socket.BeginReceive(state.buffer, 0, StateObject.BUFFERSIZE, 0, new AsyncCallback(receiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void receiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket socket = state.worksocket;
                int bytesread = socket.EndReceive(ar);

                if (bytesread > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesread));
                    socket.BeginReceive(state.buffer, 0, StateObject.BUFFERSIZE, 0, new AsyncCallback(receiveCallback), state);
                }
                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    receivedone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
*/