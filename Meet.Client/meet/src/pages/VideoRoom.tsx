import { useState } from 'react'

export const VideoRoom = () => {
    const [isChatOpen, setIsChatOpen] = useState(false)
    const [peers, setPeers] = useState(['900', '600', '600'])
    const toggleChat = () => {
        setIsChatOpen(!isChatOpen)
    }

    const setRemoteVideoStyle = () => {

        return ({
            1: 'col-span-12 min-[560px]:col-span-6 row-span-6'
        })[peers.length] || 'col-span-6 row-span-6'
    }

    const setLocalVideoStyle = () => {
        return ({
            0: ' col-span-12 row-span-12',
            1: ' col-span-12 min-[560px]:col-span-6 row-span-6',
            2: ' col-span-6 row-span-6 col-start-4'
        })[peers.length] || ' col-span-6 row-span-6'
    }

    return (
        <div className='w-screen h-screen flex'>
            {/* video panel */}
            <div className={`flex flex-col ${!isChatOpen ? 'basis-full' : 'hidden'}`}>
                {/* video grid */}
                <div className={`basis-full grid grid-cols-12 grid-rows-12`}>
                    {/* videos */}
                    {peers.map(p => (
                        <div className={setRemoteVideoStyle()}>
                            <video className='object-cover w-full h-full' autoPlay />
                        </div>
                    ))}
                    <div className={setLocalVideoStyle()}>
                        <video id="localVideo" className='object-cover w-full h-full' autoPlay />
                    </div>
                </div>
                {/* video controls */}
                <div className='bg-red-900 basis-[64px] flex p-2'>
                    <div className='basis-full flex'>
                        {/* cam */}
                        <div className='bg-blue-600 basis-14 mx-4'>
                        </div>
                        {/* mic */}
                        <div className='bg-blue-900 basis-14'>
                        </div>
                    </div>
                    <div className='basis-full flex justify-end'>
                        {/* show message */}
                        <div className='bg-blue-600 basis-14 lg:hidden'>
                        </div>
                        {/* leave room */}
                        <div className='bg-blue-900 basis-14 mx-4'>
                        </div>
                    </div>
                </div>
            </div>
            {/* chat panel */}
            <div className={`lg:basis-96 bg-blue-900 flex flex-col ${isChatOpen ? 'basis-full' : ''}`}>
                {/* chat header */}
                <div className='bg-blue-700 basis-[73.5px]'>

                </div>
                {/* chat grid */}
                <div className='h-full bg-blue-500'>

                </div>
                {/* chat input */}
                <div className='basis-[73.5px] bg-blue-300'>

                </div>
            </div>
        </div>
    )
}
