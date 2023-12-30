import React, { useState, ChangeEvent, KeyboardEvent } from 'react';
import * as signalR from '@microsoft/signalr';
import { Message, UserChannel } from '../../shared/types';
import useSignalREffects from './useSignalREffects';
import { useGetMessages, useGetUserChannels, useSignalRReceiveMessage, useSignalRSelectUser, useSignalRUserChannelSelected } from './useChat';
import api from '../../utils/api';

interface ChatProps { }

const Chat: React.FC<ChatProps> = () => {
  const [userChannels, setUserChannels] = useState<UserChannel[]>([]);
  const [selectedUserChannel, setSelectedUserChannel] = useState<UserChannel | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState<string>('');
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

  useSignalREffects({ setConnection });
  useSignalRReceiveMessage({ connection, setMessages })
  useSignalRSelectUser({ connection, selectedUserChannel })
  useSignalRUserChannelSelected({ connection, userChannels, setSelectedUserChannel })
  useGetUserChannels({ setUserChannels, setSelectedUserChannel  })
  useGetMessages({ setMessages, selectedUserChannel })

  const sendMessage = async (): Promise<void> => {
    if (newMessage.trim() === '' || !selectedUserChannel || !connection) return;

    const newMessageObj: Message = {
      user_id: selectedUserChannel._id,
      text: newMessage,
      timestamp: Date.now(),
    };

    setNewMessage('');
    api.sendMessage(() => { }, (error) => { console.error('Error sending message:', error); }, newMessageObj)
  };

  const selectUserChannel = (userChannel: UserChannel): void => {
    // Update the URL with the user_id
    const url = new URL(window.location.href);
    url.searchParams.set('user_id', userChannel._id);
    window.history.pushState({}, '', url.toString());

    setSelectedUserChannel(userChannel);
  };

  const filteredMessages = selectedUserChannel ? messages.filter((message) => message.user_id === selectedUserChannel._id) : [];

  const handleKeyPress = (e: KeyboardEvent<HTMLInputElement>): void => {
    if (e.key === 'Enter') {
      sendMessage();
    }
  };

  return (
    <div className="min-h-screen flex">
      <div className="h-screen flex w-1/4 flex-col p-4 border-r">
        <div>
          <h2 className="text-xl font-semibold mb-4">Users</h2>
          <p className="mb-2">
            {selectedUserChannel ? `Selected: ${selectedUserChannel.from.name}` : 'No user selected'}
          </p>
        </div>
        <div className="overflow-y-scroll">
          <ul>
            {userChannels.map((userChannel) => (
              <li
                key={userChannel._id}
                className={`cursor-pointer mb-2 p-2 rounded ${selectedUserChannel?._id === userChannel._id ? 'bg-blue-200' : 'bg-gray-200'
                  }`}
                onClick={() => selectUserChannel(userChannel)}
              >
                {userChannel.from.name}
              </li>
            ))}
          </ul>
        </div>
      </div>
      <div className="flex-1 p-4" style={{ maxHeight: '100vh', overflowY: 'scroll', display: 'flex', flexDirection: 'column' }}>
        {selectedUserChannel && (
          <div className="border-b">
            <div className="w-12 h-12 rounded-full mb-2 overflow-hidden">
              {/* Use the Image component from next/image */}
              {selectedUserChannel.from.name ? (
                <img
                  src={selectedUserChannel.from.name}
                  alt={selectedUserChannel.from.name}
                  className="w-12 h-12 rounded-full mb-2"
                  width={100}
                  height={100}
                  onError={(e: React.SyntheticEvent<HTMLImageElement, Event>) => {
                    // If the image fails to load, display the fallback avatar
                    const target = e.target as HTMLImageElement;
                    target.onerror = null; // Reset the event to prevent infinite loop
                    // Optionally, you can set a fallback image URL
                    target.src = ''; // or provide a fallback image URL
                  }}
                />
              ) : (
                <div className="w-12 h-12 flex items-center justify-center bg-gray-300 rounded-full mb-2">
                  <p className="text-xl font-semibold text-gray-600">
                    {selectedUserChannel.from.name.charAt(0).toUpperCase()}
                  </p>
                </div>
              )}
            </div>
            <p className="text-xl font-semibold">{selectedUserChannel.from.name}</p>
          </div>
        )}
        <div className="flex-1 overflow-y-scroll">
          {filteredMessages.map((message, index) => (
            <div key={index} className="mb-2 p-2 rounded bg-gray-200">
              <p className="text-gray-800">{message.text}</p>
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
            onKeyPress={handleKeyPress}
          />
          <button className="bg-blue-500 text-white px-4 py-2 rounded" onClick={sendMessage}>
            Send
          </button>
        </div>
      </div>
    </div>
  );
};

export default Chat;