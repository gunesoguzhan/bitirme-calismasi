import { MainLayoutProvider } from '../contexts/MainLayoutContext'

export function Home() {
    return (
        <MainLayoutProvider>
            <div></div>
        </MainLayoutProvider>
    )
}