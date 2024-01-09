import { RequestParam } from "../shared/contants";
import axios from 'axios';
import { AuthResponse, LoginRequest, Message, MessageRequest, MessageResponse, OkResponse, UserChannelRequest, UserChannelResponse, UserRequest, UserResponse } from "../shared/types";

// fetch users from the server
async function getUserChannels(onSuccess: (userChannels: OkResponse<UserChannelResponse>) => void, onError: (error: any) => void, userChannelRequest: UserChannelRequest) {
    try {
        let endpoint = `api/v1/user/channels?by=${userChannelRequest.by}&provider_id=${userChannelRequest.provider_id}`;

        if (userChannelRequest.by === RequestParam.friend) {
            endpoint = `api/v1/user/channels?by=${userChannelRequest.by}&provider_id=${userChannelRequest.provider_id}&from.ref_id=${userChannelRequest.from?.ref_id}`;
        }
        // Fetch users from the 'api/chat/users' endpoint
        const response = await axios.get(endpoint);
        // Call the onSuccess callback with the retrieved user data

        if (response.status !== 200) {
            throw new Error(response.data.message);
        }
        onSuccess(response.data);
    } catch (error) {
        // Call the onError callback in case of an error during user data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function getMessages(onSuccess: (messages: OkResponse<MessageResponse>) => void, onError: (error: any) => void, params: Message) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await axios.get(`api/v1/messages?by=user&provider_id=${params?.provider_id}&from.ref_id=${params?.from?.ref_id}&to.user_id=${params?.to?.user_id}`);
        // Call the onSuccess callback with the retrieved message data
        onSuccess(response.data);

    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error?.message);
    }
}

// fetch messages for a specific user from the server
async function sendMessage(onSuccess: (messageResponse: OkResponse<MessageResponse>) => void, onError: (error: any) => void, body: MessageRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await axios.post('api/v1/message/send', body, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.status !== 200) {
            throw new Error(response.data.message)
        }

        onSuccess(response.data);

    } catch (error) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// log the user in
async function login(onSuccess: (authResponse: OkResponse<AuthResponse>) => void, onError: (error: any) => void, body: LoginRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        console.log(body)
        const response = await axios.post(`api/v1/auth/login`, body, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.status !== 200) {
            throw new Error(response.data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(response.data);
    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// fetch the user info
async function getMyProfile(onSuccess: (userResponse: OkResponse<UserResponse>) => void, onError: (error: any) => void, body: UserRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await axios.post(`api/v1/user/me`, body, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.status !== 200) {
            throw new Error(response.data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(response.data);
    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function getUserFriends(onSuccess: (userFriends: OkResponse<UserResponse>) => void, onError: (error: any) => void, userRequest: UserRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await axios.get(`api/v1/users?by=friend&provider_id=${userRequest.provider_id}&user_id=${userRequest.user_id}&current_status=${userRequest.current_status}`);

        if (response.status !== 200) {
            throw new Error(response.data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(response.data);

    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function addFriend(onSuccess: (userFriend: OkResponse<string>) => void, onError: (error: any) => void, body: UserRequest) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await axios.post(`api/v1/user/channel/friend/add`, body, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });
        // console.log(response.status)


        if (response.status !== 201) {
            throw new Error(response.data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(response.data);

    } catch (error: any) {
        // Call the onError callback in case of an error during message data retrieval
        onError(error);
    }
}

// fetch messages for a specific user from the server
async function getUserProfile(onSuccess: (userResponse: OkResponse<UserResponse>) => void, onError: (error: any) => void, userRequest: UserRequest | null) {
    try {
        // Fetch messages from the 'api/chat/messages' endpoint with the specified user_id parameter
        const response = await axios.get(`api/v1/user/profile?user_id=${userRequest?.user_id}`);

        if (response.status !== 200) {
            throw new Error(response.data.message)
        }

        // Call the onSuccess callback with the retrieved message data
        onSuccess(response.data);

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