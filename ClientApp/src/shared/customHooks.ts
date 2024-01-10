import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
import { RelatedUser, UserChannel } from './types';
import { HubConnection } from '@microsoft/signalr';

export interface CookieOptions extends Cookies.CookieAttributes {
  key: string;
}

export const useCookie = (options: CookieOptions) => {
  const [cookieValue, setCookieValue] = useState<string | undefined>(() => Cookies.get(options.key));

  useEffect(() => {
    const fetchCookie = () => {
      const value = Cookies.get(options.key);
      setCookieValue(value);
    };

    fetchCookie();
  }, [options.key]);

  const setCookie = (value: string, cookieOptions?: Cookies.CookieAttributes) => {
    Cookies.set(options.key, value, { ...options, ...cookieOptions });
    setCookieValue(value);
  };

  const removeCookie = (cookieOptions?: Cookies.CookieAttributes) => {
    Cookies.remove(options.key, { ...options, ...cookieOptions });
    setCookieValue(undefined);
  };

  return { cookieValue, setCookie, removeCookie };
};

export const useUserChannel = () => {
  const findFriend = (userChannel?: UserChannel, userId?: string): RelatedUser | undefined => {
    return userChannel?.related_users?.find(item => item.user_id !== userId);
  }
  return {
    findFriend
  }
}

export interface InitSignalRProps {
  setConnection: React.Dispatch<React.SetStateAction<signalR.HubConnection | null>>;
  hubConnection: HubConnection;
  
}

export const useInitSignalR = ({ setConnection, hubConnection }: InitSignalRProps): void => {
  useEffect(() => {
    // Initialize SignalR connection
    const newConnection = hubConnection;

    setConnection(newConnection);

    return () => {
      // Cleanup for the SignalR connection when component unmounts
      newConnection.stop();
    };
    // eslint-disable-next-line
  }, []);
};