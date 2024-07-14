mergeInto(LibraryManager.library, {
    SocketIOInit:function()
    {
        if(typeof io == 'undefined')
        {
            console.error('Socket.IO client library is not load');
            return;
        }

        var socket = io('http://localhost:5005');

        socket.on('connect', () => {
            socket.isReady = true;
            console.log('Socket.IO connected');
        });

        socket.on('updateBrushPosition', () => {
            if(this.debugObjectStr && this.debugObjectStr)
                SendMessage(this.debugObjectStr, this.debugMethodStr);
        });

        socket.on('updateCoin', (coin) => {
            if(this.updateCoinObjectStr && this.updateCoinMethodStr)
                SendMessage(this.updateCoinObjectStr, this.updateCoinMethodStr, coin);
        });

        socket.on('spawnCoin', () => {
            if(this.spawnCoinObjectStr && this.spawnCoinMethodStr)
                SendMessage(this.spawnCoinObjectStr, this.spawnCoinMethodStr);
        });

        socket.on('updateProof', (proof) => {
            if(this.updateProofObjectStr && this.updateProofMethodStr)
                SendMessage(this.updateProofObjectStr , this.updateProofMethodStr, proof);
        });

        window.unitySocket = socket;
    },

    RegisterDebug:function(callbackObjectName, callbackMethodName)
    {
        this.debugObjectStr = UTF8ToString(callbackObjectName);
        this.debugMethodStr = UTF8ToString(callbackMethodName);
    },
    RegisterUpdateCoin:function(callbackObjectName, callbackMethodName)
    {
        this.updateCoinObjectStr = UTF8ToString(callbackObjectName);
        this.updateCoinMethodStr = UTF8ToString(callbackMethodName);
    },
    RegisterSpawnCoin:function(callbackObjectName, callbackMethodName)
    {
        this.spawnCoinObjectStr = UTF8ToString(callbackObjectName);
        this.spawnCoinMethodStr = UTF8ToString(callbackMethodName);
    },
    RegisterUpdateProof:function(callbackObjectName, callbackMethodName)
    {
        this.updateProofObjectStr = UTF8ToString(callbackObjectName);
        this.updateProofMethodStr = UTF8ToString(callbackMethodName);
    },

    EmitEvent:function(eventName, dataArray)
    {
        let callDataArray;
        if(dataArray)
            calldataArray = JSON.parse(UTF8ToString(dataArray));
        let event = UTF8ToString(eventName);

        if(window.unitySocket && dataArray)
        {
            window.unitySocket.emit(event, calldataArray.array);
            return;
        }
        if(window.unitySocket)
            window.unitySocket.emit(event);
    },

    FreeWasmString: function (ptr) {
        _free(ptr);
    },
});