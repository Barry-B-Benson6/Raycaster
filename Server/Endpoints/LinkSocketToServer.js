function bin2String(array) {
    return String.fromCharCode.apply(String, array);
}

function string2Bin(str) {
    var result = [];
    for (var i = 0; i < str.length; i++) {
        result.push(str.charCodeAt(i));
    }
    return result;
}

module.exports = {
    name: 'LinkSocketToServer',
    execute(ws,data, Servers, sockserver) {
        console.log(Servers, data.serverId)
        let server = Servers.get(data.serverId)
        if (server) {
            
            server.connectedSockets.set(data.clientId, ws)

            ws.send(string2Bin(JSON.stringify({
                action: "FinishLinking",
                success: true,
            })))
        } else {
            ws.send(string2Bin(JSON.stringify({
                action: "FinishLinking",
                success: false,
            })))
        }
    }
}