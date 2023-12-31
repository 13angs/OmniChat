import React, { useEffect } from "react";
import api from "../../utils/api";
import { Message, MessageResponse, OkResponse, UserChannel, UserChannelResponse } from "../../shared/types";

interface GetUsersProps {
    setUserChannels: React.Dispatch<React.SetStateAction<UserChannel[]>>;
    setSelectedUserChannel: React.Dispatch<React.SetStateAction<UserChannel | null>>;
}

interface GetMessagesProps {
    setMessages: React.Dispatch<React.SetStateAction<Message[]>>;
    selectedUserChannel: UserChannel | null;
}

interface UserSelectedProps {
    connection: signalR.HubConnection | null;
    userChannels: UserChannel[]
    setSelectedUserChannel: React.Dispatch<React.SetStateAction<UserChannel | null>>;
}

interface SelectUserProps {
    connection: signalR.HubConnection | null;
    selectedUserChannel: UserChannel | null
}

interface ReceiveMessageProps {
    connection: signalR.HubConnection | null;
    setMessages: React.Dispatch<React.SetStateAction<Message[]>>;
}

// Custom hook for recieving message in real-time
const useSignalRReceiveMessage = ({ connection, setMessages }: ReceiveMessageProps) => {
    useEffect(() => {
        if (!connection) return;

        // Start SignalR connection
        connection.start().catch((err) => console.error('SignalR connection error:', err));

        // Subscribe to ReceiveMessage event
        connection.on('ReceiveMessage', (strMessage) => {
            console.log(strMessage);
            // Handle incoming message
            const message: Message = JSON.parse(strMessage);

            setMessages((prevMessages) => {
                if (prevMessages.some((item) => item.created_timestamp === message.created_timestamp)) {
                    return prevMessages;
                }
                return [...prevMessages, message];
            });
        });

        return () => {
            // Stop SignalR connection when component unmounts
            connection.stop();
        };
        // eslint-disable-next-line
    }, [connection]);
}

// Custom hook for Notifying server when a user is selected
const useSignalRSelectUser = ({ connection, selectedUserChannel }: SelectUserProps) => {
    useEffect(() => {
        if (!connection || !selectedUserChannel) return;

        // Notify server when a user is selected
        connection.invoke('SelectUser', selectedUserChannel._id).catch((err) => console.error('Error invoking SelectUser:', err));
        // eslint-disable-next-line
    }, [connection, selectedUserChannel]);
}

// Custom hook for Handling user selection in real-time
const useSignalRUserChannelSelected = ({ connection, userChannels, setSelectedUserChannel }: UserSelectedProps) => {

    useEffect(() => {
        if (!connection) return;

        // Subscribe to UserSelected event
        connection.on('UserSelected', (selectedUserId) => {
            // Handle user selection in real-time
            console.log(selectedUserId);
            const selected = userChannels.find((userChannel) => userChannel._id === selectedUserId);
            if (selected) {
                setSelectedUserChannel(selected);
            }
        });

        return () => {
            // Unsubscribe from UserSelected event when component unmounts
            connection.off('UserSelected');
        };
        // eslint-disable-next-line
    }, [userChannels, connection]);
}

// Custom hook for fetching user_channels and setting the selected user based on the user_id from the URL
const useGetUserChannels = ({ setUserChannels, setSelectedUserChannel }: GetUsersProps) => {

    // Function to handle successful user data retrieval
    const handleGetUsersSuccess = (response: OkResponse<UserChannelResponse>): void => {
        setUserChannels(response.data.user_channels)
        console.log(response)

        // Set selectedUser based on user_id from the URL
        const url = new URL(window.location.href);
        const user_id = url.searchParams.get('to.user_id');
        const ref_id = url.searchParams.get('from.ref_id');

        if (user_id && ref_id) {
            const selected = response.data.user_channels.find((user: UserChannel) => user.to.user_id === user_id);
            if (selected) {
                setSelectedUserChannel(selected);
            }
        }
    }

    // fetch users when the component mounts
    useEffect(() => {
        api.getUserChannels(handleGetUsersSuccess, (error) => { console.error('Error fetching users:', error); })
        // eslint-disable-next-line
    }, [])
}

// Custom hook for fetching messages based on the selected user
const useGetMessages = ({ setMessages, selectedUserChannel }: GetMessagesProps) => {

    // Function to handle successful message data retrieval
    const handleGetMessagesSuccess = (response: OkResponse<MessageResponse>): void => {
        console.log(response.data.messages)
        setMessages(response.data.messages)
    }

    // fetch messages when the selectedUser changes
    useEffect(() => {
        if (selectedUserChannel) {
            const params: Message = {
                provider_id: selectedUserChannel.provider_id,
                from: selectedUserChannel.from,
                to: selectedUserChannel.to
            }
            api.getMessages(handleGetMessagesSuccess, (error) => { console.error('Error fetching messages:', error); }, params)
        }
        // eslint-disable-next-line
    }, [selectedUserChannel])
}

export { useGetUserChannels, useGetMessages, useSignalRUserChannelSelected, useSignalRSelectUser, useSignalRReceiveMessage }