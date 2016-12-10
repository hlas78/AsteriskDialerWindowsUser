using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AsteriskDialerWindowsUser
{
    class AsteriskClient
    {
        private string server;
        private int port;
        private string user;
        private string pwd;

        private static ManualResetEvent connectDone = new ManualResetEvent(false);
        private static ManualResetEvent sendDone = new ManualResetEvent(false);
        private static ManualResetEvent receiveDone = new ManualResetEvent(false);

        private static String response = String.Empty;

        private Socket client;

        private Logger log;

        public AsteriskClient(string server, int port, string user, string pwd)
        {
            this.server = server;
            this.port = port;
            this.user = user;
            this.pwd = pwd;
        }

        private class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 1024;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        public int StartClient()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(server);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public int Login()
        {
            try
            {
                string login = "Action: Login\r\nUsername: " + user + "\r\nSecret: " + pwd + "\r\nEvents: On\r\n\r\n";
                Send(client, login);
                sendDone.WaitOne();

                Receive(client);
                receiveDone.WaitOne();
                if (response.IndexOf("Response: Success") != -1)
                {
                    Console.WriteLine("Successful login");
                    return 1;
                }
                Console.WriteLine("Unsuccesful login:" + response);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public int Dial(string sNumber, string sExt, string sContext)
        {
            try
            {
                string dial = "Action: Originate\r\nChannel: " + sExt + "\r\nExten: " + sNumber + "\r\nContext: " + sContext + "\r\nPriority: 1\r\n\r\n";

                Send(client, dial);
                sendDone.WaitOne();

                Receive(client);
                receiveDone.WaitOne();
                if (response.IndexOf("Message: Originate successfully queued") != -1)
                {
                    Console.WriteLine("Successful dial");
                    return 1;
                }
                Console.WriteLine("Unsuccesful dial: " + response);
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        public void Ping()
        {
            try
            {
                string ping = "Action: Ping\r\n\r\n";

                Send(client, ping);
                sendDone.WaitOne();

                Receive(client);
                receiveDone.WaitOne();
                Console.WriteLine(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public int StopClient()
        {
            try
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                client.Dispose();
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return 0;
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                StateObject state = new StateObject();
                state.workSocket = client;

                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
                    response = state.sb.ToString();
                    if (response.IndexOf("\r\n\r\n") != -1)
                    {
                        state.sb.Clear();
                        receiveDone.Set();
                    }
                    else
                    {
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                    }
                }
                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket client, String data)
        {
            //response = String.Empty;
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
