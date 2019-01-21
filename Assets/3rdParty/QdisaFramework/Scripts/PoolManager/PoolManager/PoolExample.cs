/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 * 
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qdisa;

public class PoolExample : MonoBehaviour {
    //private SpawnPool pool;
    public Transform prefab;
    //public AudioSource prefab;
    public string poolName;
    //public ParticleSystem prefab;
    // Use this for initialization
    void Start () {
        PoolManager.Pools[this.poolName].Spawn
           (
               this.prefab,
               this.transform.position,
               this.transform.rotation
           );
        //PoolManager.Pools["Audio"].Spawn(
        //          this.prefab,
        //    this.transform.position,
        //    this.transform.rotation

        //    );
        //pool = GetComponent<SpawnPool>();


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
      

        }
    }
}
