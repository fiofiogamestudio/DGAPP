using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

/// <summary>
/// 图表数据类型
/// </summary>
public enum ChartDataType
{
    HeartRate,       // 心率
    AvgHeartRate,    // 平均心率
    HeartBaseline,   // 心率基线
    Breath,          // 呼吸
    BreathInterval,  // 呼吸间隔
    RSA,             // RSA
    IBI,             // IBI
    RMSSD,           // RMSSD
    PNN50,           // PNN50
    Attention,       // 注意力
}

/// <summary>
/// 折线图绘制器
/// </summary>
public class ChartDisplayer : MonoBehaviour
{
    [Header("图表设置")]
    public ChartDataType dataType;

    protected LineChart lineChart;
    protected List<AppData> dataQueue; //用于存储待显示数据的数据队列
    protected int maxDataCount = 10;
    protected bool cdComplete;
    protected float cdTime = 0.5f;
    protected float cdTimer;

    protected virtual void Awake()
    {
        lineChart = GetComponent<LineChart>();
        dataQueue = new List<AppData>();
        maxDataCount = lineChart.series[0].dataCount;
    }

    protected virtual void OnEnable()
    {
        UIManager.OnAppDataUpdate += OnRefreshAppData;
    }

    protected virtual void OnDisable()
    {
    }

    protected virtual void Start()
    {
        lineChart.ClearSerieData();
        cdComplete = true;
    }

    protected virtual void Update()
    {
        // 基类中的Update方法可以为空，由子类根据需要重写
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    protected virtual void OnRefreshAppData(AppData data)
    {
        // 更新队列
        dataQueue.Add(data);
        if (dataQueue.Count > maxDataCount)
        {
            dataQueue.RemoveAt(0);
        }

        // 更新图表
        lineChart.ClearSerieData();
        for (int i = 0; i < dataQueue.Count; i++)
        {
            float value = GetDataValue(dataQueue[i]);
            lineChart.AddData(0, value);
        }
    }

    /// <summary>
    /// 根据数据类型获取对应的值
    /// </summary>
    private float GetDataValue(AppData data)
    {
        switch (dataType)
        {
            case ChartDataType.HeartRate:
                return data.heartRate;
            case ChartDataType.AvgHeartRate:
                return data.avgHeartRate;
            case ChartDataType.Breath:
                return data.breath;
            case ChartDataType.RSA:
                return data.RSA;
            case ChartDataType.Attention:
                return data.attention;
            case ChartDataType.HeartBaseline:
                return data.heartBaseline;
            case ChartDataType.IBI:
                return data.ibi;
            case ChartDataType.RMSSD:
                return data.rmssd;
            case ChartDataType.PNN50:
                return data.pnn50;
            case ChartDataType.BreathInterval:
                return data.breathInterval;
            default:
                return 0f;
        }
    }
}
