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
    public class QdisaStateMachine : QdisaState, IStateMachine
    {
        public IState CurState
        {
            get
            {
                return _currentState;
            }
        }

        public IState DefaultState
        {
            get
            {
                return _defaultState;
            }
            set
            {
                AddState(value);
                _defaultState = value;
            }

        }

        public Dictionary<string, IState> States;
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="name"></param>
        public QdisaStateMachine(string name, IState defaultState) : base(name)
        {
            _states = new List<IState>();
            _defaultState = defaultState;
        }

        public IState GetStateWithName(string name)
        {
            return null;
        }

        public IState GetStateWithTag(string tag)
        {
            return null;
        }

        public void AddState(IState state)
        {
            if (state != null && !_states.Contains(state))
            {
                _states.Add(state);
                //将加入的新状态的parent设置为当前状态机
                state.Parent = this;
                if (_defaultState == null)
                {
                    _defaultState = state;
                }
            }
        }



        public void RemoveState(IState state)
        {
            //在状态机运行过程中不能删除
            if (_currentState == state)
            {
                return;
            }
            if (state != null && _states.Contains(state))
            {
                _states.Remove(state);
                //将已经移除的状态parent设置为空
                state.Parent = null;
                if (_defaultState == state)
                {
                    _defaultState = (_states.Count >= 1) ? _states[0] : null;
                }
            }
        }
        public override void UpdateCallback(float deltaTime)
        {
            if (_isTransition)
            {
                if (_t.TransitionCallback())
                {
                    DoTranstion(_t);
                    _isTransition = false;
                }
                return;
            }
            base.UpdateCallback(deltaTime);
            if (_currentState == null)
            {
                _currentState = _defaultState;
            }
            List<ITransition> ts = _currentState.Transitions;
            int count = ts.Count;
            for (int i = 0; i < count; i++)
            {
                ITransition t = ts[i];
                if (t.ShouldBegin())
                {
                    _isTransition = true;
                    _t = t;
                    return;
                }
            }
            _currentState.UpdateCallback(deltaTime);
        }
        /// <summary>
        /// 开始进行过渡
        /// </summary>
        /// <param name="t"></param>
        private void DoTranstion(ITransition t)
        {
            _currentState.ExitCallback(t.ToState);
            _currentState = t.ToState;
            _currentState.EnterCallback(t.FromState);
        }
        public void SetCurState(IState state)
        {

        }
        private IState _currentState;
        private IState _defaultState;
        private List<IState> _states;    //所有状态
        private bool _isTransition = false;
        private ITransition _t; //当前正在执行的过渡
    }
}

