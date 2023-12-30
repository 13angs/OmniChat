export interface User {
    _id: string;
    name: string;
    avatar: string;
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
interface MessageFrom{
    ref_id: string;
    name: string
}
interface MessageTo{
    user_id: string;
    name: string
}


export interface Message {
    _id?: string;
    user_id: string;
    text: string;
    timestamp: number;
}

export interface MessageParam {
    user_id?: string;
}

// responses
export interface OkResponse<T>{
    data: T
}
export interface UserChannelResponse{
    user_channels: UserChannel[]
}