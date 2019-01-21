/****************************************************************************
 *
 * Copyright (c) 2019 Rain
 *
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;

public class ConnectionPool
{
    public const int BUFFER_SIZE = 1024;
    public Socket clientSocket;
    public bool isConnected = false;
    public byte[] readBuff = new byte[BUFFER_SIZE];
    //缓冲区收到的字节数
    public int buffCount = 0;
    public ConnectionPool()
    {
        readBuff = new byte[BUFFER_SIZE];
    }
    public void Init(Socket socket)
    {
        this.clientSocket = socket;
        isConnected = true;
        buffCount = 0;
    }
    /// <summary>
    /// 计算缓冲区剩余的字节数
    /// </summary>
    /// <returns>剩余字节数</returns>
    public int BuffRemain()
    {
        return BUFFER_SIZE - buffCount;
    }
    public string GetAddress()
    {
        if (!isConnected)
            return "连接断开，无法获取地址";
        return clientSocket.RemoteEndPoint.ToString();
    }
    public void Close()
    {
        if (!isConnected)
            return;
        Debug.Log("断开连接" + GetAddress());
        clientSocket.Close();
        isConnected = false;
    }

}


