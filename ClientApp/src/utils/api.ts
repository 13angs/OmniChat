import { Message, MessageResponse, OkResponse, UserChannelResponse } from "../shared/types";

// fetch users from the server
async function getUserChannels(onSuccess: (userChannels: OkResponse<UserChannelResponse>) => void, onError: (error: any) => void) {
    try {
        // Fetch users from the 'api/chat/users' endpoint
        const response = await fetch('api/v1/user/channels?by=provider&provider_id=3012cdae-4f0c-48cf-8930-a2ca01115e5a');
        const data = await response.json();
        // Call the onSuccess callback with the retrieved user data
        onSuccess(data);
    } catch (error) {
        // Call the onError callback in case of an error during user data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function getMessages(onSuccess: (messages: OkResponse<MessageResponse>) => void, onError: (error: any) => void, params: Message) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await fetch(`api/v1/messages?by=user&provider_id=${params?.provider_id}&from.ref_id=${params?.from?.ref_id}&to.user_id=${params?.to?.user_id}`);
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