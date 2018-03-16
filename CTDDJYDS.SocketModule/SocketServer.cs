using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace CTDDJYDS.SocketModule
{
    public delegate void ServerMsgReceivedHandle(object sender, ServerMsgArg serverMsgArt);
    public class SocketServer : IDisposable
    {
        #region Field
        private IPEndPoint serverInfo;
        private Socket serverSocket;
        private System.ComponentModel.BackgroundWorker serverWorker;
        private List<Socket> clientSocket;
        private byte[] recvMsgBuffer;
        private byte[] sendMsgBuffer;
        private string password = string.Empty;
        private bool isServerStarted = false;
        private object lockObj;
        private bool haveCheckedPW = false;
        private bool showMsg = false;

        private Queue transmitQueue = new Queue();
        private ReaderWriterLock transmitLock = new ReaderWriterLock();
        private ManualResetEvent stopEvent = null;
        private AutoResetEvent dataReady = null;

        #endregion

        #region Property

        public bool IsServerStarted
        {
            get { return isServerStarted; }
            set { isServerStarted = value; }
        }

        #endregion

        #region Event 处理接收到的消息
        private event ServerMsgReceivedHandle onServerMsgRecv;
        public event ServerMsgReceivedHandle OnServerMsgRecv
        {
            add { onServerMsgRecv += value; }
            remove { onServerMsgRecv -= value; }
        }

        #endregion

        #region Public Method

        public SocketServer(string password)
        {
            if (string.IsNullOrEmpty(password))
                this.password = string.Empty;
            else
                this.password = password;

            clientSocket = new List<Socket>();
            recvMsgBuffer = new byte[65535];

            lockObj = new object();

            stopEvent = new ManualResetEvent(false);
            dataReady = new AutoResetEvent(false);
        }

        public int StartServer()
        {
            try
            {
                if (serverSocket != null)
                {
                    serverSocket.Close();
                }

                if (serverWorker != null && serverWorker.IsBusy)
                {
                    serverWorker.CancelAsync();
                    serverWorker.DoWork -= ServerWorker_DoWork;
                    serverWorker = null;
                }
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverInfo = new IPEndPoint(IPAddress.Any, 10101);
                serverSocket.Bind(serverInfo);
                serverSocket.Listen(50);

                serverWorker = new System.ComponentModel.BackgroundWorker();
                serverWorker.WorkerSupportsCancellation = true;
                serverWorker.WorkerReportsProgress = false;
                serverWorker.DoWork -= ServerWorker_DoWork;
                serverWorker.DoWork += ServerWorker_DoWork;
                serverWorker.RunWorkerAsync();

                stopEvent.Reset();
                ThreadPool.QueueUserWorkItem(new WaitCallback(SendThread));

                isServerStarted = true;

                return 1;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                isServerStarted = false;
                return 0;
            }
        }

        public void StopServer()
        {
            Clear();
        }

        public void SendCmd(ServerMsgArg arg)
        {
            if (!isServerStarted || !haveCheckedPW || arg == null)
                return;

            transmitLock.AcquireWriterLock(-1);
            try
            {
                transmitQueue.Enqueue(arg);
            }
            catch (Exception) { }
            finally
            {
                transmitLock.ReleaseWriterLock();
            }

            dataReady.Set();
        }

        public void SendBuffer(SocketBuffer arg)
        {
            if (!isServerStarted || !haveCheckedPW || arg.buffer == null || arg.buffer.Length == 0)
                return;

            transmitLock.AcquireWriterLock(-1);
            try
            {
                transmitQueue.Enqueue(arg);
            }
            catch (Exception) { }
            finally
            {
                transmitLock.ReleaseWriterLock();
            }

            dataReady.Set();

            if (showMsg)
                Trace.WriteLine(string.Format("{0}-Send Buffer leng- {1}", DateTime.Now.ToString("HH:mm:ss:fff"), arg.buffer.Length));
        }

        public void ShowHideMsg(bool showMsg)
        {
            this.showMsg = showMsg;
        }

        public void Dispose()
        {
            Clear();
        }

        #endregion

        #region Struct  Buffer
        public byte[] StrutsToBytesArray(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            try
            {
                byte[] bytes = new byte[size];
                Marshal.StructureToPtr(structObj, structPtr, false);
                Marshal.Copy(structPtr, bytes, 0, size);
                return bytes;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(structPtr);
            }
        }

        public object BytesToStruts(byte[] bytes, Type type)
        {
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return null;
            }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, structPtr, size);
                object obj = Marshal.PtrToStructure(structPtr, type);

                return obj;
            }
            catch
            {
                return null;
            }
            finally
            {
                Marshal.FreeHGlobal(structPtr);
            }

        }
        #endregion

        #region Private Method

        private void ServerWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    Socket socket = serverSocket.Accept();
                    socket.BeginReceive(recvMsgBuffer, 0, recvMsgBuffer.Length, SocketFlags.None, 
                        new AsyncCallback(RecieveCallBack), socket);
                }
            }
            catch (System.Exception)
            {
            }
        }

        private void RecieveCallBack(IAsyncResult AR)
        {
            try
            {
                Socket rSocket = AR.AsyncState as Socket;
                int REnd = rSocket.EndReceive(AR);
                if (REnd <= 0)
                {
                    lock (lockObj)
                    {
                        if (clientSocket.Contains(rSocket))
                            clientSocket.Remove(rSocket);
                    }

                    if (rSocket != null)
                    {
                        if (rSocket.Connected)
                        {
                            rSocket.Shutdown(SocketShutdown.Both);
                            Thread.Sleep(10);
                        }
                        rSocket.Close();
                    }

                    return;
                }

                byte[] recvBuff = new byte[REnd];
                Array.Copy(recvMsgBuffer, 0, recvBuff, 0, REnd);
                object obj = BytesToStruts(recvBuff, typeof(SocketMessage));
                if (obj != null && obj is SocketMessage)
                {
                    SocketMessage arg = (SocketMessage)obj;
                    MsgType msgType = (MsgType)arg.msgType;
                    if (showMsg)
                        Trace.WriteLine(string.Format("{0}-Received Msg - {1}", DateTime.Now.ToString("HH:mm:ss:fff"), msgType));
                    switch (msgType)
                    {
                        case MsgType.CheckPassword:
                            string clientPW = Encoding.UTF8.GetString(arg.strParameter).Trim('\0');
                            if (string.Compare(clientPW, password, false) == 0)
                            {
                                haveCheckedPW = true;
                                ServerMsgArg checkPWArg = new ServerMsgArg(rSocket, arg);
                                HandleServerMsgRecv(checkPWArg);
                            }
                            else
                            {
                                haveCheckedPW = false;
                                SocketMessage msg = new SocketMessage();
                                msg.msgType = (int)MsgType.CheckPassword_ECHO;
                                msg.fParameter0 = 0;
                                sendMsgBuffer = StrutsToBytesArray(msg);
                                if (sendMsgBuffer != null)
                                    rSocket.Send(sendMsgBuffer);

                                if (rSocket != null)
                                {
                                    if (rSocket.Connected)
                                    {
                                        rSocket.Shutdown(SocketShutdown.Both);
                                        Thread.Sleep(10);
                                    }
                                    rSocket.Close();
                                }
                            }
                            break;                      
                        case MsgType.SetName:
                            {
                                ServerMsgArg serverArg = new ServerMsgArg(rSocket, arg);
                                HandleServerMsgRecv(serverArg);
                                rSocket.BeginReceive(recvMsgBuffer, 0, recvMsgBuffer.Length, SocketFlags.None, 
                                    new AsyncCallback(RecieveCallBack), rSocket);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return;
            }
        }

        private void Clear()
        {
            try
            {
                stopEvent.Set();

                isServerStarted = false;

                if (serverWorker != null && serverWorker.IsBusy)
                {
                    serverWorker.CancelAsync();
                    serverWorker.DoWork -= ServerWorker_DoWork;
                    serverWorker = null;
                }

                lock (lockObj)
                {
                    foreach (Socket socket in clientSocket)
                    {
                        if (socket != null)
                        {
                            if (socket.Connected)
                            {
                                socket.Shutdown(SocketShutdown.Both);
                                Thread.Sleep(10);
                            }
                            socket.Close();
                        }
                    }
                    clientSocket.Clear();
                }

                if (serverSocket != null)
                {
                    serverSocket.Close();
                    serverSocket = null;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return;
            }
        }

        private void HandleServerMsgRecv(ServerMsgArg args)
        {
            if (onServerMsgRecv != null)
            {
                onServerMsgRecv(this, args);
            }
        }

        private void SendThread(object state)
        {
            try
            {
                Queue workQueue = new Queue();
                while (true)
                {
                    WaitHandle[] handles = new WaitHandle[2];
                    handles[0] = stopEvent;
                    handles[1] = dataReady;
                    int result = WaitHandle.WaitAny(handles);
                    if (result == 0)
                    {
                        break;
                    }
                    else if (isServerStarted)
                    {
                        transmitLock.AcquireWriterLock(-1);
                        try
                        {
                            workQueue.Clear();
                            foreach (var message in transmitQueue)
                            {
                                workQueue.Enqueue(message);
                            }
                            transmitQueue.Clear();
                        }
                        catch (Exception) { }
                        finally
                        {
                            transmitLock.ReleaseWriterLock();
                        }

                        foreach (var message in workQueue)
                        {
                            if (message is ServerMsgArg)
                            {
                                SendMsg(message as ServerMsgArg);
                            }
                            else if (message is SocketBuffer)
                            {
                                SocketBuffer sbf = message as SocketBuffer;
                                sbf.SendBuffer();
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void SendMsg(ServerMsgArg arg)
        {
            try
            {
                Socket rSocket = arg.socket;
                SocketMessage msg = arg.msg;
                MsgType type = (MsgType)msg.msgType;
                if (showMsg)
                    Trace.WriteLine(string.Format("{0}-Send Msg - {1}", DateTime.Now.ToString("HH:mm:ss:fff"), type));
                byte[] buffer = null;
                switch (type)
                {
                    case MsgType.CheckPassword_ECHO:
                        if (rSocket != null)
                        {
                            buffer = StrutsToBytesArray(msg);
                            if (buffer != null)
                            {
                                rSocket.Send(buffer);
                            }
                            if (msg.fParameter0 == 1)
                            {
                                clientSocket.Add(rSocket);
                                rSocket.BeginReceive(recvMsgBuffer, 0, recvMsgBuffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallBack), rSocket);
                            }
                            else
                            {
                                if (rSocket != null)
                                {
                                    if (rSocket.Connected)
                                    {
                                        rSocket.Shutdown(SocketShutdown.Both);
                                        Thread.Sleep(10);
                                    }
                                    rSocket.Close();
                                }
                            }
                        }
                        break;
                    case MsgType.SetName_ECHO:
                        if (rSocket != null)
                        {
                            buffer = StrutsToBytesArray(msg);
                            if (buffer != null)
                            {
                                rSocket.Send(buffer);
                            }
                        }
                        break;                 
                        //buffer = StrutsToBytesArray(msg);
                        //if (buffer != null)
                        //{
                        //    Socket badClient = null;
                        //    foreach (Socket client in clientSocket)
                        //    {
                        //        try
                        //        {
                        //            client.Send(buffer);
                        //        }
                        //        catch (Exception)
                        //        {
                        //            badClient = client;
                        //            continue;
                        //        }
                        //    }

                        //    if (badClient != null)
                        //        clientSocket.Remove(badClient);
                        //}

                        //break;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return;
            }
        }
        #endregion

    }
    public struct SocketMessage
    {
        public int msgType;

        public float fParameter0;
        public float fParameter1;
        public float fParameter2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
        public byte[] strParameter;
    };
    public class SocketBuffer
    {
        public Socket socket;
        public byte[] buffer;

        public SocketBuffer(Socket socket, byte[] buf)
        {
            this.socket = socket;
            if (buf != null && buf.Length > 0)
            {
                this.buffer = new byte[buf.Length];
                Array.Copy(buf, this.buffer, buf.Length);
                Thread.Sleep(50);
            }
        }

        public void SendBuffer()
        {
            if (socket != null && buffer != null && buffer.Length > 0)
                socket.Send(buffer);
        }
    }

    public class ServerMsgArg
    {
        public Socket socket;
        public SocketMessage msg;

        public ServerMsgArg()
        {
            socket = null;
        }

        public ServerMsgArg(Socket socket, SocketMessage msg)
        {
            this.socket = socket;
            this.msg = msg;
        }
    }
    public enum MsgType
    {
        None = 0,
        CheckPassword,
        CheckPassword_ECHO,
        SetName,
        SetName_ECHO,
    }
}
