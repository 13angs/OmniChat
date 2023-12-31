import { useState } from 'react';
import Cookies from 'js-cookie';

const TOKEN_COOKIE_KEY = 'jwt-token';

export interface AuthToken {
    token: string | null;
    updateToken: (token: string | null) => void
}

export const useAuthToken = (): AuthToken => {
    const [token, setToken] = useState<string | null>(() => Cookies.get(TOKEN_COOKIE_KEY) ?? null);

    const updateToken = (newToken: string | null) => {
        setToken(newToken);
        if (newToken) {
            Cookies.set(TOKEN_COOKIE_KEY, newToken, { expires: 7 }); // Set the token to expire in 7 days (adjust as needed)
        } else {
            Cookies.remove(TOKEN_COOKIE_KEY);
        }
    };

    return { token, updateToken };
};
