import React, { useEffect, useMemo } from "react";
import api from "../../utils/api";
import { Message, MessageRequest, MessageResponse, OkResponse, User, UserChannel, UserChannelRequest, UserChannelResponse, UserRequest, UserResponse } from "../../shared/types";
import { useMainContainerContext } from "../../containers/main/mainContainer";
import { RequestParam } from "../../shared/contants";

interface GetUsersProps {
    setUserChannels: React.Dispatch<React.SetStateAction<UserChannel[]>>;
    queryBy: RequestParam
}

interface GetMessagesProps {
    setMessages: React.Dispatch<React.SetStateAction<Message[]>>;
    messageRequest: MessageRequest
    // selectedUserChannel: UserChannel | null;
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

interface GetUserProfileProps {
    setUserProfile: React.Dispatch<React.SetStateAction<User | null>>;
    userRequest: UserRequest | null;
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
const useGetUserChannels = ({ setUserChannels, queryBy }: GetUsersProps) => {
    const { myProfile } = useMainContainerContext();

    // Function to handle successful user data retrieval
    const handleGetUsersSuccess = (response: OkResponse<UserChannelResponse>): void => {
        setUserChannels(response.data.user_channels)
        // Set selectedUser based on user_id from the URL
        // const url = new URL(window.location.href);
        // const user_id = url.searchParams.get('to.user_id');
        // const ref_id = url.searchParams.get('from.ref_id');
    }

    const userChannelRequest: UserChannelRequest = useMemo(() => ({
        by: queryBy,
        provider_id: myProfile?.provider_id,
        from: {
            ref_id: myProfile?._id
        }
    }), [myProfile?._id, myProfile?.provider_id, queryBy])

    // fetch users when the component mounts
    useEffect(() => {
        api.getUserChannels(handleGetUsersSuccess, (error) => { console.error('Error fetching users:', error); }, userChannelRequest)
        // eslint-disable-next-line
    }, [])
}

// Custom hook for fetching messages based on the selected user
const useGetMessages = ({ setMessages, messageRequest }: GetMessagesProps) => {


    // fetch messages when the selectedUser changes
    useEffect(() => {
        // Function to handle successful message data retrieval
        const handleGetMessagesSuccess = (response: OkResponse<MessageResponse>): void => {
            setMessages(response.data.messages)
        }
        const params: Message = {
            provider_id: messageRequest.provider_id,
            from: messageRequest.from,
            to: messageRequest.to
        }

        // reset the messages
        setMessages([]);
        api.getMessages(handleGetMessagesSuccess, (error) => { console.error('Error fetching messages:', error); }, params)

    }, [messageRequest, setMessages])
}
// Custom hook for fetching messages based on the selected user
const useGetUserProfile = ({ setUserProfile, userRequest }: GetUserProfileProps) => {


    // fetch messages when the selectedUser changes
    useEffect(() => {
        // Function to handle successful message data retrieval
        const handleGetUserProfileSuccess = (response: OkResponse<UserResponse>): void => {
            setUserProfile(response.data.user)
        }

        api.getUserProfile(handleGetUserProfileSuccess, (error) => { console.error('Error fetching messages:', error); }, userRequest ?? null)

    }, [setUserProfile, userRequest])
}

export { useGetUserChannels, useGetMessages, useSignalRUserChannelSelected, useSignalRSelectUser, useSignalRReceiveMessage, useGetUserProfile }