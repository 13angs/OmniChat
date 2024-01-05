import React, { useState, ChangeEvent, KeyboardEvent } from 'react';
import * as signalR from '@microsoft/signalr';
import { Message, UserChannel } from '../../shared/types';
import useSignalREffects from './useSignalREffects';
import { useGetMessages, useGetUserChannels, useSignalRReceiveMessage, useSignalRSelectUser, useSignalRUserChannelSelected } from './useChat';
import UserList from './userList';
import Avatar from '../../components/avatar/avatar';

interface ChatProps { }

const ChatPage: React.FC<ChatProps> = () => {
  const [userChannels, setUserChannels] = useState<UserChannel[]>([]);
  const [selectedUserChannel, setSelectedUserChannel] = useState<UserChannel | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState<string>('');
  // const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

  // useSignalREffects({ setConnection });
  // useSignalRReceiveMessage({ connection, setMessages })
  // useSignalRSelectUser({ connection, selectedUserChannel })
  // useSignalRUserChannelSelected({ connection, userChannels, setSelectedUserChannel })
  useGetUserChannels({ setUserChannels, setSelectedUserChannel })
  useGetMessages({ setMessages, selectedUserChannel })

  // const sendMessage = async (): Promise<void> => {
  //   if (newMessage.trim() === '' || !selectedUserChannel || !connection) return;

  //   const newMessageObj: Message = {
  //     user_id: selectedUserChannel._id,
  //     text: newMessage,
  //     timestamp: Date.now(),
  //   };

  //   setNewMessage('');
  //   api.sendMessage(() => { }, (error) => { console.error('Error sending message:', error); }, newMessageObj)
  // };

  const selectUserChannel = (userChannel: UserChannel): void => {
    // Update the URL with the user_id
    const url = new URL(window.location.href);
    url.searchParams.set('from.ref_id', userChannel?.from?.ref_id ?? "");
    url.searchParams.set('to.user_id', userChannel?.to?.user_id ?? "");
    window.history.pushState({}, '', url.toString());

    setSelectedUserChannel(userChannel);
  };

  const filteredMessages = selectedUserChannel ? messages.filter((message) => message?.to?.user_id === selectedUserChannel.to.user_id) : [];

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>): void => {
    if (e.key === 'Enter') {
      // sendMessage();
    }
  };

  return (
    <div className="min-h-screen flex">
      <UserList userChannels={userChannels} selectedUserChannel={selectedUserChannel} selectUserChannel={selectUserChannel} />
      <div className="flex-1 p-4" style={{ maxHeight: '100vh', overflowY: 'scroll', display: 'flex', flexDirection: 'column' }}>
        {selectedUserChannel && (
          <Avatar
            name={selectedUserChannel.to.name ?? ""}
            avatar={selectedUserChannel.to.avatar ?? ""}
          />
        )}
        <div className="flex-1 overflow-y-scroll">
          {filteredMessages.map((message) => (
            <>{message.message_object?.map((messageObject: any) => (
              <div key={`${messageObject?.text}`} className="mb-2 p-2 rounded bg-gray-200">
                <p className="text-gray-800">{messageObject?.text}</p>
              </div>
            ))}
            </>
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
          <button className="bg-blue-500 text-white px-4 py-2 rounded"
            // onClick={sendMessage}
          >
            Send
          </button>
        </div>
      </div>
    </div>
  );
};

export default ChatPage;