import React, { useState } from 'react';
import { AuthResponse, LoginRequest, OkResponse } from '../../shared/types';
import api from '../../utils/api';
import { useAuthToken } from '../../shared/customHooks';

interface LoginFormProps {
    onSubmit: (username: string, password: string) => void;
}

const LoginForm: React.FC<LoginFormProps> = ({ onSubmit }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(username, password);
    };

    return (
        <form onSubmit={handleLogin} className="max-w-sm mx-auto mt-8">
            <label className="block text-gray-700 text-sm font-bold mb-2">
                Username:
                <input
                    type="text"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                />
            </label>
            <label className="block text-gray-700 text-sm font-bold mb-2">
                Password:
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                />
            </label>
            <button type="submit" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline">
                Login
            </button>
        </form>
    );
};


const Login: React.FC = () => {
    const { updateToken } = useAuthToken();

    const handleLoginSuccess = (authResponse: OkResponse<AuthResponse>) => {
        updateToken(authResponse.data.token);
        alert('Login successful!'); // Replace this with actual logic.
    }
    const handleLoginSubmit = (username: string, password: string) => {
        // Assuming you make an API request here using fetch or axios
        // with the provided username and password, and handle the response.
        const loginRequest: LoginRequest = {
            username,
            password
        };

        api.login(handleLoginSuccess, (err) => { console.log(err) }, loginRequest)
        // For simplicity, I'll log the credentials and show a success message.

        // If you have a server, make an API call here and handle the response.
    };

    return (
        <div className="min-h-screen flex items-center justify-center">
            <div className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4">
                <h2 className="text-2xl font-bold mb-8">Login Page</h2>
                <LoginForm onSubmit={handleLoginSubmit} />
            </div>
        </div>
    );
};

export default Login;
