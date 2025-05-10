using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AppData数据分析
/// </summary>
public class AppDataAnalysis : MonoBehaviour
{
    [Header("压力水平")]
    public Text isStress;
    public Text stressState;
    public Text stressDescription;

    [Header("心率变异性")]
    public Text hrvState;
    public Text hrvDescription;

    [Header("注意力相关")]
    public Text attentionState;
    public Text attentionDescription;

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
        // 压力水平分析
        if (isStress != null || stressState != null || stressDescription != null)
        {
            AnalyzeStress(data);
        }

        // 心率变异性分析
        if (hrvState != null || hrvDescription != null)
        {
            AnalyzeHRV(data);
        }

        // 注意力分析
        if (attentionState != null || attentionDescription != null)
        {
            AnalyzeAttention(data);
        }
    }

    private void AnalyzeStress(AppData data)
    {
        // 计算平均心率与心率基线的比值
        float ratio = (float)data.avgHeartRate / data.heartBaseline;

        // 判断是否存在压力
        if (isStress != null)
        {
            isStress.text = ratio > 1.2f ? "是" : "否";
        }

        // 判断压力状态
        if (stressState != null)
        {
            if (ratio < 1.2f)
            {
                stressState.text = "正常状态";
            }
            else if (ratio < 1.4f)
            {
                stressState.text = "较小压力";
            }
            else
            {
                stressState.text = "较大压力";
            }
        }

        // 压力描述暂不填写
        if (stressDescription != null)
        {
            stressDescription.text = "";
        }
    }

    private void AnalyzeHRV(AppData data)
    {
        string state;
        string description;

        // 根据RMSSD和PNN50的值判断状态
        if (data.rmssd > 50f)
        {
            if (data.pnn50 > 0.25f)
            {
                state = "最佳状态";
                description = "副交感神经占主导，个体放松且适应能力强";
            }
            else if (data.pnn50 < 0.1f)
            {
                state = "轻度压力状态";
                description = "虽然 HRV 较高，但心跳调整稳定性较低，节奏不稳定";
            }
            else
            {
                state = "正常状态";
                description = "HRV 和节奏稳定性处于正常范围";
            }
        }
        else if (data.rmssd < 30f)
        {
            if (data.pnn50 > 0.25f)
            {
                state = "适应性较好但有压力";
                description = "HRV 整体较低，但节奏稳定";
            }
            else if (data.pnn50 < 0.1f)
            {
                state = "高压状态";
                description = "交感神经占主导，压力过大或焦虑";
            }
            else
            {
                state = "压力状态";
                description = "HRV 和节奏稳定性都较低";
            }
        }
        else
        {
            state = "正常状态";
            description = "HRV 和节奏稳定性处于正常范围";
        }

        if (hrvState != null)
        {
            hrvState.text = state;
        }

        if (hrvDescription != null)
        {
            hrvDescription.text = description;
        }
    }

    private void AnalyzeAttention(AppData data)
    {
        // 根据注意力值判断状态
        if (attentionState != null)
        {
            attentionState.text = data.attention > 50f ? "集中" : "不集中";
        }

        // 注意力描述暂不填写
        if (attentionDescription != null)
        {
            attentionDescription.text = "";
        }
    }
}
