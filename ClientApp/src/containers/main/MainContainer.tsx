// MainContainer.tsx

import React, { ReactNode, createContext, useContext, useMemo, useState } from 'react';
import Drawer from '../../components/drawer/Drawer';
import { Menu } from 'react-feather';

// Create a context with initial state
interface MainContainerContextProps {
    isDrawerOpen: boolean;
    toggleDrawer: () => void;
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


const MainContainer: React.FC<MainContainerProps> = ({ children }) => {
    const [isDrawerOpen, setIsDrawerOpen] = useState(false);

    const toggleDrawer = () => {
        setIsDrawerOpen((prevIsOpen) => !prevIsOpen);
    };

    const obj = useMemo(() => ({ isDrawerOpen, toggleDrawer }), [isDrawerOpen])

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
