export interface User {
    _id: string;
    provider_id: string;
    username: string;
    name: string;
    first_name: string;
    last_name: string;
    avatar: string | null;
    created_timestamp: number;
    modified_timestamp: number;
    current_status: number;
}
export interface UserChannel {
    _id: string;
    created_timestamp: number;
    modified_timestamp: number;
    platform: string; // Assuming this is an enum
    provider_id: string;
    channel_id: string;
    channel_type: string; // Assuming this is an enum
    operation_mode: string; // Assuming this is an enum
    message_exchange: string; // Assuming this is an enum
    from: MessageFrom; // Assuming MessageFrom is a type defined elsewhere
    to: MessageTo;   // Assuming MessageUser is a type defined elsewhere
    latest_message?: string;
    is_read?: boolean;
    related_users?: RelatedUser[]; // Assuming RelatedUser is a type defined below
}
interface RelatedUser {
    user_id: string;
    is_read: boolean;
}
export interface MessageFrom {
    ref_id?: string;
    name?: string;
}
export interface MessageTo {
    user_id?: string;
    name?: string
}

export interface Message {
    _id?: string;
    created_timestamp?: number;
    modified_timestamp?: number;
    platform?: string; // Assuming this is an enum
    provider_id?: string;
    channel_id?: string;
    channel_type?: string; // Assuming this is an enum
    operation_mode?: string; // Assuming this is an enum
    message_exchange?: string; // Assuming this is an enum
    message_object?: any;
    from?: MessageFrom; // Assuming MessageFrom is a type defined below
    to?: MessageTo;   // Assuming MessageUser is a type defined below
}

// responses
export interface OkResponse<T> {
    data: T
}
export interface UserChannelResponse {
    user_channels: UserChannel[]
}
export interface MessageResponse {
    messages: Message[]
}
export interface AuthResponse {
    token: string
}
export interface UserResponse {
    user: User;
    users: User[]
}

// request
export interface LoginRequest {
    username: string;
    password: string;
}
export interface UserRequest {
    token?: string;
}