using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AppData数据显示器
/// </summary>
public class AppDataDisplayer : MonoBehaviour
{
    [Header("心率相关")]
    public Text heartRateText;
    public Text avgHeartRateText;
    public Text heartBaselineText;

    [Header("呼吸相关")]
    public Text breathText;
    public Text breathIntervalText;

    [Header("心率变异性相关")]
    public Text rsaText;
    public Text ibiText;
    public Text rmssdText;
    public Text pnn50Text;

    [Header("注意力相关")]
    public Text attentionText;

    private void OnEnable()
    {
        UIManager.OnAppDataUpdate += OnAppDataUpdate;
    }

    private void OnDisable()
    {
        UIManager.OnAppDataUpdate -= OnAppDataUpdate;
    }

    private void OnAppDataUpdate(AppData data)
    {
        // 心率相关数据显示
        if (heartRateText != null)
            heartRateText.text = data.heartRate.ToString() + "bpm";
        if (avgHeartRateText != null)
            avgHeartRateText.text = data.avgHeartRate.ToString() + "bpm";
        if (heartBaselineText != null)
            heartBaselineText.text = data.heartBaseline.ToString() + "bpm";

        // 呼吸相关数据显示
        if (breathText != null)
            breathText.text = data.breath.ToString();
        if (breathIntervalText != null)
            breathIntervalText.text = data.breathInterval.ToString("F1");

        // 心率变异性相关数据显示
        if (rsaText != null)
            rsaText.text = data.RSA.ToString();
        if (ibiText != null)
            ibiText.text = data.ibi.ToString("F2");
        if (rmssdText != null)
            rmssdText.text = data.rmssd.ToString("F1") + "ms";
        if (pnn50Text != null)
            pnn50Text.text = (data.pnn50 * 100).ToString("F1") + "%";

        // 注意力数据显示
        if (attentionText != null)
            attentionText.text = data.attention.ToString("F1");
    }
}
