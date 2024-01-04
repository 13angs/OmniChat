// MainContainer.tsx

import React, { ReactNode, createContext, useContext, useMemo, useState } from 'react';
import Drawer from '../../components/drawer/drawer';
import { Menu } from 'react-feather';
import { CookieOptions, useCookie } from '../../shared/customHooks';
import { contants } from '../../shared/contants';
import { User } from '../../shared/types'

// Create a context with initial state
interface MainContainerContextProps {
    isDrawerOpen: boolean;
    toggleDrawer: () => void;
    myProfile: User
}

const MainContainerContext = createContext<MainContainerContextProps | undefined>(undefined);

export const useMainContainerContext = () => {
    const context = useContext(MainContainerContext);
    if (!context) {
        throw new Error('useMainContainerContext must be used within a MainContainerProvider');
    }
    return context;
};

interface MainContainerProps {
    children: ReactNode;
}

const myProfileOptions: CookieOptions = {
    key: contants.MY_PROFILE_COOKIE_KEY
};


const MainContainer: React.FC<MainContainerProps> = ({ children }) => {
    const [isDrawerOpen, setIsDrawerOpen] = useState(false);
    const { cookieValue } = useCookie(myProfileOptions);

    const toggleDrawer = () => {
        setIsDrawerOpen((prevIsOpen) => !prevIsOpen);
    };

    const obj = useMemo(() => ({ isDrawerOpen, toggleDrawer, myProfile: JSON.parse(cookieValue ?? "") }), [isDrawerOpen, cookieValue])

    return (
        <MainContainerContext.Provider value={obj}>
            <div className="flex">
                {/* Drawer Component */}
                <Drawer isOpen={isDrawerOpen} toggleDrawer={toggleDrawer} />

                {/* Main Content */}
                <div className="flex-grow p-4">
                    <div className={`flex-grow transition-all duration-300 w-full`}>
                        {/* Menu Icon */}
                        <button className="cursor-pointer" onClick={toggleDrawer}>
                            <Menu size={24} />
                        </button>
                        {/* Your main content goes here */}
                        {children}
                    </div>
                </div>
            </div>
        </MainContainerContext.Provider>
    );
};

export default MainContainer;
