// FriendsPage.tsx

import React, { useEffect, useMemo, useState } from 'react';
import api from '../../utils/api';
import { OkResponse, User, UserRequest, UserResponse } from '../../shared/types';
import { useMainContainerContext } from '../../containers/main/mainContainer';
import { useLocation } from 'react-router-dom';
import Avatar from '../../components/avatar/avatar';
import Dialog from '../../components/dialog/dialog';

const FriendsPage: React.FC = () => {
  const [friends, setFriends] = useState<User[]>([]);
  const { myProfile } = useMainContainerContext();
  const [showConfirmation, setShowConfirmation] = useState<boolean>(false);
  const [selectedFriend, setSelectedFriend] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const location = useLocation();

  // Parse query parameters from the location
  const queryParams = new URLSearchParams(location.search);
  const current_status = queryParams.get('current_status') ?? 'unfollow';
  const handleGetUserFriendsSuccess = (userFriend: OkResponse<UserResponse>) => {
    setFriends(userFriend.data.users)
  }
  const getUserFriendsRequest: UserRequest = useMemo(() => ({
    provider_id: myProfile.provider_id,
    user_id: myProfile._id,
    current_status: current_status ?? "unfollow"
  }), [current_status, myProfile._id, myProfile.provider_id])

  useEffect(() => {
    api.getUserFriends(handleGetUserFriendsSuccess, (error) => { alert(error) }, getUserFriendsRequest)
  }, [myProfile, current_status, getUserFriendsRequest]);


  const confirmAddFriend = () => {
    // Start loading
    setLoading(true);

    // Implement logic to add friend
    if (selectedFriend) {
      const userRequest: UserRequest = {
        provider_id: selectedFriend.provider_id,
        from: {
          ref_id: myProfile._id,
          name: myProfile.name,
          avatar: myProfile.avatar ?? ""
        },
        to: {
          user_id: selectedFriend._id,
          name: selectedFriend.name,
          avatar: selectedFriend.avatar ?? ""
        }
      }
      api.addFriend(() => {
        api.getUserFriends(handleGetUserFriendsSuccess, (error) => { alert(error) }, getUserFriendsRequest)
        // Loading is done, close the confirmation dialog
        setLoading(false);
        // Close the confirmation dialog
        setShowConfirmation(false);
      }, (err) => {
        // Loading is done, close the confirmation dialog
        setLoading(false);
        // Close the confirmation dialog
        setShowConfirmation(false);
        alert(err)
      }, userRequest);
    }
  };

  const handleAddFriend = (friend: User) => {
    // Show the confirmation dialog
    setShowConfirmation(true);
    setSelectedFriend(friend);
  };

  const cancelAddFriend = () => {
    // Close the confirmation dialog
    setShowConfirmation(false);
  };

  return (
    <>
      <h1 className="text-3xl font-bold mt-4 mb-4">Friends Page</h1>
      <div className="grid grid-cols-3 gap-4">
        {friends.map((friend) => (
          <div key={friend._id} className="bg-gray-200 p-4 rounded">
            <Avatar name={friend.name} avatar={friend.avatar} />
            <p className="text-lg font-semibold">{friend.name}</p>
            <p className="text-gray-600">{friend.username}</p>
            <button
              onClick={() => handleAddFriend(friend)}
              className="mt-2 bg-blue-500 text-white px-4 py-2 rounded"
            >
              Add Friend
            </button>
            {/* Add more details as needed */}
          </div>
        ))}
      </div>

      {/* Confirmation Dialog */}
      <Dialog
        show={showConfirmation}
        loading={loading}
        onConfirm={confirmAddFriend}
        onCancel={cancelAddFriend}
      />
    </>
  );
};

export default FriendsPage;
