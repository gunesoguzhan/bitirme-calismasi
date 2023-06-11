import { useContext, useEffect, useState } from 'react'
import { SocketContext } from '../../contexts/SocketContext'
import { MessageModel } from '../../types/MessageModel'
import { MessagePopup } from '../messages/MessagePopup'
import { CallModel } from '../../types/CallModel'
import { CallPopup } from '../calls/CallPopup'
import { useNavigate } from 'react-router-dom'

export function SocketEvents(props: SocketEventPropsTypes) {
  const [showMessagePopup, setShowMessagePopup] = useState(false)
  const [message, setMessage] = useState<MessageModel>()
  const [showCallPopup, setShowCallPopup] = useState(false)
  const [call, setCall] = useState<CallModel>()
  const socket = useContext(SocketContext)
  const navigate = useNavigate()

  const acceptCall = () => {
    socket?.emit('call:accepted', call)
    navigate(`/meet/${call?.room.id}`)
    setShowCallPopup(false)
  }

  const rejectCall = () => {
    socket?.emit('call:rejected', call)
    setShowCallPopup(false)
  }

  useEffect(() => {
    let popupTimer: NodeJS.Timeout | null = null

    const handleReceivedMessage = (message: MessageModel) => {
      setShowMessagePopup(true)
      setMessage(message)

      if (popupTimer) {
        clearTimeout(popupTimer)
      }

      popupTimer = setTimeout(() => {
        setShowMessagePopup(false)
      }, 3000)
    }

    const handleReceivedCall = (call: CallModel) => {
      console.log(call)
      setShowCallPopup(true)
      setCall(call)
    }

    const handleCancelledCall = () => {
      setShowCallPopup(false)
      setCall(undefined)
    }

    socket?.on('message:received', handleReceivedMessage)
    socket?.on('call:received', handleReceivedCall)
    socket?.on('call:cancelled', handleCancelledCall)

    return () => {
      if (popupTimer) {
        clearTimeout(popupTimer)
      }

      socket?.off('message:received', handleReceivedMessage)
      socket?.off('call:received', handleReceivedCall)
    }
  }, [socket])

  return (
    <div>
      {showMessagePopup && <MessagePopup message={message} />}
      {showCallPopup && <CallPopup call={call} acceptCall={acceptCall} rejectCall={rejectCall} />}
      {props.children}
    </div>
  )
}

type SocketEventPropsTypes = {
  children: React.ReactNode
}
