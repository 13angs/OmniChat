import React, { FC, useState } from 'react';

// Define the Props interface for the component
interface MyComponentProps {
  initialCount: number;
}

// Define the component using a functional component (FC) and TypeScript
const MyComponent: FC<MyComponentProps> = ({ initialCount }) => {
  // Use state to manage the count
  const [count, setCount] = useState<number>(initialCount);

  // Function to handle incrementing the count
  const handleIncrement = () => {
    setCount(count + 1);
  };

  // Function to handle decrementing the count
  const handleDecrement = () => {
    setCount(count - 1);
  };

  return (
    <div>
      <h1>Count: {count}</h1>
      <button onClick={handleIncrement}>Increment</button>
      <button onClick={handleDecrement}>Decrement</button>
    </div>
  );
};

export default MyComponent;