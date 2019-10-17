using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CTDDJYDS.SocketModule
{
    public class SocketClient
    {
        private IPEndPoint serverInfo;
        private Socket clientSocket;
        private Byte[] msgBuffer;
        private object lockObj;
        private bool isConnected = false;
        public SocketClient()
        {
            msgBuffer = new byte[65535];
            lockObj = new object();
        }

        public int Connect(IPAddress serverIPAddress, string password)
        {
            try
            {
                Clear();

                serverInfo = new IPEndPoint(serverIPAddress, 2018);
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.BeginConnect(serverInfo, new AsyncCallback(OnConnected), password);
                //clientSocket.Connect(serverInfo);               

                return 1;
            }
            catch (Exception ex)
            {
 
                return 0;
            }
        }

        public int Disconnect()
        {
            return Clear();
        }
        private void OnConnected(IAsyncResult AR)
        {
            if (clientSocket == null)
                return;

            try
            {
                clientSocket.EndConnect(AR);

                string password = AR.AsyncState as string;
                //ServerMsgArg arg = new ServerMsgArg(MsgType.CheckPassword, null, password);
                byte[] buffer = new byte[1024];
                if (buffer != null)
                    clientSocket.Send(buffer);

                clientSocket.BeginReceive(msgBuffer, 0, msgBuffer.Length, SocketFlags.None, 
                    new AsyncCallback(ReceiveCallBack), null);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return;
            }

        }

        private void ReceiveCallBack(IAsyncResult AR)
        {
            try
            {
                if (clientSocket == null)
                    return;
                int REnd = clientSocket.EndReceive(AR);
                if (REnd <= 0)
                {
                    Clear();
                    return;
                }

                byte[] recvBuff = new byte[REnd];
                Array.Copy(msgBuffer, 0, recvBuff, 0, REnd);

                clientSocket.BeginReceive(msgBuffer, 0, msgBuffer.Length, 0, new AsyncCallback(ReceiveCallBack), null);

            }
            catch (Exception ex)
            {
                return;
            }
        }

        private int Clear()
        {
            try
            {
                lock (lockObj)
                {
                    isConnected = false;
                    if (clientSocket != null)
                    {
                        if (clientSocket.Connected)
                        {
                            clientSocket.Shutdown(SocketShutdown.Both);
                            Thread.Sleep(10);
                        }
                        clientSocket.Close();
                        clientSocket = null;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
               
                return 0;
            }
        }
    }
}
