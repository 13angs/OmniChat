// FriendsPage.tsx

import React, { useEffect, useState } from 'react';
import api from '../../utils/api';
import { OkResponse, User, UserRequest, UserResponse } from '../../shared/types';
import { useMainContainerContext } from '../../containers/main/mainContainer';
import { useLocation } from 'react-router-dom';

const FriendsPage: React.FC = () => {
  const [friends, setFriends] = useState<User[]>([]);
  const { myProfile } = useMainContainerContext();
  const location = useLocation();

  // Parse query parameters from the location
  const queryParams = new URLSearchParams(location.search);
  const current_status = queryParams.get('current_status') ?? 'unfollow';

  useEffect(() => {
    const handleSuccess = (userFriend: OkResponse<UserResponse>) => {
      setFriends(userFriend.data.users)
    }
    const userRequest: UserRequest = {
      provider_id: myProfile.provider_id,
      user_id: myProfile._id,
      current_status: current_status ?? "unfollow"
    };

    api.getUserFriends(handleSuccess, (error) => { alert(error) }, userRequest)
  }, [myProfile, current_status]);

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
