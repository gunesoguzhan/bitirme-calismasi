import { useParams } from 'react-router-dom'
import { ConversationPanel } from '../components/conversations/ConversationPanel'
import { MessagePanel } from '../components/messages/MessagePanel'
import { MainLayoutProvider } from '../contexts/MainLayoutContext'

export function Messages() {
  const params = useParams<{ conversationId?: string }>()

  return (
    <MainLayoutProvider>
      {params.conversationId ?
        <MessagePanel conversationId={params.conversationId} conversationTitle='Title' />
        :
        <div className='hidden md:flex basis-full h-full overflow-hidden '></div>}
      <ConversationPanel isActive={params.conversationId ? false : true} />
    </MainLayoutProvider>
  )
}