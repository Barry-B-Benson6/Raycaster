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
    name: 'JoinGame',
    execute(ws,data, Servers) {
        console.log(Servers)
        let server = Servers.get(data.serverId)
        if (server) {
            server.connectedClients.set(data.clientId, Date.now())
            console.log(true)
            ws.send(string2Bin(JSON.stringify({
                success: true,
            })))
        } else {
            console.log(false)
            ws.send(string2Bin(JSON.stringify({
                success: false,
            })))
        }
    }
}