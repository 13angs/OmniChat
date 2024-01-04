import ChatPage from "./pages/chat/chat";
import FriendsPage from "./pages/friend/friend";
import LoginPage from "./pages/login/login";
import WelcomePage from "./pages/welcome/welcome";
import MainContainer from "./containers/main/mainContainer";

const AppRoutes = [
  {
    index: true,
    element: <MainContainer><ChatPage /></MainContainer>
  },
  {
    path: '/chat',
    element: <MainContainer><ChatPage /></MainContainer>
  },
  {
    path: '/login',
    element: <LoginPage />
  },
  {
    path: '/welcome',
    element: <WelcomePage />
  },
  {
    path: '/friends',
    element: <MainContainer><FriendsPage /></MainContainer>
  }
];

export default AppRoutes;
