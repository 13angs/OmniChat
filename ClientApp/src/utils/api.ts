import { Message, MessageParam, OkResponse, UserChannelResponse } from "../shared/types";

// fetch users from the server
async function getUserChannels(onSuccess: (userChannels: OkResponse<UserChannelResponse>) => void, onError: (error: any) => void) {
    try {
        // Fetch users from the 'api/chat/users' endpoint
        const response = await fetch('api/v1/user/channels?by=provider&provider_id=18bc6926-1a47-40fd-88bd-26700fed67ac');
        const data = await response.json();
        // Call the onSuccess callback with the retrieved user data
        onSuccess(data);
    } catch (error) {
        // Call the onError callback in case of an error during user data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function getMessages(onSuccess: (messages: Message[]) => void, onError: (error: any) => void, params: MessageParam) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await fetch(`api/chat/messages?user_id=${params.user_id}`);
        const data = await response.json();
        // Call the onSuccess callback with the retrieved message data
        onSuccess(data);
    } catch (error) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function sendMessage(onSuccess: () => void, onError: (error: any) => void, body: Message) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        await fetch('api/chat/sendMessage', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body),
        });

    } catch (error) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// Object containing utility functions for interacting with the server API
const api = {
    getUserChannels,
    getMessages,
    sendMessage
}

export default api;