/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 *
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qdisa;
using System;

public class TimeExample : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Timer.AddTimer(5, "woshi")
            .OnUpdated (                       
            (float v)=> 
            {
                Debug.Log(v * 5);
            }
            )
          
            .OnCompleted(
            () =>
            {
                Completed();
            }
            );
	}
	void Completed()
    {
        Debug.Log("完成了");
    }
}


