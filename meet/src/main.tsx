import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import { SocketProvider } from './contexts/socket-context.tsx'
import Home from './pages/home.tsx'

const router = createBrowserRouter([
  {
    path: "/",
    element: <Home />,
  }
])

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <React.StrictMode>
      <SocketProvider>
        <RouterProvider router={router} />
      </SocketProvider>
  </React.StrictMode>
)
