import { NavLink, useNavigate } from 'react-router-dom'
import { AuthContext } from '../contexts/AuthContext'
import { useContext } from 'react'

export function Login() {
    const navigate = useNavigate()

    const { signIn } = useContext(AuthContext)

    const submit = (event: React.FormEvent<HTMLFormElement>) => {
        signIn({ usernameOrEmail: 'string', password: 'string' })
            .then(success => { success && navigate('/') })
        event.preventDefault()
    }

    return (
        <div
            className="min-h-screen flex items-center justify-center"
        >
            <div className="max-w-md w-full p-6 rounded-lg shadow-lg">
                <h1 className="text-3xl font-bold text-center mb-8 border-gray-600 font-bold">
                    Welcome back!
                </h1>
                <form onSubmit={submit}>
                    <div className="mb-4">
                        <input
                            type="email"
                            name="email"
                            id="email"
                            placeholder="you@example.com"
                            className="w-full px-3 py-2 rounded-lg bg-transparent border border-gray-600 text-white focus:border-white focus:outline-none transition duration-200"
                        />
                    </div>
                    <div className="mb-6">
                        <input
                            type="password"
                            name="password"
                            id="password"
                            placeholder="********"
                            className="w-full px-3 py-2 rounded-lg bg-transparent border border-gray-600 text-white focus:border-white focus:outline-none transition duration-200"
                        />
                    </div>
                    <button
                        type="submit"
                        className="bg-[#190d30] rounded-lg py-2 px-4 font-bold hover:bg-slate-800 hover:text-white transition duration-200 w-full">
                        Sign In
                    </button>
                </form>
                <div className="text-center mt-4">
                    <p className="text-gray-600">
                        Don't have an account?{' '}
                        <a href="/signup" className="text-slate-300 hover:text-slate-100">
                            Sign Up
                        </a>
                    </p>
                </div>
                <div className="text-center mt-4">
                    <NavLink to="/forgot" className="text-slate-300 hover:text-slate-100">
                        Forgot Password?
                    </NavLink>
                </div>
            </div>
        </div >

    )
}