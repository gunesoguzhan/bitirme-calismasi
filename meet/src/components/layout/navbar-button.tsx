
export default function NavbarButton(props: NavbarButtonProps) {
    return (
        <div className='w-full h-full'>
            <button
                onClick={props.onClick}
                className={'flex pt-2 rounded-t-lg md:rounded-none md:rounded-r-lg md:pt-0 md:pl-4 text-sm flex-col md:flex-row w-full h-full items-center transition-all ease-in-out duration-150 md:hover:border-l-8' + (props.isActive ? ' bg-teal-800' : '')}>
                <div
                    className="w-6 h-6 bg-cover md:mr-4"
                    style={{ backgroundImage: `url(${props.icon})` }} />
                <span
                    className={props.collapsed ? 'md:hidden' : ''}>
                    {props.text}
                </span>
            </button>
        </div>
    )
}

type NavbarButtonProps = {
    text: string
    icon: string
    collapsed: boolean
    isActive: boolean
    onClick: React.MouseEventHandler<HTMLButtonElement>
}