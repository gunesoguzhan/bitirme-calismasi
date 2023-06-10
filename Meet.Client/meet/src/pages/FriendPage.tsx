import { MainLayoutProvider } from '../contexts/MainLayoutContext'
import { useState } from 'react'
import { FriendshipPanel } from '../components/friends/FriendshipPanel'
import { FriendSearchPanel } from '../components/friends/FriendSearchPanel'

export function FriendPage() {
  const [showSearchPanel, setShowSearchPanel] = useState<boolean>(false)
  return (
    <MainLayoutProvider>
      <FriendSearchPanel
        isActive={showSearchPanel}
        hideSearchPanel={() => setShowSearchPanel(false)} />
      <FriendshipPanel
        isActive={!showSearchPanel}
        showSearchPanel={() => setShowSearchPanel(true)} />
    </MainLayoutProvider>
  )
}