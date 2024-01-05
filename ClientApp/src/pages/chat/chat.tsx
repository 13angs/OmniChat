import React, { useState, ChangeEvent, KeyboardEvent, useMemo } from 'react';
import { Message, MessageRequest, User, UserRequest } from '../../shared/types';
import { useGetMessages, useGetUserProfile } from './useChat';
import UserList from './userList';
import { useMainContainerContext } from '../../containers/main/mainContainer';
import Avatar from '../../components/avatar/avatar';

// Props for the UserChat component
interface ChatProps { }
interface UserChatProps {
  userFriendId: string | null
}

// UserChat component for rendering the chat window
const UserChat: React.FC<UserChatProps> = ({ userFriendId }) => {

  // Accessing user profile from the main container context
  const { myProfile } = useMainContainerContext();

  // State for storing chat messages, new message input, and user profile
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState<string>('');
  const [userProfile, setUserProfile] = useState<User | null>(null);

  // Memoized request object for fetching messages
  const messageRequest: MessageRequest = useMemo(() => {
    return {
      provider_id: myProfile.provider_id,
      from: {
        ref_id: myProfile._id
      },
      to: {
        user_id: userFriendId ?? ""
      }
    }
  }, [myProfile.provider_id, myProfile._id, userFriendId]);

  // Memoized request object for fetching user profile
  const userRequest: UserRequest = useMemo(() => {
    return {
      user_id: userFriendId ?? ""
    }
  }, [userFriendId]);

  // Custom hooks for fetching user profile and messages
  useGetUserProfile({ setUserProfile, userRequest });
  useGetMessages({ setMessages, messageRequest });

  // Event handler for handling Enter key press
  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>): void => {
    if (e.key === 'Enter') {
      console.log('Enter key pressed');
      // Add logic to send the message
    }
  };

  // Rendering the UserChat component
  return (
    <div className="flex-1 p-4" style={{ maxHeight: '100vh', overflowY: 'scroll', display: 'flex', flexDirection: 'column' }}>
      {/* Displaying user profile avatar */}
      {userProfile && (
        <Avatar
          name={userProfile.name ?? ""}
          avatar={userProfile.avatar ?? ""}
        />
      )}
      {/* Displaying chat messages */}
      <div className="flex-1 overflow-y-scroll">
        {messages.map((message) => (
          <>{message.message_object?.map((messageObject: any) => (
            <div key={`${messageObject?.text}`} className="mb-2 p-2 rounded bg-gray-200">
              <p className="text-gray-800">{messageObject?.text}</p>
            </div>
          ))}
          </>
        ))}
      </div>
      {/* Input field for typing new messages */}
      <div className="flex items-center">
        <input
          type="text"
          className="flex-1 border rounded p-2 mr-2"
          placeholder="Type a message..."
          value={newMessage}
          onChange={(e: ChangeEvent<HTMLInputElement>) => setNewMessage(e.target.value)}
          onKeyDown={handleKeyDown}
        />
        {/* Button to send the message */}
        <button className="bg-blue-500 text-white px-4 py-2 rounded">
          Send
        </button>
      </div>
    </div>
  )
}

// ChatPage component for rendering the entire chat page
const ChatPage: React.FC<ChatProps> = () => {
  // State for storing the selected user's ID
  const [userFriendId, setUserFriendId] = useState<string | null>(null);

  // Rendering the ChatPage component with UserList and UserChat components
  return (
    <div className="min-h-screen flex">
      <UserList setParam={setUserFriendId} />
      {userFriendId && (<UserChat userFriendId={userFriendId} />)}
    </div>
  );
};

// Exporting the ChatPage component as the default export
export default ChatPage;