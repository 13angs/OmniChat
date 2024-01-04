import { Route, Routes } from 'react-router-dom';
import './custom.css';
import AppRoutes from './AppRoutes';

export default function App() {

  return (
    <Routes>
      {AppRoutes.map((route, index) => {
        const { element, ...rest } = route;
        const keyInd = index;
        return <Route key={keyInd} {...rest} element={element} />;
      })}
    </Routes>
  );
}
