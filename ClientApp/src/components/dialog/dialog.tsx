import React from 'react';

interface DialogProps {
    show: boolean;
    loading: boolean;
    onConfirm: () => void;
    onCancel: () => void;
}

const Dialog: React.FC<DialogProps> = ({ show, loading, onConfirm, onCancel }) => {
    return (
        <>
            {show && (
                <div className="fixed inset-0 bg-gray-800 bg-opacity-75 flex items-center justify-center">
                    <div className="bg-white p-4 rounded shadow-md">
                        <p className="text-lg font-semibold mb-4">Confirm adding friend?</p>
                        <div className="flex justify-end">
                            <button
                                onClick={onConfirm}
                                disabled={loading}
                                className={`mr-2 bg-blue-500 text-white px-4 py-2 rounded relative ${loading && 'opacity-50 cursor-not-allowed'}`}
                                style={{ minWidth: '100px' }} // Adjust the width as needed
                            >
                                {loading && (
                                    <div className="absolute inset-0 flex items-center justify-center">
                                        <div className="animate-pulse">Adding...</div>
                                    </div>
                                )}
                                {!loading && 'Confirm'}
                            </button>
                            <button
                                onClick={onCancel}
                                disabled={loading}
                                className={`bg-gray-500 text-white px-4 py-2 rounded ${loading && 'opacity-50 cursor-not-allowed'}`}
                            >
                                Cancel
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </>
    );
};

export default Dialog;
