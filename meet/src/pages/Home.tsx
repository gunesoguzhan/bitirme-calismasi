import { useState } from 'react'
import Header from '../components/layout/header'
import Navbar from '../components/layout/navbar'

export default function Home() {
  const [activeComponent, setActiveComponent] = useState('Activities')

  return (
    <div className='flex flex-col md:flex-row-reverse h-full'>
      <div className="flex flex-col basis-full">
        <Header/>
        <div className="basis-full">
          
        </div>
      </div>
      <Navbar activeLink={activeComponent} setActiveLink={setActiveComponent}/>
    </div>
  )
}