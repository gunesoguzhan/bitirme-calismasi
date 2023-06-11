import { useContext, useEffect, useState } from 'react'
import { SocketContext } from '../../contexts/SocketContext'
import { MessageModel } from '../../types/MessageModel'
import { MessagePopup } from '../messages/MessagePopup'

export function SocketEvents(props: SocketEventPropsTypes) {
  const [showPopup, setShowPopup] = useState(false)
  const [message, setMessage] = useState<MessageModel>()
  const socket = useContext(SocketContext)

  useEffect(() => {
    let popupTimer: NodeJS.Timeout | null = null

    const handleReceivedMessage = (message: MessageModel) => {
      setShowPopup(true)
      setMessage(message)

      if (popupTimer) {
        clearTimeout(popupTimer)
      }

      popupTimer = setTimeout(() => {
        setShowPopup(false)
      }, 3000)
    }

    socket?.on('message:received', handleReceivedMessage)

    return () => {
      if (popupTimer) {
        clearTimeout(popupTimer)
      }

      socket?.off('message:received', handleReceivedMessage)
    }
  }, [socket])

  return (
    <div>
      {showPopup && <MessagePopup message={message} />}
      {props.children}
    </div>
  )
}

type SocketEventPropsTypes = {
  children: React.ReactNode
}
