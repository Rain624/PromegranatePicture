using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using Qdisa;
using System;

public class PostControl : MonoBehaviour
{
    public PostProcessProfile postProcessProfile;
    /// <summary>
    /// 闪光强度类型值
    /// </summary>
    public FloatParameter bloomIntensity;
    /// <summary>
    /// PostIntensity目标值
    /// </summary>
    [SerializeField]
    private float intensityEndValue;
    /// <summary>
    /// PostIntensity初始值
    /// </summary>
    [SerializeField]
    private float intensityStartValue;
    /// <summary>
    /// 闪光时间
    /// </summary>
    [SerializeField]
    private float bloomTime;

    private void Update()
    {
        this.gameObject.SetActive(true);
    }
    /// <summary>
    /// 闪光
    /// </summary>
    public  void BloomScreen()
    {
        Bloom bloom = postProcessProfile.GetSetting<Bloom>();
        if (!Timer.Exist("Bloom"))
        {
            Timer.AddTimer(bloomTime, "Bloom")
                       .OnUpdated((v) =>
                       {
                         bloomIntensity.Interp(intensityStartValue, intensityEndValue, v);
                         bloom.intensity.value = bloomIntensity.value;
                       }
                       )
                       .OnCompleted(
                       () =>
                       {
                        if (!Timer.Exist("Reset"))
                        {
                        Timer.AddTimer(bloomTime, "Reset")
                       .OnUpdated((f) =>
                       {
                          bloomIntensity.Interp(intensityStartValue, intensityEndValue, (1 - f));
                          bloom.intensity.value = bloomIntensity.value;
                       }
                       );
                        }
        }
        );
        }



    }
}
