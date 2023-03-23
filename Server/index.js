const express = require('express');
const bodyParser = require('body-parser');
const app = express();
app.use(bodyParser.json({limit:'10mb'}));
const port = 3000;

let Servers = new Map()

app.listen(port, () => console.log(`Server is active on port ${port}!`));

app.post('/createGame', (req, res) => {
    let server = {
        serverName: req.body.serverName,
        serverId: req.body.serverId,
        hostName: req.body.hostName,
    }
    Servers.set(req.body.serverId, server)
    console.log(Servers)
    res.send({
        success: true,
        serverId: req.body.serverId
    })
})
app.post('/servers', (req, res) => {
    let listOfServers = []
    for (const [key, value] of Servers) {
        listOfServers.push(value)
    }
    console.log(listOfServers)
    res.send(listOfServers)
})
