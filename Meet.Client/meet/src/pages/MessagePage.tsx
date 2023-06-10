import { useNavigate, useParams } from 'react-router-dom'
import { ConversationPanel } from '../components/conversations/ConversationPanel'
import { MessagePanel } from '../components/messages/MessagePanel'
import { MainLayoutProvider } from '../contexts/MainLayoutContext'
import { useEffect, useState } from 'react'
import { ConversationModel } from '../types/ConversationModel'
import axiosInstance from '../axiosInstance'

export function MessagePage() {
  const params = useParams<{ conversationId?: string }>()
  const [conversations, setConversations] = useState<ConversationModel[]>()
  const navigate = useNavigate()

  useEffect(() => {
    axiosInstance.get("/api/conversations")
      .then(response => setConversations(response.data))
  }, [])

  return (
    <MainLayoutProvider>
      {params.conversationId ?
        <MessagePanel conversationId={params.conversationId} conversationTitle={conversations?.find(x => x.id == params.conversationId)?.room.title} style='basis-full' onClose={() => navigate('/messages')} />
        :
        <div className='hidden md:flex basis-full h-full overflow-hidden '></div>}
      <ConversationPanel isActive={params.conversationId ? false : true} conversations={conversations} />
    </MainLayoutProvider>
  )
}