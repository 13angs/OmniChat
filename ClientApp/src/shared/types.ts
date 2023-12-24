export interface User {
    _id: string;
    name: string;
    avatar: string;
}

export interface Message {
    _id?: string;
    user_id: string;
    text: string;
    timestamp: number;
}