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

        socket.on('updateBrushPosition', () => 
        {
            console.log("update Brush Position");
        });

        // socket.on('', () => {

        // });
        window.unitySocket = socket;
    },

    EmitEvent:function(eventName, dataArray)
    {
        const callDataArray = JSON.parse(UTF8ToString(dataArray));
        const event = UTF8ToString(eventName);

        if(window.unitySocket)
            window.unitySocket.emit(event, calldataArray);
    },

    FreeWasmString: function (ptr) {
        _free(ptr);
    },
});