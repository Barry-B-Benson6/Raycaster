const express = require('express');
const { Server } = require('./Endpoints/Types/server');
const bodyParser = require('body-parser');
const port = 3000;
const app = express()
const fs = require('fs')
app.use(bodyParser.json({ limit: '10mb' }));
app.listen(port, () => console.log(`Server is active on port ${port}!`));
let Servers = new Map()

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

const { WebSocketServer } = require('ws')
const sockserver = new WebSocketServer({ port: 3001 })

let Endpoints = new Map()

const EndpointFiles = fs.readdirSync('./Endpoints/').filter(file => file.endsWith(".js"))
for (const file of EndpointFiles){
    const command = require(`./Endpoints/${file}`)
    Endpoints.set(command.name, command)
}


sockserver.on('connection', ws => {
    console.log('New client connected')
    ws.on('close', () => console.log('Client Disconnected'))
    ws.on('message', data => {
        let dataObject = JSON.parse(bin2String(data))
        Endpoints.get(dataObject.endpoint).execute(ws,dataObject,Servers)
    })
    ws.onerror = function () {
        console.log('websocket error')
    }
})

app.post('/joinGame', (req, res) => {

})
