import { useContext, useEffect } from 'react'
import { SocketContext } from '../contexts/SocketContext'
import { useNavigate, useSearchParams } from 'react-router-dom'

export function CallingPage() {
    const navigate = useNavigate()
    const socket = useContext(SocketContext)
    const [searchParams] = useSearchParams()

    useEffect(() => {
        const callAccepted = () => {
            navigate(`/meet/${searchParams.get('meetingId')}`)
        }

        const callRejected = () => {
            navigate('/')
        }
        socket?.on('call:accepted', callAccepted)
        socket?.on('call:rejected', callRejected)

        return (() => {
            socket?.on('call:accepted', callAccepted)
            socket?.on('call:rejected', callRejected)
        })
    }, [navigate, searchParams, socket])

    return (
        <div className="fixed h-full w-full flex flex-col justify-center items-center bg-[#190d30]">
            <div className="p-4 bg-white rounded-full w-32 h-32 flex items-center justify-center mb-8">
                <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                    className="h-12 w-12 text-gray-800"
                >
                    <path
                        strokeLinecap="round"
                        strokeLinejoin="round"
                        strokeWidth={2}
                        d="M13 10V3L4 14h7v7l9-11h-7z"
                    />
                </svg>
            </div>
            <h1 className="text-white text-4xl font-bold">{searchParams.get('title')}</h1>
            <p className="text-gray-300 text-lg">Calling...</p>
            <div className="flex mt-8">
                <button className="bg-cyan-700 hover:bg-cyan-600 active:bg-cyan-500 text-white py-2 px-4 rounded"
                    onClick={() => {
                        socket?.emit('call:cancelled', searchParams.get('meetingId'))
                        navigate('/')
                    }}>
                    Cancel
                </button>
            </div>
        </div>
    )
}