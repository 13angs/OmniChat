import { messageType } from "../shared/contants";
import { TextMessage } from "../shared/types"

export class MessageTypeService {
    static getTextMessage(text: string) {
        const message: TextMessage = {
            type: messageType.text,
            text
        }

        return [message];
    }
}