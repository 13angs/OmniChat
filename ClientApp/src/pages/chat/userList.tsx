import React, { useEffect, useState } from 'react';
import { Message, RelatedUser, UserChannel, UserChannelRequest } from '../../shared/types';
import { useMainContainerContext } from '../../containers/main/mainContainer';
import { useUserChannel } from '../../shared/customHooks';
import { useGetUserChannels } from './useChat';
import { RequestParam } from '../../shared/contants';
import * as signalR from '@microsoft/signalr';
import { useChatContext } from './chat';

interface RealtimeMessageProps {
    setUserChannels: React.Dispatch<React.SetStateAction<UserChannel[]>>
    setMessages: React.Dispatch<React.SetStateAction<Message[]>>
    setLatestMessage: React.Dispatch<React.SetStateAction<Message | null>>
    
}

const useRealtimeUserChannels = ({ setUserChannels, setMessages, setLatestMessage }: RealtimeMessageProps) => {
    const { connection } = useChatContext();

    useEffect(() => {

        const startConnection = async () => {
            if (!connection) return;
            // Stop SignalR connection when component unmounts
            if (connection?.state !== signalR.HubConnectionState.Connected) {
                // Start SignalR connection
                await connection.start();
                console.log('UserChannel: Connecting...')
            }
            try {
                // Add the user to a group (you might want to customize the group name)
                await connection.invoke("AddToProvider");

                // Subscribe to ReceiveMessage event
                connection.on('ReceiveUserChannelFromProvider', (strUserChannel) => {
                    // Handle incoming message
                    const userChannel: UserChannel = JSON.parse(strUserChannel);
                    setUserChannels((prevUserChannels) => {

                        if (prevUserChannels.some((item) => (item._id === userChannel._id))) {
                            prevUserChannels = prevUserChannels.filter(i => i._id !== userChannel._id);
                            prevUserChannels.push(userChannel);
                            return prevUserChannels;
                        }
                        return prevUserChannels;
                    });
                });

                // Subscribe to ReceiveMessage event
                connection.on('ReceiveMessageFromProvider', (strMessage) => {
                    // Handle incoming message
                    const message: Message = JSON.parse(strMessage);
                    setMessages((prevMessages) => {

                        if (prevMessages.some((item) => (item.created_timestamp === message.created_timestamp) ||
                            (item.user_channel_id !== message.user_channel_id))) {
                            return prevMessages;
                        }

                        setLatestMessage(message);
                        return [...prevMessages, message];
                    });
                });
            } catch (err) {
                console.error(err)
            }
        }

        startConnection();

        return () => {
            // Stop SignalR connection when component unmounts
            if (connection?.state === signalR.HubConnectionState.Connected) {
                // Remove the user from the group
                connection.invoke("RemoveFromProvider");
                connection.stop();
            }
        };
        // eslint-disable-next-line
    }, [connection]);
}

// Component for rendering individual user buttons
interface UserButtonProps {
    userChannel: UserChannel;
    isSelected: boolean;
    onClick: () => void;
}

// UserButton component for rendering a button representing an individual user in the UserList
const UserButton: React.FC<UserButtonProps> = ({ userChannel, isSelected, onClick }) => {
    const { myProfile } = useMainContainerContext();
    const { findFriend } = useUserChannel();

    // Use custom hook to find friend based on user channel and current user ID
    const friendProps = findFriend(userChannel, myProfile?._id);

    return (
        <button
            className={`cursor-pointer mb-2 p-2 rounded ${isSelected ? 'bg-blue-200' : 'bg-gray-200'}`}
            style={{ width: '100%', textAlign: 'left' }}
            onClick={onClick}
        >
            <div className='flex justify-center items-center'>
                <p className='flex-grow'>{friendProps?.name}</p>

                {friendProps?.is_read === false && (
                    <div className='w-2 h-2 bg-green-500 rounded-xl' />
                )}
            </div>
        </button>
    );
};

// Main UserList component
interface UserListProps {
    setParam?: React.Dispatch<React.SetStateAction<UserChannelRequest | null>>;
    setMessages: React.Dispatch<React.SetStateAction<Message[]>>
    setLatestMessage: React.Dispatch<React.SetStateAction<Message | null>>
}

// UserList component for rendering the list of user channels and selecting a user channel
const UserList: React.FC<UserListProps> = ({ setParam, setMessages, setLatestMessage }) => {
    // State for managing user channels and selected user channel
    const [userChannels, setUserChannels] = useState<UserChannel[]>([]);
    const [relatedUser, setRelatedUser] = useState<RelatedUser | null>(null);

    // Context and custom hook for user-related functionality
    const { myProfile, isDrawerOpen } = useMainContainerContext();
    const { findFriend } = useUserChannel();

    // Custom hook for fetching user channels
    useGetUserChannels({ setUserChannels, queryBy: RequestParam.friend });

    useRealtimeUserChannels({ setUserChannels, setMessages, setLatestMessage });

    // Function to select a user channel and update the URL
    const selectUserChannel = (userChannel: UserChannel): void => {
        const relUser: RelatedUser | undefined = findFriend(userChannel ?? undefined, myProfile?._id);

        // Update the URL with the user_id
        const url = new URL(window.location.href);
        url.searchParams.set('to.user_id', relUser?.user_id ?? "");
        url.searchParams.set('channel_type', userChannel.channel_type ?? "");
        url.searchParams.set('user_channel_id', userChannel._id ?? "");
        window.history.pushState({}, '', url.toString());

        // Set the relatedUser state
        setRelatedUser(null);
        setRelatedUser(relUser ?? null);
    };

    // Extracting friendUserId from the URL and updating the state using setParam
    const url = new URL(window.location.href);
    const friendUserId = url.searchParams.get('to.user_id');
    const channelType = url.searchParams.get('channel_type');
    const userChannelId = url.searchParams.get('user_channel_id');

    useEffect(() => {
        if (setParam) {
            setParam((prev) => ({
                ...prev,
                user_channel_id: userChannelId?.toString(),
                to: {
                    user_id: friendUserId ?? ""
                },
                channel_type: Number(channelType)
            }));
        }
    }, [channelType, friendUserId, setParam, userChannelId]);

    // Rendering the UserList component
    return (
        <div className={`h-full flex ${isDrawerOpen ? 'w-1/4' : 'w-1/5'} flex-col border-r min-h-[calc(100vh_-_100px)] max-h-[calc(100vh_-_100px)]`}>
            <div>
                <h2 className="text-xl font-semibold mb-4">Users</h2>
                <p className="mb-2">
                    {relatedUser ? `Selected: ${relatedUser.name}` : 'No user selected'}
                </p>
            </div>
            <div className="overflow-y-scroll">
                {/* Map through user channels to render UserButton components */}
                {userChannels.map((userChannel) => (
                    <UserButton
                        key={userChannel._id}
                        userChannel={userChannel}
                        isSelected={relatedUser?.user_id === userChannel._id}
                        onClick={() => selectUserChannel(userChannel)}
                    />
                ))}
            </div>
        </div>
    );
};

// Exporting the UserList component as the default export
export default UserList;