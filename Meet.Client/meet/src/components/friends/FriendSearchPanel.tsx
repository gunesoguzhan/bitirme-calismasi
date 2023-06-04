import { SubmitHandler, useForm } from 'react-hook-form'
import { UserModel } from '../../types/UserModel'
import axiosInstance from '../../axiosInstance'
import { useState, useContext } from 'react'
import { FriendshipItem } from './FriendshipItem'
import { SocketContext } from '../../contexts/SocketContext'
import { AuthContext } from '../../contexts/AuthContext'

export function FriendSearchPanel(props: FriendSearchPanelProps) {
    const [users, setUsers] = useState<UserModel[]>([])
    const { register, handleSubmit, reset } = useForm<SearchModel>()
    const socket = useContext(SocketContext)
    const authContext = useContext(AuthContext)
    const sendFriendshipRequest = (user: UserModel) => {
        socket?.emit('friendship:sendRequest', authContext?.user, user)
    }
    const onSubmit: SubmitHandler<SearchModel> = async (data) => {
        axiosInstance.get(`/api/friendship/search?searchString=${data.searchString}`)
            .then(response => {
                if (response.status != 200) return
                setUsers(response.data)
            })
        reset()
    }

    return (
        <div className={`flex flex-col basis-full h-full overflow-hidden ${!props.isActive ? 'hidden md:flex' : ''}`}>
            <div className='m-[15px] text-2xl'>
                <button
                    onClick={props.hideSearchPanel}
                    className='outline-none md:hidden rounded-lg hover:bg-slate-800 focus:bg-slate-600 transition-all ease-out duration-150 p-2' >
                    <span
                        className='pb-[3px] px-[20px] py-[0px] h-[10px] bg-contain bg-no-repeat bg-left'
                        style={{ backgroundImage: `url(/icons/back-light.png)` }}>
                    </span>
                    <span className='pr-4'>Search User</span>
                </button>
                <div className='hidden md:block ml-[20px]'>Search User</div>
            </div>
            <form className="flex p-8 md:p-16 lg:px-32" onSubmit={handleSubmit(onSubmit)}>
                <input
                    type="text"
                    className="flex-1 rounded-lg mr-4 px-4 py-2 outline-none bg-slate-800 focus:bg-slate-600 transition-all ease-out duration-150"
                    placeholder="Search User"
                    autoComplete='off'
                    {...register('searchString', {
                        required: true,
                        minLength: 1
                    })} />
                <button
                    type="submit"
                    className="bg-[#190d30] hover:bg-slate-800 rounded-lg outline-none active:bg-slate-600 transition-all ease-out duration-150" >
                    <div className='px-8 py-3 bg-no-repeat bg-center bg-contain' style={{ backgroundImage: "url('/icons/search-light.png')" }}></div>
                </button>
            </form>
            <ul className='basis-full overflow-y-auto m-4 md:mx-16 lg:mx-32'>
                {users?.map(x => {
                    return (
                        <li key={x.id} className="px-4 py-2">
                            <FriendshipItem user={x} >
                                <button className='bg-[#190d30] outline-none rounded-lg hover:bg-slate-800 active:bg-slate-600 transition-all ease-out duration-150 p-2'
                                    onClick={() => sendFriendshipRequest(x)}>
                                    <span
                                        className='pb-[3px] px-[10px] py-[0px] h-[10px] bg-contain bg-no-repeat bg-left'
                                        style={{ backgroundImage: `url(/icons/addFriend-light.png)` }}>
                                    </span>
                                </button>
                            </FriendshipItem>
                        </li>
                    )
                })}
            </ul>
        </div>
    )
}

type FriendSearchPanelProps = {
    isActive: boolean
    hideSearchPanel: React.MouseEventHandler<HTMLButtonElement>
}

interface SearchModel {
    searchString: string
}