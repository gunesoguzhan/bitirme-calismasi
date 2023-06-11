import { CallModel } from '../../types/CallModel'

export function CallItem(props: CallItemPropTypes) {
    return (
        <div className="flex w-full text-left outline-none transition-all ease-out duration-150 p-1 px-4">
            <div className={"bg-slate-800 flex items-center justify-center h-12 w-12 rounded-lg text-white text-xl"}>
                {`${props.call.caller.firstName.charAt(0).toLocaleUpperCase()}${props.call.caller.lastName.charAt(0).toLocaleUpperCase()}`}
            </div>
            <div className="flex-1 overflow-hidden pl-3">
                <div className="text-lg truncate">{`${props.call.caller.firstName} ${props.call.caller.lastName}`}</div>
            </div>
        </div >
    )
}

type CallItemPropTypes = {
    call: CallModel
}