using UnityEngine;
using NativeWebSocket;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket ws;

    async void Start()
    {
        ws = new WebSocket("ws://localhost:8080/ws");

        ws.OnOpen += () =>
        {
            Debug.Log(" WebSocket connected");
            string json = "{\"type\":\"find_match\",\"data\":{}}";
            ws.SendText(json);
        };

        ws.OnMessage += (bytes) =>
        {
            string message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Message from server: " + message);
        };

        ws.OnError += (e) =>
        {
            Debug.Log(" WebSocket error: " + e);
        };

        ws.OnClose += (e) =>
        {
            Debug.Log("WebSocket closed");
        };

        await ws.Connect();
    }

    void Update()
    {
        ws.DispatchMessageQueue();
    }

    async void OnApplicationQuit()
    {
        await ws.Close();
    }
}
