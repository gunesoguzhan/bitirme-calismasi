import { NavLink, useNavigate } from 'react-router-dom'
import { useForm, SubmitHandler } from "react-hook-form"
import { RegisterUserModel } from '../types/RegisterUserModel'
import axiosInstance from '../axiosInstance'

export function Register() {
    const navigate = useNavigate()
    const { register, handleSubmit, watch, formState: { errors } } = useForm<RegisterUserModel>()
    const onSubmit: SubmitHandler<RegisterUserModel> = data => {
        var abortController = new AbortController()
        axiosInstance.post("/api/auth/register", data, { signal: abortController.signal })
            .then(response => {
                if (response.status !== 200)
                    return
                navigate('/')
            })
        return (() => { abortController.abort() })
    }

    return (
        <div className="min-h-screen flex items-center justify-center">
            <div className="max-w-md w-full p-6 rounded-lg shadow-lg">
                <h1 className="text-3xl font-bold text-center mb-8 border-gray-600 font-bold">
                    Welcome to Meet!
                </h1>
                <form onSubmit={handleSubmit(onSubmit)}>
                    <div>
                        <input type='text' placeholder='First name'
                            className={"w-full px-3 py-2 rounded-lg bg-transparent border  text-white  focus:border-white focus:outline-none transition duration-200" + (errors.firstName == undefined ? " border-gray-600" : " border-red-600")}
                            {...register('firstName', {
                                required: {
                                    value: true,
                                    message: 'Required'
                                }, pattern: {
                                    value: /^[a-zA-Z]{2,30}$/,
                                    message: 'Enter a valid name.'
                                }
                            })} />
                        <p className='text-sm text-red-600 h-6'>{errors.firstName?.message}</p>
                    </div>
                    <div>
                        <input type='text' placeholder='Last name'
                            className={"w-full px-3 py-2 rounded-lg bg-transparent border  text-white  focus:border-white focus:outline-none transition duration-200" + (errors.lastName == undefined ? " border-gray-600" : " border-red-600")}
                            {...register('lastName', {
                                required: {
                                    value: true,
                                    message: 'Required'
                                }, pattern: {
                                    value: /^[a-zA-Z]{2,30}$/,
                                    message: 'Enter a valid name.'
                                }
                            })} />
                        <p className='text-sm text-red-600 h-6'>{errors.lastName?.message}</p>
                    </div>
                    <div>
                        <input type='text' placeholder='Username'
                            className={"w-full px-3 py-2 rounded-lg bg-transparent border  text-white  focus:border-white focus:outline-none transition duration-200" + (errors.username == undefined ? " border-gray-600" : " border-red-600")}
                            {...register('username', {
                                required: {
                                    value: true,
                                    message: 'Required'
                                }, pattern: {
                                    value: /^[a-zA-Z0-9_-]{3,30}$/,
                                    message: 'Enter a valid username.'
                                }
                            })} />
                        <p className='text-sm text-red-600 h-6'>{errors.username?.message}</p>
                    </div>
                    <div>
                        <input type='text' placeholder='Email'
                            className={"w-full px-3 py-2 rounded-lg bg-transparent border  text-white  focus:border-white focus:outline-none transition duration-200" + (errors.email == undefined ? " border-gray-600" : " border-red-600")}
                            {...register('email', {
                                required: {
                                    value: true,
                                    message: 'Required'
                                }, pattern: {
                                    value: /^([a-zA-Z0-9._%+-]+)@([a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$/,
                                    message: 'Enter a valid email.'
                                }
                            })} />
                        <p className='text-sm text-red-600 h-6'>{errors.email?.message}</p>
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
                    <div>
                        <input type='password' placeholder='Confirm password'
                            className={"w-full px-3 py-2 rounded-lg bg-transparent border  text-white  focus:border-white focus:outline-none transition duration-200" + (errors.confirmPassword == undefined ? " border-gray-600" : " border-red-600")}
                            {...register('confirmPassword', {
                                required: {
                                    value: true,
                                    message: 'Required'
                                }, validate: (val: string) => {
                                    if (watch('password') != val)
                                        return "Passwords do not match."
                                }
                            })} />
                        <p className='text-sm text-red-600 h-6'>{errors.confirmPassword?.message}</p>
                    </div>
                    <button type="submit"
                        className="bg-[#190d30] rounded-lg py-2 px-4 font-bold hover:bg-slate-800 hover:text-white transition duration-200 w-full">
                        Register
                    </button>
                </form>
                <div className="text-center mt-4">
                    <p className="text-gray-600">
                        Already have an account?{' '}
                        <NavLink to="/login" className="text-slate-300 hover:text-slate-100">
                            Log in
                        </NavLink>
                    </p>
                </div>
            </div>
        </div>
    )
}