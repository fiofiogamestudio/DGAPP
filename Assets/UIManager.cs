using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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

    public Text HeartRateText;
    public Text BreathText;
    public Text AttentionText;
    public Text RSAText;

    public void RefreshAppData(AppData data)
    {
        this.HeartRateText.text = data.heartRate.ToString();
        this.BreathText.text = data.breath.ToString();
        this.AttentionText.text = data.attention.ToString();
        this.RSAText.text = data.RSA.ToString();
    }
}
