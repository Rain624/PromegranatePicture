/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 * 
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace Qdisa
{
    public  static class MonoSingletonCreator
    {
        public static T CreatMonoSingleton<T>()where T :MonoBehaviour
        {

            T instance = null;
            if (!Application.isPlaying) return instance;
            instance = Object.FindObjectOfType<T>();

            if (instance != null) return instance;
            MemberInfo memberInfo = typeof(T);
            var attributes = memberInfo.GetCustomAttributes(true);
            foreach(var attribute in attributes)
            {
                var defineAttri = attribute as MonoSingletonPath;
                if (defineAttri == null)
                {
                    continue;
                }
                instance = CreatComponentOnGameObject<T>(defineAttri.PathInHierarchy,true);
                break;
            }
            if (instance == null)
            {
                var obj = new GameObject(typeof(T).Name);
                Object.DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
        private static T CreatComponentOnGameObject<T>(string path,bool dontDestroy)where T : MonoBehaviour
        {
            //查找场景中是否有这个物体
            var obj = FindGameObject(path, true, dontDestroy);
            if (obj == null)
            {
                obj = new GameObject("Singleton of" + typeof(T).Name);
                if (dontDestroy)
                {
                    Object.DontDestroyOnLoad(obj);
                }
            }
            return obj.AddComponent<T>();
        }
        private static GameObject FindGameObject(string path, bool build, bool dontDestroy)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var subPath = path.Split('/');
            if (subPath == null || subPath.Length == 0)
            {
                return null;
            }

            return FindGameObject(null, subPath, 0, build, dontDestroy);
        }
        private static GameObject FindGameObject(GameObject root, string[] subPath, int index, bool build, bool dontDestroy)
        {
            GameObject client = null;

            if (root == null)
            {
                client = GameObject.Find(subPath[index]);
            }
            else
            {
                var child = root.transform.Find(subPath[index]);
                if (child != null)
                {
                    client = child.gameObject;
                }
            }

            if (client == null)
            {
                if (build)
                {
                    client = new GameObject(subPath[index]);
                    if (root != null)
                    {
                        client.transform.SetParent(root.transform);
                    }

                    if (dontDestroy && index == 0)
                    {
                        GameObject.DontDestroyOnLoad(client);
                    }
                }
            }

            if (client == null)
            {
                return null;
            }

            return ++index == subPath.Length ? client : FindGameObject(client, subPath, index, build, dontDestroy);
        }
    }
   
}

