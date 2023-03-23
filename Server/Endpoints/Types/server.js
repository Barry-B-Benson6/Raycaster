class Server{
    constructor(serverName, serverId, hostName){
        this.serverName = serverName
        this.serverId = serverId
        this.hostName = hostName
        this.entities = new Map()
        this.connectedClients = new Map()

        setInterval(() =>{
            for (const [key,value] of this.connectedClients){
                //In here check if client has timed out if so delete connection
            }
        }, 5000)
    }
}
module.exports = {Server}