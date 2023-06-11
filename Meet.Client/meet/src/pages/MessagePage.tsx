import { useNavigate, useParams } from 'react-router-dom'
import { ConversationPanel } from '../components/conversations/ConversationPanel'
import { MessagePanel } from '../components/messages/MessagePanel'
import { MainLayoutProvider } from '../contexts/MainLayoutContext'
import { useContext, useEffect, useState } from 'react'
import { ConversationModel } from '../types/ConversationModel'
import axiosInstance from '../axiosInstance'
import { SocketContext } from '../contexts/SocketContext'
import { MessageModel } from '../types/MessageModel'

export function MessagePage() {
  const params = useParams<{ conversationId?: string }>()
  const [conversations, setConversations] = useState<ConversationModel[]>()
  const navigate = useNavigate()
  const socket = useContext(SocketContext)

  useEffect(() => {
    axiosInstance.get("/api/conversations")
      .then(response => setConversations(response.data))
  }, [])

  useEffect(() => {
    const updateLastMessage = (message: MessageModel) => {
      const conversation = conversations?.find(x => x.room.id === message.room.id)
      if (!conversation) return
      conversation.lastMessage = message
      const conv = conversations?.filter(x => x.room.id !== message.room.id)
      conv?.unshift(conversation)
      setConversations(conv)
    }

    socket?.on('message:received', updateLastMessage)
    return (() => {
      socket?.off('message:received', updateLastMessage)
    })
  }, [conversations, socket])

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