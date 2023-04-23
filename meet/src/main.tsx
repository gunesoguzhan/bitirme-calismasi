import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import VideoRoom from './pages/VideoRoom.tsx'
import { SocketProvider } from './contexts/SocketContext.tsx'
import Home from './pages/Home.tsx'
import NotFoundError from './pages/NotFoundError.tsx'

const router = createBrowserRouter([
  {
    path: "/",
    element: <Home/>,
    errorElement: <NotFoundError/>
  },
  {
    path: "/meeting",
    element: <VideoRoom/>
  }
]);

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
    <SocketProvider>
      <RouterProvider router={router} /> 
    </SocketProvider>
  </React.StrictMode>,
)
