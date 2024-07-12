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
    }
}
