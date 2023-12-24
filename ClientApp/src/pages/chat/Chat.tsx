import React, { useState, ChangeEvent, KeyboardEvent } from 'react';
import * as signalR from '@microsoft/signalr';
import { User, Message } from '../../shared/types';
import useSignalREffects from './useSignalREffects';
import { useGetMessages, useGetUsers, useSignalRReceiveMessage, useSignalRSelectUser, useSignalRUserSelected } from './useChat';
import api from '../../utils/api';

interface ChatProps { }

const Chat: React.FC<ChatProps> = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [selectedUser, setSelectedUser] = useState<User | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState<string>('');
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);

  useSignalREffects({ setConnection });
  useSignalRReceiveMessage({ connection, setMessages })
  useSignalRSelectUser({ connection, selectedUser })
  useSignalRUserSelected({ connection, users, setSelectedUser })
  useGetUsers({ setUsers, setSelectedUser })
  useGetMessages({ setMessages, selectedUser })

  const sendMessage = async (): Promise<void> => {
    if (newMessage.trim() === '' || !selectedUser || !connection) return;

    const newMessageObj: Message = {
      user_id: selectedUser._id,
      text: newMessage,
      timestamp: Date.now(),
    };

    setNewMessage('');
    api.sendMessage(() => { }, (error) => { console.error('Error sending message:', error); }, newMessageObj)
  };

  const selectUser = (user: User): void => {
    // Update the URL with the user_id
    const url = new URL(window.location.href);
    url.searchParams.set('user_id', user._id);
    window.history.pushState({}, '', url.toString());

    setSelectedUser(user);
  };

  const filteredMessages = selectedUser ? messages.filter((message) => message.user_id === selectedUser._id) : [];

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
            {selectedUser ? `Selected: ${selectedUser.name}` : 'No user selected'}
          </p>
        </div>
        <div className="overflow-y-scroll">
          <ul>
            {users.map((user) => (
              <li
                key={user._id}
                className={`cursor-pointer mb-2 p-2 rounded ${selectedUser?._id === user._id ? 'bg-blue-200' : 'bg-gray-200'
                  }`}
                onClick={() => selectUser(user)}
              >
                {user.name}
              </li>
            ))}
          </ul>
        </div>
      </div>
      <div className="flex-1 p-4" style={{ maxHeight: '100vh', overflowY: 'scroll', display: 'flex', flexDirection: 'column' }}>
        {selectedUser && (
          <div className="border-b">
            <div className="w-12 h-12 rounded-full mb-2 overflow-hidden">
              {/* Use the Image component from next/image */}
              {selectedUser.avatar ? (
                <img
                  src={selectedUser.avatar}
                  alt={selectedUser.name}
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
                    {selectedUser.name.charAt(0).toUpperCase()}
                  </p>
                </div>
              )}
            </div>
            <p className="text-xl font-semibold">{selectedUser.name}</p>
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