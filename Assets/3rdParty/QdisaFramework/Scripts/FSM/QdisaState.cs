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
    public delegate void QdisaStateDelegate();
    public delegate void QdisaStateDelegateState(IState state);
    public delegate void QdisaStateDelegateFloat(float f);

    public class QdisaState : IState
    {
        public event QdisaStateDelegateState OnEnter;

        public event QdisaStateDelegateState OnExit;

        public event QdisaStateDelegateFloat OnUpdate;

        public float Timer
        {
            get
            {
                return _timer;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        public IStateMachine Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transition"></param>
        public void AddTransition(ITransition transition)
        {
            if (transition != null && !_transitions.Contains(transition))
            {
                _transitions.Add(transition);
            }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="name">指定的状态名</param>
        public QdisaState(string name)
        {
            _name = name;
            _transitions = new List<ITransition>();
        }

        public virtual void EnterCallback(IState prev)
        {
            //重置计时器
            _timer = 0f;
            if (OnEnter != null)
            {
                OnEnter(prev);
            }
        }

        public virtual void ExitCallback(IState next)
        {
            if (OnExit != null)
            {
                OnExit(next);
            }
            //重置计时器
            _timer = 0f;
        }

        public virtual void UpdateCallback(float deltaTime)
        {
            //累加计时器
            _timer += deltaTime;
            if (OnUpdate != null)
            {
                OnUpdate(deltaTime);
            }
        }
        public List<ITransition> Transitions
        {
            get
            {
                return _transitions;
            }
        }

        public void SetStateMachine(IStateMachine stateMachine)
        {

        }
        private string _name;
        private string _tag;
        private float _timer;    //状态运行时长
        private IStateMachine _parent;
        private List<ITransition> _transitions;
    }
}

