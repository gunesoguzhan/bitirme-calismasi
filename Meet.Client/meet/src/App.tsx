import { Route, Routes } from 'react-router-dom'
import { LoginPage } from './pages/LoginPage'
import { RegisterPage } from './pages/RegisterPage'
import { VideoPage } from './pages/VideoPage'
import { HomePage } from './pages/HomePage'
import { MessagePage } from './pages/MessagePage'
import { FriendPage } from './pages/FriendPage'
import { ProtectedRoute } from './components/routes/ProtectedRoute'
import { CallingPage } from './pages/CallingPage'

export function App() {
    return (
        <Routes>
            <Route path='/login' element={<LoginPage />} />
            <Route path='/register' element={<RegisterPage />} />
            <Route element={<ProtectedRoute />}>
                <Route path='/meet/:meetingId' element={<VideoPage />} />
                <Route path='/' element={<HomePage />} />
                <Route path='/messages' element={<MessagePage />} />
                <Route path='/messages/:conversationId' element={<MessagePage />} />
                <Route path='/friends' element={<FriendPage />} />
                <Route path='/call' element={<CallingPage />} />
                {/* <Route path='/calls' element={<CallPage />} /> */}
            </Route>
        </Routes>
    )
}