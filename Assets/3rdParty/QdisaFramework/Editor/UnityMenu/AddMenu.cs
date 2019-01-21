/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 *
 ****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AddMenu
{
    private static int cuteFlag;
    [MenuItem("Qdisa工具/全屏置顶",false,1)]
    private static void AddWindowManage()
    {
        try
        {
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            if (cameras.Length!=0)
            {
                    for(int i=0;i< cameras.Length; i++)
                    {
                    WindowManager windowManager = cameras[i].gameObject.GetComponent<WindowManager>();
                if (windowManager != null)
                {
                    windowManager.ISNoBorderAndWindow = false;    
                    break;
                }
                if (i == cameras.Length-1)
                {
                    cameras[i].gameObject.AddComponent<WindowManager>();
                }
                    }

            }
            else
            {
                Debug.LogError("添加全屏组件时出错，请查看是否存在MainCamera.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("添加全屏组件时出错，请查看是否存在MainCamera。错误信息为：" + e);
        }

    }
    [MenuItem("Qdisa工具/窗口无边框", false, 11)]

    private static void AddWindowManageNOBorder()
    {
        try
        {
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            if (cameras.Length != 0)
            {
                for (int i = 0; i < cameras.Length; i++)
                {
                    WindowManager windowGet = cameras[i].gameObject.GetComponent<WindowManager>();
                    if (windowGet != null)
                    {
                        windowGet.ISNoBorderAndWindow = true;
                        break;
                    }
                    if (i == cameras.Length - 1)
                    {
                        WindowManager windowAdd=cameras[i].gameObject.AddComponent<WindowManager>();
                        windowAdd.ISNoBorderAndWindow = true;
                    }
                }

            }
            else
            {
                Debug.LogError("添加全屏组件时出错，请查看是否存在MainCamera.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("添加全屏组件时出错，请查看是否存在MainCamera。错误信息为：" + e);
        }
    }
    [MenuItem("Qdisa工具/未完待续",false,22)]
    private static void Continue()
    {
        switch (cuteFlag)
        {
            case 0:
                Debug.Log("啦啦啦啦啦啦,你点我干什么？(*^__^*)");
                break;
            case 1:
                Debug.Log("你又戳我，快说什么事，没什么事我走了，再见!");
                break;
            case 2:
                Debug.Log("第三次了,你有完没完了，我不理你了!");
                break;
            case 3:break;
            case 4: break;
            case 5: break;
            case 6: break;
        }
        cuteFlag++;
        if (cuteFlag == 7)
        {
            cuteFlag = 0;
        }
    }
}


