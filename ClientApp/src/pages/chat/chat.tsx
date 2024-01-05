import React, { useState, ChangeEvent, KeyboardEvent, useMemo } from 'react';
import { Message, MessageRequest, User, UserRequest } from '../../shared/types';
import { useGetMessages, useGetUserProfile } from './useChat';
import UserList from './userList';
import { useMainContainerContext } from '../../containers/main/mainContainer';
import Avatar from '../../components/avatar/avatar';

interface UserChatProps {
  userFriendId: string | null;
}

const UserChat: React.FC<UserChatProps> = ({ userFriendId }) => {
  const { myProfile } = useMainContainerContext();
  const [messages, setMessages] = useState<Message[]>([]);
  const [newMessage, setNewMessage] = useState<string>('');
  const [userProfile, setUserProfile] = useState<User | null>(null);

  const messageRequest: MessageRequest = useMemo(() => {
    return {
      provider_id: myProfile.provider_id,
      from: {
        ref_id: myProfile._id
      },
      to: {
        user_id: userFriendId ?? ""
      }
    };
  }, [myProfile.provider_id, myProfile._id, userFriendId]);

  const userRequest: UserRequest = useMemo(() => {
    return {
      user_id: userFriendId ?? ""
    };
  }, [userFriendId]);

  useGetUserProfile({ setUserProfile, userRequest });
  useGetMessages({ setMessages, messageRequest });

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>): void => {
    if (e.key === 'Enter') {
      console.log('Enter key pressed');
      // Add logic to send the message
    }
  };

  const handleSendMessage = () => {
    // Add logic to send the message
  };

  return (
    <div className="flex-1 p-4 max-h-screen overflow-y-scroll flex flex-col">
      {userProfile && (
        <Avatar
          name={userProfile.name ?? ""}
          avatar={userProfile.avatar ?? ""}
        />
      )}
      <div className="flex-1 overflow-y-scroll">
        {messages.map((message) => (
          <div key={message._id} className="bg-gray-200 p-2 rounded mb-2">
            {message.message_object?.map((messageObject: any) => (
              <p key={`${messageObject?.text}`} className="text-gray-800">
                {messageObject?.text}
              </p>
            ))}
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
  const [userFriendId, setUserFriendId] = useState<string | null>(null);

  return (
    <div className="min-h-screen flex">
      <UserList setParam={setUserFriendId} />
      {userFriendId && <UserChat userFriendId={userFriendId} />}
    </div>
  );
};

export default ChatPage;
