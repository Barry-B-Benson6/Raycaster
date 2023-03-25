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
    name: 'SendDirtyEntities',
    execute(ws,data, Servers) {
        let server = Servers.get(data.serverId)
        console.log("HI")
        server.RefreshClient(data.clientId)
        
        data.entities.forEach(entity => {
            server.entities.set(entity.entityId, entity)
            server.SendEntitiesToClients()
        })

        if (server.connectedClients.has(data.clientId)) {
        }
        console.log(data)
        ws.send(string2Bin(JSON.stringify({
            success: true,
        })))
    }
}