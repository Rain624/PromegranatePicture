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
    public class MonoSingletonProperty<T> where T : MonoBehaviour, ISingleton
    {
        private static T mInstance = null;
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = MonoSingletonCreator.CreatMonoSingleton<T>();
                }
                return mInstance;
            }
        }
        public static void Dispose()
        {
            Object.DestroyImmediate(mInstance.gameObject);
            mInstance = null;
        }
    
    }
}

