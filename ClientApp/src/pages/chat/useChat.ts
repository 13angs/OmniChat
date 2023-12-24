import React, { useEffect } from "react";
import api from "../../utils/api";
import { Message, MessageParam, User } from "../../shared/types";

interface GetUsersProps {
    setUsers: React.Dispatch<React.SetStateAction<User[]>>;
    setSelectedUser: React.Dispatch<React.SetStateAction<User | null>>;
}

interface GetMessagesProps {
    setMessages: React.Dispatch<React.SetStateAction<Message[]>>;
    selectedUser: User | null;
}

interface UserSelectedProps {
    connection: signalR.HubConnection | null;
    users: User[]
    setSelectedUser: React.Dispatch<React.SetStateAction<User | null>>;
}

interface SelectUserProps {
    connection: signalR.HubConnection | null;
    selectedUser: User | null
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
                if (prevMessages.some((item) => item.timestamp === message.timestamp)) {
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
const useSignalRSelectUser = ({ connection, selectedUser }: SelectUserProps) => {
    useEffect(() => {
        if (!connection || !selectedUser) return;

        // Notify server when a user is selected
        connection.invoke('SelectUser', selectedUser._id).catch((err) => console.error('Error invoking SelectUser:', err));
        // eslint-disable-next-line
    }, [connection, selectedUser]);
}

// Custom hook for Handling user selection in real-time
const useSignalRUserSelected = ({ connection, users, setSelectedUser }: UserSelectedProps) => {

    useEffect(() => {
        if (!connection) return;

        // Subscribe to UserSelected event
        connection.on('UserSelected', (selectedUserId) => {
            // Handle user selection in real-time
            console.log(selectedUserId);
            const selected = users.find((user) => user._id === selectedUserId);
            if (selected) {
                setSelectedUser(selected);
            }
        });

        return () => {
            // Unsubscribe from UserSelected event when component unmounts
            connection.off('UserSelected');
        };
        // eslint-disable-next-line
    }, [users, connection]);
}

// Custom hook for fetching users and setting the selected user based on the user_id from the URL
const useGetUsers = ({ setUsers, setSelectedUser }: GetUsersProps) => {

    // Function to handle successful user data retrieval
    const handleGetUsersSuccess = (data: User[]): void => {
        setUsers(data)

        // Set selectedUser based on user_id from the URL
        const url = new URL(window.location.href);
        const user_id = url.searchParams.get('user_id');
        if (user_id) {
            const selected = data.find((user: User) => user._id === user_id);
            if (selected) {
                setSelectedUser(selected);
            }
        }
    }

    // fetch users when the component mounts
    useEffect(() => {
        api.getUsers(handleGetUsersSuccess, (error) => { console.error('Error fetching users:', error); })
        // eslint-disable-next-line
    }, [])
}

// Custom hook for fetching messages based on the selected user
const useGetMessages = ({ setMessages, selectedUser }: GetMessagesProps) => {

    // Function to handle successful message data retrieval
    const handleGetMessagesSuccess = (data: Message[]): void => {
        setMessages(data)
    }

    // fetch messages when the selectedUser changes
    useEffect(() => {
        if (selectedUser) {
            const params: MessageParam = {
                user_id: selectedUser?._id
            }
            api.getMessages(handleGetMessagesSuccess, (error) => { console.error('Error fetching messages:', error); }, params)
        }
        // eslint-disable-next-line
    }, [selectedUser])
}

export { useGetUsers, useGetMessages, useSignalRUserSelected, useSignalRSelectUser, useSignalRReceiveMessage }