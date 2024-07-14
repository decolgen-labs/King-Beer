using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Game
{
    public static class JsSocketConnect
    {
        [DllImport ("__Internal")]
        public static extern void SocketIOInit();

        [DllImport ("__Internal")]
        public static extern void EmitEvent(string eventName, string dataArray = null);
        [DllImport ("__Internal")]
        public static extern void RegisterDebug(string callbackObjectName, string callbackMethodName);
        [DllImport ("__Internal")]
        public static extern void RegisterUpdateCoin(string callbackObjectName, string callbackMethodName);
        [DllImport ("__Internal")]
        public static extern void RegisterSpawnCoin(string callbackObjectName, string callbackMethodName);
        [DllImport ("__Internal")]
        public static extern void RegisterUpdateProof(string callbackObjectName, string callbackMethodName);
    }
}
