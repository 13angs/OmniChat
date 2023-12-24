import Chat from "./pages/chat/Chat";

const AppRoutes = [
  {
    index: true,
    element: <Chat />
  },
  {
    path: '/chat',
    element: <Chat />
  }
];

export default AppRoutes;
