import { useEffect, useState } from 'react'
import { CallModel } from '../../types/CallModel'
import { CallItem } from './CallItem'

export function CallPanel() {
    const [calls, setCalls] = useState<CallModel[]>()

    useEffect(() => {
        setCalls([
            {
                id: '1',
                caller: {
                    id: '1',
                    firstName: 'ali',
                    lastName: 'veli'
                },
                date: new Date(),
                room: {
                    id: '1',
                    title: 'room1'
                }
            },
            {
                id: '2',
                caller: {
                    id: '2',
                    firstName: 'user',
                    lastName: 'two'
                },
                date: new Date(),
                room: {
                    id: '1',
                    title: 'room2'
                }
            }])
    }, [])

    return (
        <div className='flex flex-col overflow-hidden h-full pb-[10px] md:basis-[500px] bg-[#0a0c14]' >
            <div className='h-[60px] text-center p-3'>


            </div>
            <ul className='basis-full overflow-y-auto'>
                {calls?.map((x, i) => {
                    return (
                        <li key={i} className='mt-2'>
                            <CallItem call={x} />
                        </li>
                    )
                })}
            </ul>
        </ div >
    )
}
