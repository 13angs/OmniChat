import { AuthResponse, LoginRequest, Message, MessageRequest, MessageResponse, OkResponse, UserChannelRequest, UserChannelResponse, UserRequest, UserResponse } from "../shared/types";

// fetch users from the server
async function getUserChannels(onSuccess: (userChannels: OkResponse<UserChannelResponse>) => void, onError: (error: any) => void, userChannelRequest: UserChannelRequest) {
    try {
        // Fetch users from the 'api/chat/users' endpoint
        const response = await fetch(`api/v1/user/channels?by=${userChannelRequest.by}&provider_id=${userChannelRequest.provider_id}`);
        const data = await response.json();
        // Call the onSuccess callback with the retrieved user data

        if (response.status !== 200) {
            throw new Error(data.message);
        }
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

    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error?.message);
    }
}

// fetch messages for a specific user from the server
async function sendMessage(onSuccess: (messageResponse: OkResponse<MessageResponse>) => void, onError: (error: any) => void, body: MessageRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await fetch('api/v1/message/send', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body),
        });

        const data = await response.json();

        if(response.status !== 200)
        {
            throw new Error(data.message)
        }

    } catch (error) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// log the user in
async function login(onSuccess: (authResponse: OkResponse<AuthResponse>) => void, onError: (error: any) => void, body: LoginRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await fetch(`api/v1/auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body),
        });
        const data = await response.json();

        if (response.status !== 200) {
            throw new Error(data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(data);
    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// fetch the user info
async function getMyProfile(onSuccess: (userResponse: OkResponse<UserResponse>) => void, onError: (error: any) => void, body: UserRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await fetch(`api/v1/user/me`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body),
        });
        const data = await response.json();

        if (response.status !== 200) {
            throw new Error(data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(data);
    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function getUserFriends(onSuccess: (userFriends: OkResponse<UserResponse>) => void, onError: (error: any) => void, userRequest: UserRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await fetch(`api/v1/users?by=friend&provider_id=${userRequest.provider_id}&user_id=${userRequest.user_id}&current_status=${userRequest.current_status}`);
        const data = await response.json();

        if (response.status !== 200) {
            throw new Error(data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(data);

    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function addFriend(onSuccess: (userFriend: OkResponse<string>) => void, onError: (error: any) => void, userRequest: UserRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await fetch(`api/v1/user/channel/friend/add`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userRequest),
        });
        // console.log(response.status)

        const data = await response.json();

        if (response.status !== 201) {
            throw new Error(data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(data);

    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function getUserProfile(onSuccess: (userResponse: OkResponse<UserResponse>) => void, onError: (error: any) => void, userRequest: UserRequest | null) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await fetch(`api/v1/user/profile?user_id=${userRequest?.user_id}`);
        const data = await response.json();

        if (response.status !== 200) {
            throw new Error(data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(data);

    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// Object containing utility functions for interacting with the server API
const api = {
    getUserChannels,
    getMessages,
    sendMessage,
    login,
    getMyProfile,
    getUserFriends,
    addFriend,
    getUserProfile
}

export default api;