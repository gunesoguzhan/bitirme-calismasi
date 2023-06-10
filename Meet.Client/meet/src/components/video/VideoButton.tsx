export function VideoButton(props: VideoButtonPropsTypes) {
    return (
        <button className={`bg-[#190d30] outline-none rounded-lg hover:bg-slate-800 active:bg-slate-600 transition-all ease-out duration-150 px-4 mx-2 ${props.style}`}
            onClick={props.onClick}>
            <span
                className='pb-[3px] px-[10px] py-[0px] h-[10px] bg-contain bg-no-repeat bg-left'
                style={{ backgroundImage: `url(${props.backgroundImage})` }}>
            </span>
        </button>
    )
}

type VideoButtonPropsTypes = {
    backgroundImage: string
    onClick: React.MouseEventHandler<HTMLButtonElement>
    style?: string
}