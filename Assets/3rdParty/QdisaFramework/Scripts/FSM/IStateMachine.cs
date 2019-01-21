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
    /// <summary>
    /// 状态机接口
    /// </summary>
    public interface IStateMachine
    {
        IState CurState { get; }
        IState DefaultState { get; set; }
        void AddState(IState state);
        void RemoveState(IState state);
        void UpdateCallback(float deltaTime);
        void SetCurState(IState state);
        IState GetStateWithTag(string tag);
        IState GetStateWithName(string name);
    }
}
