using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AppClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private UdpClient listener;
    private StringBuilder buffer = new StringBuilder();

    public Text LogText;
    private ConcurrentQueue<string> logMessages = new ConcurrentQueue<string>();
    private void LogToScreen(string message)
    {
        Debug.Log(message);
        logMessages.Enqueue(message + "\n");
    }

    private int bcPort = 11000;

    void Awake()
    {
        Screen.SetResolution(1080, 1920, false);
    }

    void Start()
    {
        listener = new UdpClient(bcPort) { EnableBroadcast = true };
        ListenForBroadcast();
        // TryConnect("172.20.10.2");
    }

    void Update()
    {
        string message;
        while (logMessages.TryDequeue(out message))
        {
            if (LogText.text.Length > 3000) // 避免文本过长导致性能问题
            {
                LogText.text = "";
            }
            LogText.text += message; // 将消息添加到 UI 文本组件
        }

        while (dataQueue.Count > 0)
        {
            var data = dataQueue.Dequeue();
            UIManager.instance.RefreshAppData(data);
        }
    }


    private async void ListenForBroadcast()
    {
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, bcPort);

        try
        {
            LogToScreen("Waiting for broadcast");
            UdpReceiveResult result = await listener.ReceiveAsync();
            string serverIP = result.RemoteEndPoint.Address.ToString();
            LogToScreen($"Received broadcast from {serverIP} : {Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length)}");

            // 尝试与服务器建立TCP连接
            if ((client == null || !client.Connected) && !string.IsNullOrEmpty(serverIP))
            {
                TryConnect(serverIP);
                listener.Close(); // 关闭UDP监听，避免进一步接收
            }
        }
        catch (Exception e)
        {
            LogToScreen(e.ToString());
        }
        finally
        {
            if (listener != null)
            {
                listener.Close();
            }
        }
    }


    private void TryConnect(string serverIP)
    {
        try
        {
            client = new TcpClient(serverIP, 8080);  
            stream = client.GetStream();
            LogToScreen("Connected to the server at " + serverIP);
            BeginRead();
        }
        catch (Exception e)
        {
            LogToScreen("Error in connecting to server at " + serverIP + ": " + e.Message);
        }
    }

    private void BeginRead()
    {
        byte[] buffer = new byte[1024];
        try
        {
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReadCallback), buffer);
        }
        catch (Exception e)
        {
            LogToScreen("Read error: " + e.Message);
        }
    }

    private void ReadCallback(IAsyncResult ar)
    {
        int bytesRead = stream.EndRead(ar);
        if (bytesRead > 0)
        {
            byte[] buffer = (byte[])ar.AsyncState;
            string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            DeserializeAndProcessData(receivedData);
            BeginRead(); // Continue reading data
        }
    }

    private void DeserializeAndProcessData(string jsonData)
    {
        buffer.Append(jsonData);

        while (true)
        {
            var bufferString = buffer.ToString();
            var endIndex = bufferString.IndexOf("}{") + 1;
            if (endIndex > 0)
            {
                var singleJson = bufferString.Substring(0, endIndex);
                buffer.Remove(0, endIndex);

                ProcessJson(singleJson);
            }
            else
            {
                break;
            }
        }
    }

    private Queue<AppData> dataQueue = new Queue<AppData>();

    private void ProcessJson(string json)
    {
        try
        {
            AppData data = JsonUtility.FromJson<AppData>(json);
            LogToScreen($"Heart Rate: {data.heartRate}, Avg Heart Rate: {data.avgHeartRate}, Breath: {data.breath}, RSA: {data.RSA}, Attention: {data.attention}");
            dataQueue.Enqueue(data);
        }
        catch (Exception e)
        {
            LogToScreen("Error in JSON Deserialization: " + e.Message);
            LogToScreen("JSON Received: " + json);
        }
    }

    void OnApplicationQuit()
    {
        if (stream != null)
        {
            stream.Close();
        }
        if (client != null)
        {
            client.Close();
        }
        if (listener != null)
        {
            listener.Close();
        }
    }
}
