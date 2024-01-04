// Drawer.tsx

import React from 'react';
import { Facebook, Users } from 'react-feather';
import { Link } from 'react-router-dom';

interface DrawerProps {
  isOpen: boolean;
  toggleDrawer: () => void;
}

const Drawer: React.FC<DrawerProps> = ({ isOpen, toggleDrawer }) => {
  return (
    <div
      className={`bg-gray-800 text-white w-64 h-screen p-4 transition-all duration-300 ${
        isOpen ? 'translate-x-0' : '-translate-x-full fixed top-0 left-0'
      }`}
    >
      {/* Menu Icon for Closing Drawer */}
      <p className="text-2xl font-bold mb-4">Drawer Content</p>

      {/* Menu Items/Links */}
      <Link to="/chat" onClick={toggleDrawer} className="flex py-2 px-2 hover:bg-gray-700">
        <Facebook className="mr-2" />Chat
      </Link>
      <Link to="/friends" onClick={toggleDrawer} className="flex py-2 px-2 hover:bg-gray-700">
        <Users className="mr-2" /> Friends
      </Link>

      {/* Add additional items in the drawer as needed */}
    </div>
  );
};

export default Drawer;
