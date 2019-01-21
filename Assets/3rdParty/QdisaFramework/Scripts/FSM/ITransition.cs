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
    /// 用于状态过渡的接口
    /// </summary>
    public interface ITransition
    {
        string Name { get; }
        /// <summary>
        /// 从哪个状态开始过渡
        /// </summary>
        IState FromState { get; set; }
        /// <summary>
        /// 过渡到哪个状态
        /// </summary>
        IState ToState { get; set; }
        /// <summary>
        /// 过渡是的回调
        /// </summary>
        /// <returns>返回true过渡结束false继续进行</returns>
        bool TransitionCallback();
        /// <summary>
        /// 能否开始过渡
        /// </summary>
        /// <returns>返回true开始进行过渡返回false不进行过渡</returns>
        bool ShouldBegin();

    }
}
