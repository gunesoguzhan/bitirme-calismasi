import { useState } from 'react'
import NavbarButton from './navbar-button'

export default function Navbar(props: navbarProps) {
    const [collapsed, setCollapsed] = useState(true)

    return (
        <div
            onMouseEnter={() => setCollapsed(false)}
            onMouseLeave={() => setCollapsed(true)}
            className='flex basis-16 md:hover:basis-60 md:flex-col transition-all ease-out duration-150 bg-[#0a0c14]'>
            <div className="md:basis-20"></div>
            <div className="basis-full">
                <ul className='flex h-full md:flex-col'>
                    <li className="basis-full md:basis-16">
                        <NavbarButton
                            text='Activities'
                            icon='/icons/activities-light.png'
                            collapsed={collapsed}
                            isActive={props.activeLink === 'Activities'}
                            onClick={() => props.setActiveLink('Activities')} />
                    </li>
                    <li className="basis-full md:basis-16">
                        <NavbarButton
                            text='Messages'
                            icon='/icons/messages-light.png'
                            collapsed={collapsed}
                            isActive={props.activeLink === 'Messages'}
                            onClick={() => props.setActiveLink('Messages')} />
                    </li>
                    <li className="basis-full md:basis-16">
                        <NavbarButton
                            text='Calls'
                            icon='/icons/calls-light.png'
                            collapsed={collapsed}
                            isActive={props.activeLink === 'Calls'}
                            onClick={() => props.setActiveLink('Calls')} />
                    </li>
                    <li className="basis-full md:basis-16">
                        <NavbarButton
                            text='Friends'
                            icon='/icons/friends-light.png'
                            collapsed={collapsed}
                            isActive={props.activeLink === 'Friends'}
                            onClick={() => props.setActiveLink('Friends')} />
                    </li>
                </ul>
            </div>
        </div>
    )
}

type navbarProps = {
    activeLink: string
    setActiveLink: React.Dispatch<React.SetStateAction<string>>
}