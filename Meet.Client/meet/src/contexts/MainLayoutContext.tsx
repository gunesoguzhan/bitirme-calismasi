import { createContext, useState } from 'react'
import { Navbar } from '../components/navbar/Navbar'

interface MainLayoutValue {
    hideNavbar: () => void
    showNavbar: () => void
}

export const MainLayoutContext = createContext<MainLayoutValue>({
    hideNavbar: () => { return }, showNavbar: () => { return }
})

export function MainLayoutProvider(props: MainLayoutProviderProps) {
    const [isNavbarHidden, setIsNavbarHidden] = useState(false)

    const hideNavbar = () => { setIsNavbarHidden(true) }
    const showNavbar = () => { setIsNavbarHidden(false) }

    const contextValue: MainLayoutValue = {
        hideNavbar: hideNavbar,
        showNavbar: showNavbar
    }

    return (
        <MainLayoutContext.Provider value={ contextValue }>
            <div className='flex flex-col w-screen h-screen md:flex-row-reverse md:justify-end'>
                {props.children}
                <Navbar isNavbarHidden={isNavbarHidden} />
            </div>
        </MainLayoutContext.Provider>
    )
}

type MainLayoutProviderProps = {
    children: React.ReactNode
}