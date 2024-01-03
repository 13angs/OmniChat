import Chat from "./pages/chat/Chat";
import FriendsPage from "./pages/friend/Friend";
import Login from "./pages/login/login";
import Welcome from "./pages/welcome/welcome";
import MainContainer from "./containers/main/MainContainer";

const AppRoutes = [
  {
    index: true,
    element: <MainContainer><Chat /></MainContainer>
  },
  {
    path: '/chat',
    element: <MainContainer><Chat /></MainContainer>
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
    element: <MainContainer><FriendsPage /></MainContainer>
  }
];

export default AppRoutes;
