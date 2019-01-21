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
    public delegate bool QdisaTransitionDelegate();


    public class QdisaTransition : ITransition
    {
        public event QdisaTransitionDelegate OnTransition;
        public event QdisaTransitionDelegate OnCheck;
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public IState FromState
        {
            get
            {
                return _fromState;
            }
            set
            {
                _fromState = value;
            }
        }
        public IState ToState
        {
            get
            {
                return _toState;
            }
            set
            {
                _toState = value;
            }
        }
        public QdisaTransition(string name, IState fromState, IState toState)
        {
            _name = name;
            _fromState = fromState;
            _toState = toState;
        }
        public bool TransitionCallback()
        {
            if (OnTransition != null)
            {
                return OnTransition();
            }
            return true;
        }
        /// <summary>
        /// 能否开始过渡
        /// </summary>
        /// <returns></returns>
        public bool ShouldBegin()
        {
            if (OnCheck != null)
            {
                return OnCheck();
            }
            return false;
        }
        private IState _fromState;
        private IState _toState;
        private string _name;

    }
}
