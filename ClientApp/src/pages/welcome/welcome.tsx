// Welcome.tsx
import React, { useEffect, useState } from 'react';
import api from '../../utils/api';
import { OkResponse, User, UserRequest, UserResponse } from '../../shared/types';
import { CookieOptions, useCookie } from '../../shared/customHooks';
import { useNavigate } from 'react-router-dom';
import { contants } from '../../shared/contants';

const tokenProfileOptions: CookieOptions = {
    key: contants.TOKEN_COOKIE_KEY
}
const myProfileOptions: CookieOptions = {
    key: contants.MY_PROFILE_COOKIE_KEY
}

const Welcome: React.FC = () => {
    const navigate = useNavigate();
    const [userData, setUserData] = useState<User | null>(null);
    const { cookieValue: token } = useCookie(tokenProfileOptions);
    const { setCookie } = useCookie(myProfileOptions);

    const handleGetMyProfileSuccess = (userResponse: OkResponse<UserResponse>): void => {
        const user: User = userResponse.data.user;
        setUserData(user);
        setCookie(JSON.stringify(user));
    }

    useEffect(() => {
        const userRequest: UserRequest = {
            token: token
        }

        // Make the POST request to /user/me
        if (token) {
            api.getMyProfile(handleGetMyProfileSuccess, (err) => { alert(err.message) }, userRequest);
        }else{
            navigate('/login');
        }

    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-r from-blue-500 to-indigo-500 text-white">
            <div className="text-center">
                <h1 className="text-4xl font-bold mb-4">Welcome, {userData?.name ?? 'Guest'}!</h1>
                {userData && (
                    <div>
                        <p>Username: {userData.username}</p>
                        <p>Provider ID: {userData.provider_id}</p>
                        {/* Add more user details as needed */}
                    </div>
                )}
            </div>
        </div>
    );
};

export default Welcome;
