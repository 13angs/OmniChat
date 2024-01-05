import React from 'react';
import { UserChannel } from '../../shared/types';

interface UserListProps {
    userChannels: UserChannel[];
    selectedUserChannel: UserChannel | null;
    selectUserChannel: (userChannel: UserChannel) => void;
}

const UserList: React.FC<UserListProps> = ({ userChannels, selectedUserChannel, selectUserChannel }) => {
    return (
        <div className="h-screen flex w-1/4 flex-col p-4 border-r">
            <div>
                <h2 className="text-xl font-semibold mb-4">Users</h2>
                <p className="mb-2">
                    {selectedUserChannel ? `Selected: ${selectedUserChannel.from.name}` : 'No user selected'}
                </p>
            </div>
            <div className="overflow-y-scroll">
                {userChannels.map((userChannel) => (
                    <button
                        style={{ width: '100%', textAlign: 'left' }}
                        key={`${userChannel._id}`}
                        className={`cursor-pointer mb-2 p-2 rounded ${selectedUserChannel?._id === userChannel._id ? 'bg-blue-200' : 'bg-gray-200'}`}
                        onClick={() => selectUserChannel(userChannel)}
                    >
                        {userChannel.to.name}
                    </button>
                ))}
            </div>
        </div>
    );
};

export default UserList;
