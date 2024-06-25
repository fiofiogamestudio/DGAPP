using UnityEngine;
using System;

[System.Serializable]
public class AppData
{
    public int heartRate;
    public int avgHeartRate;
    public int breath;
    public int RSA;
    public float attention;

    // 序列化方法，返回JSON字符串
    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }
}
