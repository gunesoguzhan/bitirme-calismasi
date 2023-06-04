import { useContext, useState } from 'react'
import { NavbarItem } from './NavbarItem'
import { AuthContext } from '../../contexts/AuthContext'

export function Navbar(props: NavbarProps) {
    const [collapsed, setCollapsed] = useState(true)
    const authContext = useContext(AuthContext)

    return (
        <div
            onMouseEnter={() => setCollapsed(false)}
            onMouseLeave={() => setCollapsed(true)}
            className={'flex min-w-[60px] md:hover:min-w-[200px] md:flex-col transition-all ease-out duration-150 bg-[#190d30] md:h-screen' + (props.isNavbarHidden && ' hidden md:flex')} >
            <div className="md:basis-48"></div>
            <ul className='basis-full flex md:flex-col md:basis-0'>
                {/* <li className="basis-full md:basis-[60px]">
                    <NavbarItem
                        text='Activities'
                        icon='/icons/activities-light.png'
                        href='/activities'
                        collapsed={collapsed}/>
                </li> */}
                <li className="basis-full md:basis-[60px]">
                    <NavbarItem
                        text='Messages'
                        icon='/icons/messages-light.png'
                        href='/messages'
                        collapsed={collapsed} />
                </li>
                <li className="basis-full md:basis-[60px]">
                    <NavbarItem
                        text='Calls'
                        icon='/icons/calls-light.png'
                        href='/calls'
                        collapsed={collapsed} />
                </li>
                <li className="basis-full md:basis-[60px]">
                    <NavbarItem
                        text='Friends'
                        icon='/icons/friends-light.png'
                        href='/friends'
                        collapsed={collapsed} />
                </li>
            </ul>
            <div className="md:basis-full"></div>
            <ul className='basis-48 md:basis-0 flex md:flex-col md:basis-0'>
                <li className="basis-full md:basis-[60px]">
                    <NavbarItem
                        text={`${authContext?.user?.firstName} ${authContext?.user?.lastName}`}
                        icon='/icons/user-light.png'
                        href={`/user/${authContext?.user?.id}`}
                        collapsed={collapsed} />
                </li>
            </ul>
        </div>
    )
}

type NavbarProps = {
    isNavbarHidden?: boolean
}