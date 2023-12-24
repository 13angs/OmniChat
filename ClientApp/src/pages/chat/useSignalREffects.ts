// useSignalREffects.ts
import { useEffect } from 'react';
import * as signalR from '@microsoft/signalr';
// import { HttpTransportType } from '@microsoft/signalr';
import websocket from '../../utils/websocket';

interface SignalREffectsProps {
  setConnection: React.Dispatch<React.SetStateAction<signalR.HubConnection | null>>;
}

const useSignalREffects = ({ setConnection }: SignalREffectsProps): void => {
  useEffect(() => {
    // Initialize SignalR connection
    const newConnection = websocket.chatWebsocket;

    setConnection(newConnection);

    return () => {
      // Cleanup for the SignalR connection when component unmounts
      newConnection.stop();
    };
    // eslint-disable-next-line
  }, []);
};

export default useSignalREffects;
