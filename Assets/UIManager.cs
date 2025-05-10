using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 更新图表
    public static event Action<AppData> OnAppDataUpdate;

    public static UIManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        // Debug
        // IPText.text = client.;
    }

    // 基准值
    private int heartRateBaseline = 75;
    private int breathBaseline = 15;
    private float attentionBaseline = 30f;
    private int RSABaseline = 50;

    // 波动范围
    private float heartRateVariation = 10f;
    private float breathVariation = 3f;
    private float attentionVariation = 20f;
    private float RSAVariation = 15f;

    float targetTime = 1f;
    float timer = 0;
    // private void Update()
    // {
    //     timer += Time.deltaTime;
    //     if (timer > targetTime)
    //     {
    //         timer = 0f;
    //         var data = new AppData();

    //         // 生成心率数据（60-100之间波动）
    //         data.heartRate = Mathf.RoundToInt(heartRateBaseline + UnityEngine.Random.Range(-heartRateVariation, heartRateVariation));
    //         data.heartRate = Mathf.Clamp(data.heartRate, 60, 100);

    //         // 生成呼吸数据（12-20次/分钟之间波动）
    //         data.breath = Mathf.RoundToInt(breathBaseline + UnityEngine.Random.Range(-breathVariation, breathVariation));
    //         data.breath = Mathf.Clamp(data.breath, 12, 20);

    //         // 生成注意力数据
    //         data.attention = attentionBaseline + UnityEngine.Random.Range(-attentionVariation, attentionVariation);

    //         // 生成RSA数据（30-70之间波动）
    //         data.RSA = Mathf.RoundToInt(RSABaseline + UnityEngine.Random.Range(-RSAVariation, RSAVariation));
    //         data.RSA = Mathf.Clamp(data.RSA, 30, 70);

    //         // 生成其他相关数据
    //         data.heartBaseline = heartRateBaseline;
    //         data.ibi = 60000f / data.heartRate; // 计算IBI（毫秒）
    //         data.rmssd = data.RSA * 0.8f; // RMSSD与RSA相关
    //         data.pnn50 = data.RSA * 0.6f * 0.01f; // pNN50与RSA相关
    //         data.breathInterval = 60f / data.breath; // 计算呼吸间隔（秒）
    //         data.avgHeartRate = (data.heartRate + heartRateBaseline) / 2; // 计算平均心率

    //         RefreshAppData(data);
    //     }
    // }

    public Text HeartRateText;
    public Text BreathText;
    public Text AttentionText;
    public Text RSAText;

    public void RefreshAppData(AppData data)
    {
        this.HeartRateText.text = data.heartRate.ToString();
        this.BreathText.text = data.breath.ToString();
        this.AttentionText.text = (data.attention * 100).ToString("F1") + "%";
        this.RSAText.text = data.RSA.ToString();

        OnAppDataUpdate?.Invoke(data);
    }
}
