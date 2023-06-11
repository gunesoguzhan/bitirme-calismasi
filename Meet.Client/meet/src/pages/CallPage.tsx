import { CallPanel } from '../components/calls/CallPanel'
import { MainLayoutProvider } from '../contexts/MainLayoutContext'

export function CallPage() {
    return (
        <MainLayoutProvider>
            <CallPanel />
        </MainLayoutProvider>
    )
}