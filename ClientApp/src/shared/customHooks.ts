import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';

export interface CookieOptions extends Cookies.CookieAttributes {
  key: string;
}

export const useCookie = (options: CookieOptions) => {
  const [cookieValue, setCookieValue] = useState<string | undefined>(() => Cookies.get(options.key));

  useEffect(() => {
    const fetchCookie = () => {
      const value = Cookies.get(options.key);
      setCookieValue(value);
    };

    fetchCookie();
  }, [options.key]);

  const setCookie = (value: string, cookieOptions?: Cookies.CookieAttributes) => {
    Cookies.set(options.key, value, { ...options, ...cookieOptions });
    setCookieValue(value);
  };

  const removeCookie = (cookieOptions?: Cookies.CookieAttributes) => {
    Cookies.remove(options.key, { ...options, ...cookieOptions });
    setCookieValue(undefined);
  };

  return { cookieValue, setCookie, removeCookie };
};