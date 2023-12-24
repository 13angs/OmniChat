import * as signalR from '@microsoft/signalr';

const NODE_ENV = process.env.NODE_ENV; 
const SIGNALR_URL = NODE_ENV === 'development' ? 'http://localhost:5078' : ''

function createSignlR(url: string) {
    return new signalR.HubConnectionBuilder()
        .withUrl(url, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
        })
        .build()
}

const websocket = {
    chatWebsocket: createSignlR(`${SIGNALR_URL}/hub/chat`)
}

export default websocket;