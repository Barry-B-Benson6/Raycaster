const {Server} = require('./Types/server.js')

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
    name: 'CreateGame',
    execute(ws,data, Servers, sockserver) {
        let server = new Server(data.serverName, data.serverId, data.hostName, sockserver)
        server.connectedClients.set( data.clientId, Date.now())
        Servers.set(data.serverId, server)
        console.log(Servers)
        ws.send(string2Bin(JSON.stringify({
            success: true,
            serverId: data.serverId
        })))
    }
}