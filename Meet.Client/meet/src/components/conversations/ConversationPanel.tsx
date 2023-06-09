import { ConversationModel } from '../../types/ConversationModel'
import { ConversationItem } from './ConversationItem'

export function ConversationPanel(props: ConversationPanelProps) {
    return (
        <div className={'flex flex-col overflow-hidden h-full pb-[10px] md:basis-[500px] bg-[#0a0c14]' + (props.isActive ? '' : ' hidden md:flex')}>
            <div className='h-[60px] p-4 text-xl'>
                {/* <button type='button' className='bg-[#190d30] rounded-lg px-4 py-2 hover:bg-slate-800 active:bg-slate-600 outline-none transition-all ease-out duration-150'>
                    <span className='bg-contain bg-center bg-no-repeat p-3 pt-[10px]' style={{ backgroundImage: 'url(/icons/plus-light.png)' }}></span>
                    <span className='py-4'>New</span>
                </button> */}
                <div>Last Messages</div>
            </div>
            <ul className='basis-full overflow-y-auto'>
                {props.conversations?.map(x => {
                    return (
                        <li key={x.id} className='mt-2'>
                            <ConversationItem href={`/messages/${x.id}`} conversation={x} />
                        </li>
                    )
                })}
            </ul>
        </div>

    )
}

type ConversationPanelProps = {
    isActive: boolean
    conversations?: ConversationModel[]
}
