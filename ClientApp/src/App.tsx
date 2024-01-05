import { Route, Routes } from 'react-router-dom';
import './custom.css';
import AppRoutes from './AppRoutes';

export default function App() {

  return (
    <Routes>
      <Route index={AppRoutes.index.index} element={AppRoutes.index.element} />
      <Route path={AppRoutes.chat.path} element={AppRoutes.chat.element} />
      <Route path={AppRoutes.login.path} element={AppRoutes.login.element} />
      <Route path={AppRoutes.welcome.path} element={AppRoutes.welcome.element} />
      <Route path={AppRoutes.friend.path} element={AppRoutes.friend.element} />
    </Routes>
  );
}
