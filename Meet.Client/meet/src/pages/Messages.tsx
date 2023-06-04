import { useParams } from 'react-router-dom'
import { ConversationPanel } from '../components/conversations/ConversationPanel'
import { MessagePanel } from '../components/messages/MessagePanel'
import { MainLayoutProvider } from '../contexts/MainLayoutContext'
import { useEffect, useState } from 'react'
import { ConversationModel } from '../types/ConversationModel'
import axiosInstance from '../axiosInstance'

export function Messages() {
  const params = useParams<{ conversationId?: string }>()
  const [conversations, setConversations] = useState<ConversationModel[]>()

  useEffect(() => {
    axiosInstance.get("/api/conversations")
      .then(response => setConversations(response.data))
  }, [])

  return (
    <MainLayoutProvider>
      {params.conversationId ?
        <MessagePanel conversationId={params.conversationId} conversationTitle={conversations?.find(x => x.id == params.conversationId)?.room.title} />
        :
        <div className='hidden md:flex basis-full h-full overflow-hidden '></div>}
      <ConversationPanel isActive={params.conversationId ? false : true} conversations={conversations} />
    </MainLayoutProvider>
  )
}