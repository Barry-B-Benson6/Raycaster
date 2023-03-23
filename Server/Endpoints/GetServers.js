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
    name: 'GetServers',
    execute(ws,data, Servers) {
        let listOfServers = []
        for (const [key, value] of Servers) {
            listOfServers.push({
                serverName: value.serverName,
                serverId: value.serverId,
                hostName: value.hostName
            })
        }
        ws.send(string2Bin(JSON.stringify(listOfServers)))
    }
}