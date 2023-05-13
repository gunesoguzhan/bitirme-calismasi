import { Route, Routes } from 'react-router-dom'
import { Home } from './pages/Home'
import { Login } from './pages/Login'
import { Messages } from './pages/Messages'
import { ProtectedRoute } from './components/routes/ProtectedRoute'

export function App() {
    return (
        <Routes>
            <Route path='/signin' element={<Login />} />
            <Route element={<ProtectedRoute />}>
                <Route path='/' element={<Home />} />
                <Route path='/messages' element={<Messages />} />
                <Route path='/messages/:conversationId' element={<Messages />} />
            </Route>
        </Routes>
    )
}