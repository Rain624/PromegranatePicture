/****************************************************************************
 *
 * Copyright (c) 2018 Rain
 * 
 ****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using Qdisa;


[DisallowMultipleComponent]
public class WindowManager : MonoBehaviour, ISingleton {

    [DllImport("user32.dll")]
    static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    public static extern int FindWindow(string IpClassName, string IpWindowName);
    [DllImport("user32")]
    public static extern int GetSystemMetrics(int nIndex);
    [DllImport("user32")]
    public static extern bool ReleaseCapture();
    [DllImport("user32")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int IParam);


    public IntPtr UnityWindowHwnd;
    public bool ISTop = true;
    public bool ISNoBorderAndWindow;
    [ConditionalHide("ISNoBorderAndWindow", true)]
    public Rect screenPosition = new Rect(0, 0, 0, 0);

    private int nativeWidth;
    private int nativeHeight;

    const int SM_CXSCREEN = 0x00000000;
    const int SM_CYSCREEN = 0x00000001;

    // not used right now
    //const uint SWP_NOMOVE = 0x2;
    //const uint SWP_NOSIZE = 1;
    //const uint SWP_NOZORDER = 0x4;
    const uint SWP_HIDEWINDOW = 0x0080;
    const uint SWP_SHOWWINDOW = 0x0040;
    const int GWL_STYLE = -16;
    const int WS_BORDER = 1;
    const int WS_POPUP = 0x800000;


    public WindowManager()
    {
     }
    public static WindowManager Instance
    {
        get
        {
            return MonoSingletonProperty<WindowManager>.Instance;
        }
    }
    public void Dispose()
    {
        MonoSingletonProperty<WindowManager>.Dispose();
    }
    // Use this for initialization
    void Awake () {
#if UNITY_STANDALONE_WIN
        if (!ISNoBorderAndWindow)
        {
            FullScreen();
        }
        else
        {
            SetWindowNoBorder();
        }
        StartCoroutine("GetUnityWindow");
#endif
    }

    // Update is called once per frame
    void Update () {
#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (!ISNoBorderAndWindow)
        {
            SetWindow(UnityWindowHwnd);
        }
#endif
    }

    private void OnDestroy()
    {    
        //在前面，位于其他顶部窗口的后面
        SetWindowPos(UnityWindowHwnd, -2,
        (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width, (int)screenPosition.height,
        SWP_SHOWWINDOW);
    }
    /// <summary>
    /// 设置窗口无边框
    /// </summary>
    /// <param name="hwmd"></param>
    private void SetWindowNoBorder()
    {

        screenPosition.x = (int)((Screen.currentResolution.width - screenPosition.width) / 2);
        screenPosition.y = (int)((Screen.currentResolution.height - screenPosition.height) / 2);
        if (Screen.currentResolution.height <= 768)
        {
            screenPosition.y = 0;
        }
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);//设置无框；
        SetWindowPos(GetForegroundWindow(), 0, (int)screenPosition.x, (int)screenPosition.y,
            (int)screenPosition.width, (int)screenPosition.height, SWP_SHOWWINDOW);//exe居中显示；
    }
    /// <summary>
    /// 设置窗口为置顶
    /// </summary>
    private void  SetWindow(IntPtr hwmd)
    {
        if (ISTop)
        {
            screenPosition.width = nativeWidth;
            screenPosition.height = nativeHeight;
            //设置为无边框模式
            //SetWindowLong(hwmd, GWL_STYLE, WS_BORDER);
            SetWindowPos(hwmd, -1,
                (int)screenPosition.x, (int)screenPosition.y, (int)screenPosition.width,
                (int)screenPosition.height, SWP_SHOWWINDOW);
        }
    }
    /// <summary>
    /// 设置为全屏
    /// </summary>
    private void FullScreen()
    {
        nativeWidth = GetSystemMetrics(SM_CXSCREEN);
        nativeHeight =GetSystemMetrics(SM_CYSCREEN);

        Screen.SetResolution(nativeWidth, nativeHeight, true);
    }
    /// <summary>
    /// 获取unityexe文件的窗口
    /// </summary>
    /// <returns></returns>
    IEnumerator GetUnityWindow()
    {
        yield return new WaitForSeconds(0.1f);
        UnityWindowHwnd = GetForegroundWindow();
    }
    #region 拖动窗口
    /// <summary>
    ///拖动窗口
    /// </summary>
    /// <param name="window"></param>
    public void DragWindow(IntPtr window)
    {
#if UNITY_STANDALONE_WIN
        ReleaseCapture();
        SendMessage(window, 0xA1, 0x02, 0);
        SendMessage(window, 0x0202, 0, 0);
#endif
    }
    #endregion
}
