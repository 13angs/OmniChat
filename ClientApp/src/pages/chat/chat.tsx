import React, { useState, ChangeEvent, KeyboardEvent, useMemo, useEffect, createContext, useContext, ReactNode } from 'react';
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
import moment from 'moment'

interface UserChatProps {
  // userFriendId: string | null;
  userChannelRequest: UserChannelRequest | null
  messages: Message[];
  setMessages: React.Dispatch<React.SetStateAction<Message[]>>
  latestMessage: Message | null
  setLatestMessage: React.Dispatch<React.SetStateAction<Message | null>>
  newMessage: string;
  setNewMessage: React.Dispatch<React.SetStateAction<string>>
}

const UserChat: React.FC<UserChatProps> = ({ userChannelRequest, messages, setMessages, latestMessage, setLatestMessage, newMessage, setNewMessage }) => {
  const { myProfile } = useMainContainerContext();
  const [userProfile, setUserProfile] = useState<User | null>(null);

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

  // handle read message
  useEffect(() => {
    const readMessageRequest: UserChannelRequest = {
      user_channel_id: latestMessage ? latestMessage?.user_channel_id : userChannelRequest?.user_channel_id,
      to: {
        user_id: userChannelRequest?.to?.user_id
      }
    }
    api.readMessage(() => { }, (err) => { alert(err) }, readMessageRequest)
  }, [latestMessage, latestMessage?.user_channel_id, userChannelRequest?.to?.user_id, userChannelRequest?.user_channel_id])

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
      <div className="flex-1 overflow-y-scroll mt-2 flex flex-col-reverse">
        {messages.slice().reverse().map((message) => (
          <div key={message._id} className='flex items-start mb-4'>
            {/* User Avatar */}
            <Avatar
              name={message?.from?.name ?? ""}
              avatar={message?.from?.avatar ?? ""}
              displayName={false}
            />

            {/* Message Content */}
            <div className='flex flex-col ml-3'>
              {/* Timestamp */}
              <p className="text-xs text-gray-500 mb-1">
                {moment(message.created_timestamp).format("DD MMM YYYY")}
              </p>

              {/* Message Body */}
              <div className={`flex items-center p-2 rounded ${UserChannelService
                .displayUsertMessage(message, myProfile, userChannelRequest?.to, 'bg-gray-200', 'bg-blue-500 text-white')}`}>
                {message.message_object?.map((messageObject: any) => (
                  <p key={`${messageObject?.text}`} className={UserChannelService
                    .displayUsertMessage(message, myProfile, userChannelRequest?.to, "text-gray-800", 'white')}>
                    {messageObject?.text}
                  </p>
                ))}
              </div>
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

interface ChatContextProps {
  connection: signalR.HubConnection | null;
}

const ChatContext = createContext<ChatContextProps | undefined>(undefined);

export const useChatContext = (): ChatContextProps => {
  const context = useContext(ChatContext);
  if (!context) {
    throw new Error('useChatContext must be used within a ChatContext');
  }
  return context;
};

interface ChatContextProviderProps {
  children: ReactNode;
}

const ChatContextProvider: React.FC<ChatContextProviderProps> = ({ children }) => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

  useInitSignalR({ setConnection, hubConnection: websocket.chatWebsocket });

  const obj = useMemo(() => ({ connection }), [connection])
  return (
    <ChatContext.Provider value={obj}>
      {/* Your main content goes here */}
      {children}
    </ChatContext.Provider>
  );
};

const ChatPage: React.FC = () => {
  const [userChannelRequest, setUserChannelRequest] = useState<UserChannelRequest | null>(null);
  const [latestMessage, setLatestMessage] = useState<Message | null>(null);
  const [newMessage, setNewMessage] = useState<string>('');
  const [messages, setMessages] = useState<Message[]>([]);

  return (
    <ChatContextProvider>
      <div className="flex min-h-[calc(100vh_-_100px)] max-h-[calc(100vh_-_100px)]">
        <UserList setParam={setUserChannelRequest} setLatestMessage={setLatestMessage} setMessages={setMessages} />
        {userChannelRequest?.to?.user_id && (
          <UserChat
            userChannelRequest={userChannelRequest}
            latestMessage={latestMessage}
            setLatestMessage={setLatestMessage}
            newMessage={newMessage}
            setNewMessage={setNewMessage}
            messages={messages}
            setMessages={setMessages}
          />)}
      </div>
    </ChatContextProvider>
  );
};

export default ChatPage;
