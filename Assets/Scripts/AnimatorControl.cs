using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Qdisa;
using System;


public class AnimatorControl : MonoBehaviour
{
    public delegate void AniCompleteEventHandler();
    public event AniCompleteEventHandler OnAniComplete;
    public Transform PromegranateTran;
    public GameObject Butterfly;
    public GameObject OutGo;
    public GameObject InGo1;
    public GameObject InGo2;
    public GameObject InGo3;
    public GameObject InGo4;
    public GameObject HitCamGo;
    private Animation promegranateAni;
    // Start is called before the first frame update
    void Start()
    {
        promegranateAni = PromegranateTran.GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PlayAni();
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    InitPromeAni();
        //}

    }
    /// <summary>
    /// 播放动画
    /// </summary>
    public void PlayAni()
    {
        PlayPromeAni();
        ShowButterfly(false);
        OutPicture(true);
        Timer.AddTimer(5, "第二段动画开始").OnCompleted
            (() =>
            {
                InPicture(true);
                PlayHitPicture();
            }
            );
  
    }
    /// <summary>
    /// 播放冲镜动画
    /// </summary>
    private void PlayHitPicture()
    {
        if (!Timer.Exist("第三段动画开始"))
        {
            Timer.AddTimer(7, "第三段动画开始").OnCompleted
            (() =>
            {

                HitPicture(true);
                if (OnAniComplete != null)
                    OnAniComplete();
                if(!Timer.Exist("最后动画结束"))
                Timer.AddTimer(5f, "最后动画结束").OnCompleted
                (() =>
                {
                    Debug.Log("我播放完成了");
                    InitAni();
                }
                );
            }
            );
        }
    }
    /// <summary>
    /// 初始化画面
    /// </summary>
    public void InitAni()
    {
        InitPromeAni();
        InitButterfly();
    }
    
    /// <summary>
    /// 播放石榴花的动画
    /// </summary>
    private  void PlayPromeAni()
    {
        //if (!promegranateAni.IsPlaying("Shake"))
        //{
            promegranateAni["Shake"].speed = 1;
            promegranateAni.Play("Shake");
     
        //}
        
    }
    public void OnCompletePromeAni()
    {
        if (promegranateAni.IsPlaying("Shake") && promegranateAni["Shake"].normalizedTime >= 1)
        {
            //动画执行完毕
            Debug.Log("动画执行完毕");
        }
    }

    /// <summary>
    /// 初始化石榴花的状态
    /// </summary>
    private void InitPromeAni()
    {
        if (!promegranateAni.IsPlaying("Shake"))
        {
            promegranateAni.Play("Shake");
        }
        promegranateAni["Shake"].time = 0;
        promegranateAni["Shake"].speed = 0;
    }
    /// <summary>
    /// 初始化蝴蝶状态
    /// </summary>
    private void InitButterfly()
    {
        ShowButterfly(true);
        InPicture(false);
        HitPicture(false);
        OutPicture(false);
    }
   
    /// <summary>
    /// 初始的蝴蝶位置
    /// </summary>
    /// <param name="isUse">true时使用false不使用</param>
    private void ShowButterfly(bool isUse)
    {
        Butterfly.SetActive(isUse);
    }
    /// <summary>
    /// 是否进入画面
    /// </summary>
    /// <param name="isUse">true进入false不进</param>
    private void InPicture(bool  isUse)
    {
        InGo1.SetActive(isUse);
        InGo2.SetActive(isUse);
        InGo3.SetActive(isUse);
        InGo4.SetActive(isUse);
    }
    /// <summary>
    /// 是否开启退场动画
    /// </summary>
    /// <param name="isUse">true开启false关闭</param>
    private void HitPicture(bool isUse)
    {
        HitCamGo.SetActive(isUse);
        if (isUse == true)
        {
            Animation ani = HitCamGo.GetComponent<Animation>();
            foreach (AnimationState state in ani)
            {
                state.speed = 0.5F;
            }
        }

    }
    /// <summary>
    /// 是否飞出画面
    /// </summary>
    /// <param name="isUse">true飞出false不飞</param>
    private void OutPicture(bool isUse)
    {
        OutGo.SetActive(isUse);
    }
}
