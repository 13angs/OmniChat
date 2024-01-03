// FriendsPage.tsx

import React, { useEffect, useState } from 'react';
import Drawer from '../../components/drawer/Drawer';
import api from '../../utils/api';
import { OkResponse, User, UserResponse } from '../../shared/types';
import { Menu } from 'react-feather';

const FriendsPage: React.FC = () => {
  const [friends, setFriends] = useState<User[]>([]);
  const [isDrawerOpen, setDrawerOpen] = useState(false);

  const toggleDrawer = () => {
    setDrawerOpen(!isDrawerOpen);
  };

  useEffect(() => {
    const handleSuccess = (userFriend: OkResponse<UserResponse>) => {
      setFriends(userFriend.data.users)
    }
    api.getUserFriends(handleSuccess, (error) => { alert(error) })
  }, []);

  return (
    <div className={`flex transition-all duration-300 ${isDrawerOpen ? '' : 'h-screen'}`}>
      {/* Collapsible Drawer */}
      <Drawer isOpen={isDrawerOpen} toggleDrawer={toggleDrawer} />

      {/* Main Content */}
      <div
        className={`flex-grow p-4 transition-all duration-300 ${
          isDrawerOpen ? 'w-3/4' : 'w-full'
        }`}
      >
        {/* Menu Icon */}
        <div className="cursor-pointer" onClick={toggleDrawer}>
          <Menu size={24} />
        </div>

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
      </div>
    </div>
  );
};

export default FriendsPage;
