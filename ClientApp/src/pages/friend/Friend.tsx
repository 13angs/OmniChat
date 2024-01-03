// FriendsPage.tsx

import React, { useEffect, useState } from 'react';
import api from '../../utils/api';
import { OkResponse, User, UserResponse } from '../../shared/types';

const FriendsPage: React.FC = () => {
  const [friends, setFriends] = useState<User[]>([]);

  useEffect(() => {
    const handleSuccess = (userFriend: OkResponse<UserResponse>) => {
      setFriends(userFriend.data.users)
    }
    api.getUserFriends(handleSuccess, (error) => { alert(error) })
  }, []);

  return (
    <>
      <h1 className="text-3xl font-bold mt-4 mb-4">Friends Page</h1>
      <div className="grid grid-cols-3 gap-4">
        {friends.map((friend) => (
          <div key={friend._id} className="bg-gray-200 p-4 rounded">
            <p className="text-lg font-semibold">{friend.name}</p>
            <p className="text-gray-600">{friend.username}</p>
            {/* Add more details as needed */}
          </div>
        ))}
      </div>
    </>
  );
};

export default FriendsPage;
