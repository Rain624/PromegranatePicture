/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 *
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qdisa;

public class ClientExample : MonoBehaviour {
    public static NetParam netParam = new NetParam("192.168.21.20", 8090, NetParam.NetProtocol.TCP);
    Client client = new Client(netParam);
    private string s;

    // Use this for initialization
    void Start () {
        client.StartClient();
        client.OnReceive+= (s) => { Debug.Log(s); };

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            client.SendMessage("wa");
    }
    /// <summary>
    /// 必须存在
    /// </summary>
    private void OnDestroy()
    {
        client.OnDestroy();
    }
    private void Debugs(string s)
    {
        Debug.Log(s);
    }

}


