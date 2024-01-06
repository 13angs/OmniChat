import React, { useState, ChangeEvent, KeyboardEvent, useMemo } from 'react';
import { Message, MessageRequest, MessageResponse, OkResponse, User, UserChannelRequest, UserRequest } from '../../shared/types';
import { useGetMessages, useGetUserProfile } from './useChat';
import UserList from './userList';
import { useMainContainerContext } from '../../containers/main/mainContainer';
import Avatar from '../../components/avatar/avatar';
import api from '../../utils/api';
import { MessageTypeService } from '../../utils/service';

interface UserChatProps {
  // userFriendId: string | null;
  userChannelRequest: UserChannelRequest | null
}

const UserChat: React.FC<UserChatProps> = ({ userChannelRequest }) => {
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
        user_id: userChannelRequest?.to?.user_id ?? ""
      }
    };
  }, [myProfile.provider_id, myProfile._id, userChannelRequest?.to?.user_id]);

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
        ref_id: myProfile._id,
        name: myProfile.name,
        avatar: myProfile.avatar ?? ""
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
  const [userChannelRequest, setUserChannelRequest] = useState<UserChannelRequest | null>(null);
  return (
    <div className="min-h-screen flex">
      <UserList setParam={setUserChannelRequest} />
      {userChannelRequest?.to?.user_id && <UserChat userChannelRequest={userChannelRequest} />}
    </div>
  );
};

export default ChatPage;
