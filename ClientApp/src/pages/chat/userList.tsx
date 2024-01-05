import React, { useMemo, useState } from 'react';
import { RelatedUser, UserChannel } from '../../shared/types';
import { useMainContainerContext } from '../../containers/main/mainContainer';
import { useUserChannel } from '../../shared/customHooks';
import { useGetUserChannels } from './useChat';

// Component for rendering individual user buttons
interface UserButtonProps {
    userChannel: UserChannel;
    isSelected: boolean;
    onClick: () => void;
}

const UserButton: React.FC<UserButtonProps> = ({ userChannel, isSelected, onClick }) => {
    const { myProfile } = useMainContainerContext();
    // Use custom hook to find friend based on user channel and current user ID
    const friendName = useUserChannel().findFriend(userChannel, myProfile?._id)?.name;

    return (
        <button
            className={`cursor-pointer mb-2 p-2 rounded ${isSelected ? 'bg-blue-200' : 'bg-gray-200'}`}
            style={{ width: '100%', textAlign: 'left' }}
            onClick={onClick}
        >
            {friendName}
        </button>
    );
};

// Main UserList component
interface UserListProps {
    selectUserChannel: (userChannel: UserChannel) => void;
}

const UserList: React.FC<UserListProps> = ({ selectUserChannel }) => {
    // State for managing user channels and selected user channel
    const [userChannels, setUserChannels] = useState<UserChannel[]>([]);
    const [selectedUserChannel, setSelectedUserChannel] = useState<UserChannel | null>(null);

    // Context and custom hook for user-related functionality
    const { myProfile } = useMainContainerContext();
    const { findFriend } = useUserChannel();

    // Custom hook for fetching user channels
    useGetUserChannels({ setUserChannels, setSelectedUserChannel })

    // Use useMemo to memoize the relatedUser value
    const relatedUser: RelatedUser | undefined = useMemo(() => {
        return findFriend(selectedUserChannel ?? undefined, myProfile?._id);
    }, [findFriend, myProfile?._id, selectedUserChannel]);

    return (
        <div className="h-screen flex w-1/4 flex-col p-4 border-r">
            <div>
                <h2 className="text-xl font-semibold mb-4">Users</h2>
                <p className="mb-2">
                    {relatedUser ? `Selected: ${relatedUser.name}` : 'No user selected'}
                </p>
            </div>
            <div className="overflow-y-scroll">
                {/* Map through user channels to render UserButton components */}
                {userChannels.map((userChannel) => (
                    <UserButton
                        key={userChannel._id}
                        userChannel={userChannel}
                        isSelected={relatedUser?.user_id === userChannel._id}
                        onClick={() => selectUserChannel(userChannel)}
                    />
                ))}
            </div>
        </div>
    );
};

export default UserList;
