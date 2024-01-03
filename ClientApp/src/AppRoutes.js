import Chat from "./pages/chat/Chat";
import FriendsPage from "./pages/friend/Friend";
import Login from "./pages/login/login";
import Welcome from "./pages/welcome/welcome";

const AppRoutes = [
  {
    index: true,
    element: <Chat />
  },
  {
    path: '/chat',
    element: <Chat />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/welcome',
    element: <Welcome />
  },
  {
    path: '/friends',
    element: <FriendsPage />
  }
];

export default AppRoutes;
