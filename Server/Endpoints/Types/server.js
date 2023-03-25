function string2Bin(str) {
    var result = [];
    for (var i = 0; i < str.length; i++) {
        result.push(str.charCodeAt(i));
    }
    return result;
}

class Server{
    constructor(serverName, serverId, hostName, sockserver){
        this.serverName = serverName
        this.serverId = serverId
        this.hostName = hostName
        this.entities = new Map()
        this.connectedClients = new Map()
        this.connectedSockets = new Map()
        this.sockserver = sockserver

        setInterval(() =>{
            for (const [key,value] of this.connectedClients){
                //In here check if client has timed out if so delete connection
            }
        }, 5000)
    }

    RefreshClient(clientId){
        this.connectedClients.set(clientId,Date.now())
    }

    SendEntitiesToClients(){
        for (const [clientId,value] of this.connectedClients){
            const ws = this.connectedSockets.get(clientId)
            ws.send(string2Bin(JSON.stringify({
                action: "UpdateEntities",
                entityStates: Array.from(this.entities.values())
            })))
            console.log(`EntityStates: ${JSON.stringify(Array.from(this.entities))}`)

            this.entities.forEach((value,key) => {
                if (!value.isAlive){
                    this.entities.delete(key)
                }
            })
            //Send entities to client
        }
    }
}
module.exports = {Server}