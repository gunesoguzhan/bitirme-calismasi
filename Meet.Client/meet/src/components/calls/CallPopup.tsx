import { CallModel } from '../../types/CallModel'

export function CallPopup(props: CallPopupPropTypes) {
    return (
        <div className="fixed h-full w-full flex flex-col justify-center items-center bg-[#190d30] bottom-0 right-0 md:h-96 md:w-96 md:mb-4 md:mr-4 md:rounded-lg">
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
            <h1 className="text-white text-4xl font-bold">{`${props.call?.caller.firstName} ${props.call?.caller.lastName}`}</h1>
            <p className="text-gray-300 text-lg">Incoming Call...</p>
            <div className="flex mt-8">
                <button className="bg-cyan-700 hover:bg-cyan-600 active:bg-cyan-500 text-white py-2 px-4 rounded"
                    onClick={() => props.acceptCall()}>
                    Answer
                </button>
                <button className="bg-red-500 hover:bg-red-600 active:bg-red-700 text-white py-2 px-4 rounded ml-4"
                    onClick={() => props.rejectCall()}>
                    Reject
                </button>
            </div>
        </div>
    )
}

type CallPopupPropTypes = {
    call?: CallModel
    acceptCall: () => void
    rejectCall: () => void
}