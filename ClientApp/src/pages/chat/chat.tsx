import React, { useState, ChangeEvent, KeyboardEvent, useMemo, useEffect } from 'react';
import { Message, MessageRequest, MessageResponse, OkResponse, User, UserChannelRequest, UserRequest } from '../../shared/types';
import { useGetMessages, useGetUserProfile } from './useChat';
import UserList from './userList';
import { useMainContainerContext } from '../../containers/main/mainContainer';
import Avatar from '../../components/avatar/avatar';
import api from '../../utils/api';
import { MessageTypeService, UserChannelService } from '../../utils/service';
import { useInitSignalR } from '../../shared/customHooks';
import websocket from '../../utils/websocket';
import * as signalR from '@microsoft/signalr';

interface RealtimeMessageProps {
  setMessages: React.Dispatch<React.SetStateAction<Message[]>>
  providerId?: string | null
}

const useRealtimeMessage = ({ setMessages, providerId }: RealtimeMessageProps) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

  useInitSignalR({ setConnection, hubConnection: websocket.chatWebsocket });

  useEffect(() => {

    const startConnection = async () => {
      if (!connection) return;

      try {
        // Start SignalR connection
        await connection.start();

        // Add the user to a group (you might want to customize the group name)
        await connection.invoke("AddToProvider", providerId);

        // Subscribe to ReceiveMessage event
        connection.on('ReceiveMessageFromProvider', (strMessage) => {
          // Handle incoming message
          const message: Message = JSON.parse(strMessage);
          console.log(message);
          setMessages((prevMessages) => {

            if (prevMessages.some((item) => (item.created_timestamp === message.created_timestamp) || 
                (item.user_channel_id !== message.user_channel_id))) {
              return prevMessages;
            }
            return [...prevMessages, message];
          });
        });
      } catch (err) {
        console.log(err)
      }
    }

    startConnection();

    return () => {
      // Stop SignalR connection when component unmounts
      if (connection?.state === signalR.HubConnectionState.Connected) {
        connection.stop();
      }
    };
    // eslint-disable-next-line
  }, [connection]);
}

interface UserChatProps {
  // userFriendId: string | null;
  userChannelRequest: UserChannelRequest | null
}

const UserChat: React.FC<UserChatProps> = ({ userChannelRequest }) => {
  const { myProfile } = useMainContainerContext();
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState<string>('');
  const [userProfile, setUserProfile] = useState<User | null>(null);

  useRealtimeMessage({ setMessages, providerId: myProfile?.provider_id });

  const messageRequest: MessageRequest = useMemo(() => {
    return {
      provider_id: myProfile?.provider_id,
      from: {
        ref_id: myProfile?._id
      },
      to: {
        user_id: userChannelRequest?.to?.user_id ?? ""
      }
    };
  }, [myProfile?.provider_id, myProfile?._id, userChannelRequest?.to?.user_id]);

  const userRequest: UserRequest = useMemo(() => {
    return {
      user_id: userChannelRequest?.to?.user_id ?? ""
    };
  }, [userChannelRequest?.to?.user_id]);

  useGetUserProfile({ setUserProfile, userRequest });
  useGetMessages({ setMessages, messageRequest });

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>): void => {
    if (e.key === 'Enter') {
      console.log('Enter key pressed');
      // Add logic to send the message
      handleSendMessage();
    }
  };

  const handleSuccessSendMessage = (messageResponse: OkResponse<MessageResponse>) => {
    setNewMessage('');
  }

  const handleSendMessage = () => {
    // Add logic to send the message

    const sendMessageRequest: MessageRequest = {
      channel_type: userChannelRequest?.channel_type,
      provider_id: myProfile?.provider_id,
      from: {
        ref_id: myProfile?._id,
        name: myProfile?.name,
        avatar: myProfile?.avatar ?? ""
      },
      to: {
        user_id: userProfile?._id,
        name: userProfile?.name,
        avatar: userProfile?.avatar ?? ""
      },
      message_object: MessageTypeService.getTextMessage(newMessage)
    }

    api.sendMessage(handleSuccessSendMessage, (error) => { alert(error) }, sendMessageRequest);
  };

  return (
    <div className="flex-1 p-4 flex flex-col">
      {userProfile && (
        <Avatar
          name={userProfile.name ?? ""}
          avatar={userProfile.avatar ?? ""}
        />
      )}
      <div className="border-b" />
      <div className="flex-1 overflow-y-scroll mt-2">
        {messages.map((message) => (
          <div key={message._id} className='flex items-start mb-1'>
            <Avatar
              name={message?.from?.name ?? ""}
              avatar={message?.from?.avatar ?? ""}
              displayName={false}
            />
            <div className={`flex items-center p-2 ml-2 rounded ${UserChannelService
              .displayUsertMessage(message, myProfile, userChannelRequest?.to, 'bg-gray-200', 'bg-blue-500 text-white')}`}>
              {message.message_object?.map((messageObject: any) => (
                <p key={`${messageObject?.text}`} className={UserChannelService
                  .displayUsertMessage(message, myProfile, userChannelRequest?.to, "text-gray-800", 'white')}>
                  {messageObject?.text}
                </p>
              ))}
            </div>
          </div>
        ))}

      </div>
      <div className="flex items-center">
        <input
          type="text"
          className="flex-1 border rounded p-2 mr-2"
          placeholder="Type a message..."
          value={newMessage}
          onChange={(e: ChangeEvent<HTMLInputElement>) => setNewMessage(e.target.value)}
          onKeyDown={handleKeyDown}
        />
        <button className="bg-blue-500 text-white px-4 py-2 rounded" onClick={handleSendMessage}>
          Send
        </button>
      </div>
    </div>
  );
};

const ChatPage: React.FC = () => {
  const [userChannelRequest, setUserChannelRequest] = useState<UserChannelRequest | null>(null);
  return (
    <div className="flex min-h-[calc(100vh_-_100px)] max-h-[calc(100vh_-_100px)]">
      <UserList setParam={setUserChannelRequest} />
      {userChannelRequest?.to?.user_id && <UserChat userChannelRequest={userChannelRequest} />}
    </div>
  );
};

export default ChatPage;
