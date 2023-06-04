import { UserModel } from '../../types/UserModel'

export function FriendshipItem(props: FriendItemPropTypes) {
    return (
        <div className="flex w-full text-left outline-none transition-all ease-out duration-150 p-1 px-4">
            <div className={"bg-slate-800 flex items-center justify-center h-12 w-12 rounded-lg text-white text-xl"}>
                {`${props.user.firstName.charAt(0).toLocaleUpperCase()}${props.user.lastName.charAt(0).toLocaleUpperCase()}`}
            </div>
            <div className="flex-1 overflow-hidden pl-3">
                <div className="text-lg truncate">{`${props.user.firstName} ${props.user.lastName}`}</div>
            </div>
            <div className="flex flex-col justify-center text-sm text-right pr-2">
                <div>
                    {props.children}
                </div>
            </div>
        </div >
    )
}

type FriendItemPropTypes = {
    user: UserModel
    children: React.ReactNode
}