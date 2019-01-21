/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 * 
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Qdisa;

public class FSMExample : MonoBehaviour {
    private QdisaStateMachine testMachine;
    private QdisaState _testOpen;
    private QdisaState _testClose;
    private QdisaTransition _open2Close;
    private QdisaTransition _close2Open;

    private int _iTransition;

    public  bool _isClose;

    private void Start()
    {
        InitLightFsm();
    }
    private void FSMTest()
    {
        
    }
    private void Update()
    {
        testMachine.UpdateCallback(Time.deltaTime);
    }
    private void InitLightFsm()
    {

        _testOpen = new QdisaState("Open");

        _testOpen.OnEnter += (IState state) =>
        {

            Debug.Log("灯打开了");
        };
        _testOpen.OnUpdate += (float f) =>
          {
              
              Debug.Log("灯在打开的状态");
          };
        _testOpen.OnExit += (IState state) =>
        {
            Debug.Log("灯退出了打开的状态");
        };
       
        _testClose = new QdisaState("Close");
        _testClose.OnEnter += (IState state) =>

          {
              Debug.Log("灯进入熄灭状态");
          };
        _testClose.OnUpdate += (float f) =>
          {
              Debug.Log("灯在熄灭的状态");
          };
        _testClose.OnExit += (IState state) =>
          {
              Debug.Log("灯退出了熄灭的状态");
          };
        _open2Close = new QdisaTransition("Open2Close", _testOpen, _testClose);
        _open2Close.OnCheck += () => {
            return _isClose;
        };
        _open2Close.OnTransition += () => {
            return TestTransition();
        };
        _testOpen.AddTransition(_open2Close);

        _close2Open = new QdisaTransition("Close2Open", _testClose, _testOpen);
        _close2Open.OnCheck += () =>
        {
            return !_isClose;
        };
        _close2Open.OnTransition += () =>
        {
            return TestTransition();
        };
        _testClose.AddTransition(_close2Open);

        testMachine = new QdisaStateMachine("Root", _testOpen);

        //testMachine.AddState(_testOpen);
        //testMachine.AddState(_testClose);
    }
     
    private bool TestTransition()
    {
        Debug.Log("我在过渡状态读数" + _iTransition);
        _iTransition++;
        if (_iTransition == 100)
        {
            _iTransition = 0;
            return true;
        }
        else
        {
            return false;
        }
    }
}
