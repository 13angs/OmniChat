import { messageType } from "../shared/contants";
import { Message, MessageTo, TextMessage, User } from "../shared/types"

export class MessageTypeService {
    static getTextMessage(text: string) {
        const message: TextMessage = {
            type: messageType.text,
            text
        }

        return [message];
    }
}

export class UserChannelService {
    static displayUsertMessage(message: Message, myProfile?: User | null, messageTo?: MessageTo, fromStyle?: string, toStyle?: string) {
        if (message.from?.ref_id === messageTo?.user_id && message.to?.user_id === myProfile?._id) {
            return fromStyle; //'bg-gray-200'
        } else if (message.from?.ref_id === myProfile?._id && message.to?.user_id === messageTo?.user_id) {
            return toStyle; //'bg-blue-500 text-white self-end'
        }
    }
}