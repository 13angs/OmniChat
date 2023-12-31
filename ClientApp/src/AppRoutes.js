import Chat from "./pages/chat/Chat";
import Login from "./pages/login/login";

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
  }
];

export default AppRoutes;
