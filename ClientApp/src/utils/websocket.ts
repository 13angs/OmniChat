import * as signalR from '@microsoft/signalr';
import Cookies from 'js-cookie';
import { contants } from '../shared/contants';

const NODE_ENV = process.env.NODE_ENV;
const SIGNALR_URL = NODE_ENV === 'development' ? 'http://localhost:5078' : ''

function createSignalR(url: string) {
    return new signalR.HubConnectionBuilder()
        .withUrl(url, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
            accessTokenFactory: async () => {
                const token = Cookies.get(contants.TOKEN_COOKIE_KEY);
                // Handle the case where the token is undefined
                return token ? token.toString() : '';
            }
        })
        .withAutomaticReconnect()
        .build();
}


const websocket = {
    chatWebsocket: createSignalR(`${SIGNALR_URL}/hub/chat`)
}

export default websocket;