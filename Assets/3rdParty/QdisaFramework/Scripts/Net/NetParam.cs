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

namespace Qdisa
{
    public class NetParam
    {

        private string ipAddress;
        private int port;
        private ProtocolType protocol;
        public  enum NetProtocol
        {
            TCP,
            UDP
        }
        //public  NetProtocol netProtocolType;
        public NetParam(string ipAddress,int port,NetProtocol netProtocol)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.protocol = JudgeProtocol(netProtocol);
        }
        //public NetProtocol NetProtocolType
        //{
        //    get
        //    {
        //        return netProtocolType;
        //    }
        //    set
        //    {
        //        netProtocolType = value;
        //    }
        //}
        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
            set
            {
                ipAddress = value;
            }
        }
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }
        public ProtocolType Protocol
        {
            get
            {
                return protocol;
            }
            set
            {
                protocol = value;
            }
        }
        private ProtocolType JudgeProtocol(NetProtocol netProtocol)
        {
            ProtocolType protocolType=ProtocolType.Tcp;
            switch (netProtocol)
            {
                case NetProtocol.TCP: protocolType=ProtocolType.Tcp;
                    break;
                case NetProtocol.UDP:protocolType=ProtocolType.Udp;
                    break;
            }
            return protocolType;
        }
    }
}





