/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 * 
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Qdisa
{
    public class QMonoSingletonPath : MonoSingletonPath
    {
        public QMonoSingletonPath(string pathInHierarchy) : base(pathInHierarchy)
        {

        } 
    }
    [AttributeUsage(AttributeTargets.Class)]
    public class MonoSingletonPath : Attribute
    {
        private string mPathInHierarchy;
        
        public MonoSingletonPath(string pathInHierarchy)
        {
            mPathInHierarchy = pathInHierarchy;
        }
        public string PathInHierarchy
        {
            get { return mPathInHierarchy; }
        }
   
    }
}


