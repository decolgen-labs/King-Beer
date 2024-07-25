using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    public class SocketConnectManager : MonoBehaviour
    {
        public static SocketConnectManager Instance { get; private set; }

        public SocketIOUnity socket;
        Dictionary<string, Action<string>> _actionEventDic = new Dictionary<string, Action<string>>();

        void Awake()
        {
            Instance = this;

            var uri = new Uri("http://localhost:5006");
            socket = new SocketIOUnity(uri, new SocketIOOptions
            {
                Query = new Dictionary<string, string>
                    {
                        {"token", "UNITY" }
                    }
                ,
                EIO = 4
                ,
                Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
            });
            socket.JsonSerializer = new NewtonsoftJsonSerializer();

            socket.OnConnected += (sender, e) =>
            {
                Debug.Log("socket.OnConnected");
            };

            socket.On("updateCoin", (coin) =>
            {
                Debug.Log("updateCoin: " + coin.GetValue<string>());
                Debug.Log("updateCoinHasKey: " + _actionEventDic.ContainsKey("updateCoin"));
                if(_actionEventDic.ContainsKey("updateCoin"))
                {
                    string data = coin.GetValue<string>();
                    _actionEventDic["updateCoin"].Invoke(data);
                }
            });

            socket.On("spawnCoin", (spawn) =>
            {
                if(_actionEventDic.ContainsKey("spawnCoin"))
                {
                    _actionEventDic["spawnCoin"].Invoke(null);
                }
            });

            socket.On("updateProof", (proof) =>
            {
                if(_actionEventDic.ContainsKey("updateProof"))
                {
                    string data = proof.GetValue<string>();
                    _actionEventDic["updateProof"].Invoke(data);
                }
            });

            socket.Connect();

        }

        public void EmitEvent(string eventName, string dataArray = null)
        {
            socket.Emit(eventName, dataArray);
        }

        public void OnEvent(string eventName, Action<string> action)
        {
            Debug.Log("OnEvent: " + eventName);
            _actionEventDic.Add(eventName, action);
        }
    }
}
