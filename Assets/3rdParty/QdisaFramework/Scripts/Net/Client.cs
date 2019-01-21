/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 *
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace Qdisa
{

    public class Client 
    {
        public event Action<string> OnReceive;

        private NetParam netParam;
        private Socket clientSocket;
        private MessageHandle messageHandle = new MessageHandle();
        private IPEndPoint ipEP;

        //线程
        private Thread heartThread;
        private Thread clientThread;

        /// <summary>
        /// 客户端线程是否退出标志
        /// </summary>
        private bool clientFlag=true;

        private bool isConnected;
        
        public Client(NetParam netParam)
        {
            this.netParam = netParam;
        }
        public void StartClient()
        {
            clientThread = new Thread(OpenClient);
            clientThread.IsBackground = true;
            clientThread.Start();
        }
        private void OpenClient()
        {
            while (clientFlag)
            {

                Connect();
                Thread.Sleep(3000);
            }
        }
        #region 心跳包
        /// <summary>
        /// 用于开启线程
        /// </summary>
        private void Heart()
        {
            while (isConnected)
            {
                SendHeart(clientSocket);
                Thread.Sleep(2000);          
            }
        }
        /// <summary>
        /// 发送心跳包
        /// </summary>
        private void SendHeart(Socket socket)
        {
            SendMessage("");
        }
        /// <summary>
        /// 发送心跳包后的回调
        /// </summary>
        /// <param name="ar"></param>
        private void SendHeartCallBack(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndSend(ar);
        }
        #endregion
        /// <summary>
        /// 设置客户端socket的参数
        /// </summary>
        /// <param name="netParam"></param>
        private void SetParam(NetParam netParam)
        {
            IPAddress IP = IPAddress.Parse(netParam.IpAddress);
            ipEP = new IPEndPoint(IP, netParam.Port);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, netParam.Protocol);
        }
        /// <summary>
        /// 连接服务器
        /// </summary>
        private void Connect()
        {
            if (!isConnected)
            {

                SetParam(netParam);

                clientSocket.BeginConnect(ipEP, new AsyncCallback(ConnectCallBack), clientSocket);

            }
        }
        /// <summary>
        /// 连接后的回调
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallBack(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
   
            try
            {
                client.EndConnect(ar);
                isConnected = true;
                heartThread = new Thread(Heart);
                heartThread.IsBackground = true;
                heartThread.Start();
                if (clientSocket == null || clientSocket.Connected == false)
                    return;
                clientSocket.BeginReceive(messageHandle.Data, messageHandle.StartIndex, messageHandle.RemainSize,
                    SocketFlags.None, ReceiveCallBack, clientSocket);
            }
            catch (SocketException e)
            {
                isConnected = false;
                if (e.ErrorCode == 10061)
                {
                    Debug.Log("服务器程序未运行或服务器端口未开放");
                }
                else
                {
                    Debug.Log(e);
                }
            }

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

                messageHandle.ReadMessage(count, ReceiveMessage);//接收到命令时产生一个事件

                clientSocket.BeginReceive(messageHandle.Data, messageHandle.StartIndex, messageHandle.RemainSize,
              SocketFlags.None, ReceiveCallBack, clientSocket);
            }
            catch (Exception e)
            {

                Debug.Log(e);
                //isConnected = false;
            }
        }
        private void ReceiveMessage(string s)
        {
            if (OnReceive != null)
                OnReceive(s);
        }
        /// <summary>
        /// public服务器发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        public void SendMessage(string msg)
        {
            SendMessage(clientSocket, msg);
        }
        /// <summary>
        /// 给服务器发送消息
        /// </summary>
        /// <param name="clientSocket"></param>
        /// <param name="msg"></param>
        private void SendMessage(Socket clientSocket, string msg)
        {
            if (clientSocket != null && clientSocket.Connected == true)
            {
                try
                {
                    byte[] bytes = MessageHandle.PackData(msg);
                    clientSocket.BeginSend(bytes, 0, bytes.Length, 0,SendMesCallBack, clientSocket);
                }
                catch (SocketException e)
                {
                    Debug.Log(e);
                }
            }
            else
            {
                isConnected = false;
            }
        }
        private void SendMesCallBack(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndSend(ar);
        }
        /// <summary>
        /// 销毁物体
        /// </summary>
        public void OnDestroy()
        {
            clientFlag = false;
            isConnected = false;
            if(clientSocket!=null)
            clientSocket.Close();
        }
    }
}




