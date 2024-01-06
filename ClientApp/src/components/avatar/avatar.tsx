import React from 'react';

interface AvatarProps {
  name: string | null;
  avatar: string | null;
  width?: number;
  height?: number;
  displayName?: boolean
}

const Avatar: React.FC<AvatarProps> = ({ name, avatar, width = 100, height = 100, displayName = true }) => {
  return (
    <div>
      <div className="w-12 h-12 rounded-full mb-2 overflow-hidden">
        {/* Use the Image component from next/image */}
        {avatar ? (
          <img
            src={avatar}
            alt={avatar}
            className="w-12 h-12 rounded-full mb-2"
            width={width}
            height={height}
          />
        ) : (
          <div className="w-12 h-12 flex items-center justify-center bg-gray-300 rounded-full mb-2">
            <p className="text-xl font-semibold text-gray-600">
              {name?.charAt(0).toUpperCase()}
            </p>
          </div>
        )}
      </div>
      {displayName && (
        <p className="text-xl font-semibold">{name}</p>
      )}
    </div>
  );
};

export default Avatar;
