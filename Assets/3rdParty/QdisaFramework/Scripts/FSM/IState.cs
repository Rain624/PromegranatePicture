/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 * 
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qdisa
{
    public delegate void VoidStateDelegate();
    public delegate void VoidStateDelegateState(IState state);
    public delegate void VoidStateDelegateFloat(float f);

    public  interface IState
    {
        /// <summary>
        /// 从进入状态开始计算时长
        /// </summary>
        float Timer { get; }
        string Name { get; }
        string Tag { get; set; }
        void EnterCallback(IState prev);
        void ExitCallback(IState next);
        void UpdateCallback(float deltaTime);
        /// <summary>
        /// 添加过渡
        /// </summary>
        /// <param name="transition">状态过渡</param>
        void AddTransition(ITransition transition);
        /// <summary>
        /// 当前状态的状态机
        /// </summary>
        IStateMachine Parent { get; set; }
        void SetStateMachine(IStateMachine stateMachine);
        /// <summary>
        /// 状态过渡
        /// </summary>
        /// 当前状态中的所有过渡
        List<ITransition> Transitions { get; }
    }
}
