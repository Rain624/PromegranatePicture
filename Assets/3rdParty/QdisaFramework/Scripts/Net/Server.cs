/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 *
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Text;


namespace Qdisa
{

    public class Server
    {
        //public Action<string> OnReceive;
        public event Action<string> OnReceive;
        private NetParam netParam;
        private Socket serverSocket;
        private Thread serverThread;
        private string command;
        private static MessageHandle messageHandle = new MessageHandle();

        private  List<Socket> ClientSockets = new List<Socket>();
        //private  List<Socket> ServerSockets = new List<Socket>();

        public Server(NetParam netParam)

        {
            this.netParam = netParam;
        }
        public void StartServer()
        {
            serverThread = new Thread(OpenServer);
            serverThread.IsBackground = true;

            serverThread.Start();
        }

    
        private void OpenServer()
        {
            ServerParam(netParam);
        }
        private void ServerParam(NetParam netParam)
        {
            IPAddress IP = IPAddress.Parse(netParam.IpAddress);
            IPEndPoint ipEp = new IPEndPoint(IP, netParam.Port);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, netParam.Protocol);
            serverSocket.Bind(ipEp);
            serverSocket.Listen(100);
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

        }
        private void AcceptCallBack(IAsyncResult ar)
        {
                Socket serverSocket = ar.AsyncState as Socket;
                Socket clientSocket = serverSocket.EndAccept(ar);
                ClientSockets.Add(clientSocket);
                string msg = "你好";
                SendMessage(clientSocket, msg);
                 Debug.Log("客户端连接上了");
                clientSocket.BeginReceive(messageHandle.Data, messageHandle.StartIndex, messageHandle.RemainSize, 
                    SocketFlags.None, ReceiveCallBack, clientSocket);
                serverSocket.BeginAccept(AcceptCallBack, serverSocket);
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            Socket clientSocket = ar.AsyncState as Socket;
            try
            {
                if (clientSocket == null || clientSocket.Connected == false)
                    return;
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    clientSocket.Close();
                    return;
                }
                //无法解析没有数据长度前缀的数据
                messageHandle.ReadMessage(count,ReceiveMessage);
                clientSocket.BeginReceive(messageHandle.Data, messageHandle.StartIndex, messageHandle.RemainSize, 
                    SocketFlags.None, ReceiveCallBack, clientSocket);

            }
            catch (Exception e)
            {
                Debug.Log("接收时出现的错误" + e);
            }
        }
        /// <summary>
        /// 为了在接收到信息的时候提供一个事件
        /// </summary>
        /// <param name="s"></param>
        private void ReceiveMessage(string s)
        {
            if(OnReceive!=null)
            OnReceive(s);
        }
        public void SendMessage(string msg)
        {
            if (ClientSockets.Count > 0)
            {
                for(int i = 0; i < ClientSockets.Count; i++)
                {
                    SendMessage(ClientSockets[i], msg);
                }
            }
        }
        /// <summary>
        /// 发送数据的方法
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="msg"></param>
        private void SendMessage(Socket clientSocket, string msg)
        {
            if (clientSocket != null && clientSocket.Connected == true)
            {
                byte[] bytes= MessageHandle.PackData(msg);
                clientSocket.Send(bytes);
                //clientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, ReceiveCallBack, clientSocket);
            }
        }
    
        public void OnDestroy()
        {
            serverSocket.Close();
            if (serverThread != null)
            {
                for (int i = 0; i < ClientSockets.Count; i++)
                {
                    ClientSockets[i].Close();
                }
                serverThread.Abort();
            }
        }
    }
}


