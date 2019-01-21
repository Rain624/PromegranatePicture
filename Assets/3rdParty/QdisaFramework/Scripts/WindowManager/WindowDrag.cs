using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowDrag : MonoBehaviour,IPointerDownHandler,IPointerExitHandler,IPointerUpHandler {
    private bool isDrag = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDrag = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
    }

    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        if (isDrag == true)
        {
            Drag();
        }
    }
    public void Drag()
    {
        WindowManager.Instance.DragWindow(WindowManager.Instance.UnityWindowHwnd);
    }
}
