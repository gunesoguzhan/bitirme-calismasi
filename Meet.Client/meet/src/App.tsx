import { Route, Routes } from 'react-router-dom'
import { Home } from './pages/Home'
import { Login } from './pages/Login'
import { Messages } from './pages/Messages'
import { ProtectedRoute } from './components/routes/ProtectedRoute'
import { Register } from './pages/Register'
import { Friends } from './pages/Friends'

export function App() {
    return (
        <Routes>
            <Route path='/login' element={<Login />} />
            <Route path='/register' element={<Register />} />
            <Route element={<ProtectedRoute />}>
                <Route path='/' element={<Home />} />
                <Route path='/messages' element={<Messages />} />
                <Route path='/messages/:conversationId' element={<Messages />} />
                <Route path='/friends' element={<Friends />} />
            </Route>
        </Routes>
    )
}