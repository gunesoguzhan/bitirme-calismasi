import { NavLink } from 'react-router-dom'
import { SubmitHandler, useForm } from 'react-hook-form'
import { LoginUserModel } from '../types/LoginUserModel'
import { useContext } from 'react'
import { AuthContext } from '../contexts/AuthContext'

export function LoginPage() {

    const authContext = useContext(AuthContext)
    const { register, handleSubmit, formState: { errors } } = useForm<LoginUserModel>()
    const onSubmit: SubmitHandler<LoginUserModel> = async (data) => {
        await authContext?.login(data)
    }

    return (
        <div className="min-h-screen flex items-center justify-center">
            <div className="max-w-md w-full p-6 rounded-lg shadow-lg">
                <h1 className="text-3xl text-center mb-8 border-gray-600 font-bold">
                    Welcome back!
                </h1>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div>
                        <input type='text' placeholder='Username or email'
                            className={"w-full px-3 py-2 rounded-lg bg-transparent border  text-white  focus:border-white focus:outline-none transition duration-200" + (errors.usernameOrEmail == undefined ? " border-gray-600" : " border-red-600")}
                            {...register('usernameOrEmail', {
                                required: {
                                    value: true,
                                    message: 'Required'
                                }, pattern: {
                                    value: /^[a-zA-Z0-9_-]{3,30}$|^([a-zA-Z0-9._%+-]+)@([a-zA-Z0-9.-]+.[a-zA-Z]{2,})$/,
                                    message: 'Enter an username or email.'
                                }
                            })} />
                        <p className='text-sm text-red-600 h-6'>{errors.usernameOrEmail?.message}</p>
                    </div>
                    <div>
                        <input type='password' placeholder='Password'
                            className={"w-full px-3 py-2 rounded-lg bg-transparent border  text-white  focus:border-white focus:outline-none transition duration-200" + (errors.password == undefined ? " border-gray-600" : " border-red-600")}
                            {...register('password', {
                                required: {
                                    value: true,
                                    message: 'Required'
                                }, pattern: {
                                    value: /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/,
                                    message: 'Enter one letter, one number and 8 characters at least.'
                                }
                            })} />
                        <p className='text-sm text-red-600 h-6'>{errors.password?.message}</p>
                    </div>
                    <button type="submit"
                        className="bg-[#190d30] rounded-lg py-2 px-4 font-bold hover:bg-slate-800 hover:text-white transition duration-200 w-full">
                        Log In
                    </button>
                </form>
                <div className="text-center mt-4">
                    <p className="text-gray-600">
                        Don't have an account?{' '}
                        <NavLink to="/register" className="text-slate-300 hover:text-slate-100">
                            Register
                        </NavLink>
                    </p>
                </div>
                <div className="text-center mt-4">
                    <NavLink to="/forgot" className="text-slate-300 hover:text-slate-100">
                        Forgot Password?
                    </NavLink>
                </div>
            </div>
        </div>
    )
}