import ChatPage from "./pages/chat/chat";
import FriendsPage from "./pages/friend/friend";
import LoginPage from "./pages/login/login";
import WelcomePage from "./pages/welcome/welcome";
import MainContainer from "./containers/main/mainContainer";

const AppRoutes = {
  index: {
    index: true,
    element: <MainContainer><ChatPage /></MainContainer>
  },
  chat: {
    path: '/chat',
    element: <MainContainer><ChatPage /></MainContainer>
  },
  login: {
    path: '/login',
    element: <LoginPage />
  },
  welcome: {
    path: '/welcome',
    element: <WelcomePage />
  },
  friend: {
    path: '/friends',
    element: <MainContainer><FriendsPage /></MainContainer>
  }
};
export default AppRoutes;
