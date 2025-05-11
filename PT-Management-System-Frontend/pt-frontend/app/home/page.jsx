"use client"
import Image from "next/image"
import Link from "next/link"
import React, { useState } from "react"
import { Facebook, Instagram, Twitter ,LogOut,CircleUser,CircleX} from "lucide-react"
import styles from "../styles/home.module.css";
import { signOut,useSession } from "next-auth/react";
import { useRouter } from 'next/navigation';


export default function Home() {
  const [error, setError] = useState(null);

  const [trainer, setTrainer] = useState("");
  const [frequency, setFrequency] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [slots, setSlots] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [selectedSlot, setSelectedSlot] = useState(null);
  const [changeActive,setChangeActive]=useState(false);
  const router = useRouter();
  const { data: session } = useSession();  

  console.log("Session:", session);
  const trainers = [
    {
      id: "1",
      name: "JAMIE WARRING",
      profession: "Fitness Trainer",
      imageUrl: "/trainer_jamie.jpg",
    },
    {
      id: "2",
      name: "MARK",
      profession: "Fitness Trainer",
      imageUrl: "/mark.jpg",
    },
    {
      id: "3",
      name: "RAPHAEL",
      profession: "Fitness Trainer",
      imageUrl: "/raphael.jpeg",
    },
  ];

  const makeReservation =async (e) => {
    e.preventDefault();
    console.log("slot",slots)
    console.log("slot id",selectedSlot.slot_id)
    if (!selectedSlot.slot_id) {
      alert("slot id not found.");
      return;
    }
    console.log(" id",session?.user?.id)
    try {
      const response = await fetch("http://localhost:8004/make_reservation/make_reservation", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          slot_id: selectedSlot.slot_id,
          user_id: session?.user?.id,
          timestamp:new Date().toISOString(),
        }),
      });

      const data = await response.json();
      alert("An appointment has been made.");

      router.push("/payment")

      setShowModal(false);
      setChangeActive(false);
      setSelectedSlot(null);
      
    } catch (error) {
      console.error("Error fetching slots:", error);
      alert("Rezervasyon sırasında hata oluştu.");
      
    }
  };



  
  const handleSubmit =async (e) => {
    e.preventDefault();
    const selectedTrainer = trainers.find((t) => t.name === trainer);

    if (!selectedTrainer) {
      alert("Trainer not found.");
      return;
    }
    
    try {
      const response = await fetch("http://localhost:8004/make_reservation/available_slots", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          trainer_id: selectedTrainer.id,
          start_date: startDate,
          end_date:endDate,
        }),
      });

      const data =await response.json();
      setSlots(data || []);
      setShowModal(true);
      console.log(slots)
    } catch (error) {
      console.error("Error fetching slots:", error);
      setSlots([]);
      setShowModal(true);
    }
  };



  const handleSignOut = () => {
    
    signOut({ redirect: false }).then(() => {
      
      const kcLogout =
      "http://localhost:8080/realms/ptmanagement/protocol/openid-connect/logout" +
      "?post_logout_redirect_uri=" +
      encodeURIComponent("http://localhost:3000") +
      "&client_id=nextjs-app";

  
      window.location.href = kcLogout;
    });
  };
 
  return (
    <div className="flex min-h-screen flex-col">
      <header className="bg-black text-white">
        <div className="container mx-auto px-4 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center">
              <Link href="/" className="text-xl font-bold text-white">
                <span className="text-orange-500">DALMS</span>FITNESS
              </Link>
            </div>

            <nav className="hidden md:flex space-x-6">
              <Link href="/" className="text-sm hover:text-orange-500">Home</Link>
              <Link href="/our-trainers" className="text-sm hover:text-orange-500">Our Trainers</Link>
              <Link href="/settings">
                <span className="text-sm hover:text-orange-500">Settings</span>
              </Link>
              <Link href="/contact-us" className="text-sm hover:text-orange-500">Contact Us</Link>
            </nav>

            <div className="flex items-center space-x-6">
              
              <div className="flex items-center space-x-3">
                <Link href="#" className="text-white hover:text-orange-500">
                  <Twitter size={16} />
                </Link>
                <Link href="#" className="text-white hover:text-orange-500">
                  <Instagram size={16} />
                </Link>
              </div>

              
              <div className="flex items-center space-x-4">
                <button
                  type="button"
                  onClick={handleSignOut}
                  className="bg-red-600 p-2 rounded hover:bg-red-800"
                >
                  <LogOut size={20} className="text-white" />
                </button>
                <button className="p-2 hover:text-orange-500">
                  <CircleUser size={20} />
                </button>
              </div>
            </div>
          </div>
        </div>
      </header>



      <div className="bg-black text-white text-center py-2 text-xs">
        <div className="container mx-auto">
          <p>PERSONAL GYM TRAINERS</p>
        </div>
      </div>

      <section className="relative bg-black text-white">
  
  <div className="absolute inset-0 bg-black/70 z-10"></div>
  
  
  <div className="relative h-[500px]">
    <Image
      src="/gym_background.jpg"  
      alt="Gym background"
      layout="fill"  
      objectFit="cover" 
      className="z-0" 
    />
  </div>

  
  <div className="absolute inset-0 flex flex-col items-center justify-center text-center z-20">
    <h1 className="text-4xl md:text-5xl font-bold mb-2">BE READY TO</h1>
    <h2 className="text-4xl md:text-5xl font-bold mb-6">BECOME HEALTHY</h2>
    <p className="max-w-2xl mb-8 text-gray-300">
      A gym is more than a place to work out. It's a place to grow, to push yourself, to discover what you're
      capable of. Join us and transform your life.
    </p>
    <p className="max-w-2xl mb-8 text-gray-300">
      "Discipline, Agility, Lift, Muscle, Strength"
    </p>
    <button className="bg-orange-500 hover:bg-orange-600 text-white font-bold py-3 px-8 rounded-full">
      JOIN NOW
    </button>
  </div>
</section>



      <section className="bg-gray-900 text-white py-16">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold text-center mb-12">OUR PROCESS</h2>
          <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
            <div className="bg-gray-800 p-6 text-center">
              <div className="bg-orange-500 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                <span className="text-white text-2xl font-bold">1</span>
              </div>
              <h3 className="text-lg font-bold mb-2">ANALYZE YOUR GOAL</h3>
            </div>
            <div className="bg-gray-800 p-6 text-center">
              <div className="bg-orange-500 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                <span className="text-white text-2xl font-bold">2</span>
              </div>
              <h3 className="text-lg font-bold mb-2">WORK WITH PLAN</h3>
            </div>
            <div className="bg-gray-800 p-6 text-center">
              <div className="bg-orange-500 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                <span className="text-white text-2xl font-bold">3</span>
              </div>
              <h3 className="text-lg font-bold mb-2">IMPROVE YOURSELF</h3>
            </div>
            <div className="bg-gray-800 p-6 text-center">
              <div className="bg-orange-500 w-16 h-16 rounded-full flex items-center justify-center mx-auto mb-4">
                <span className="text-white text-2xl font-bold">4</span>
              </div>
              <h3 className="text-lg font-bold mb-2">ACHIEVE YOUR FITNESS</h3>
            </div>
          </div>
        </div>
      </section>

      <section className="bg-gray-900 text-white py-16">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold text-center mb-12">OUR TRAINERS</h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            {trainers.map((trainer) => (
              <div key={trainer.id} className="bg-gray-800">
                <div className="h-80 relative">
                  <Image
                    src={trainer.imageUrl}
                    alt={`Trainer ${trainer.name}`}
                    fill
                    className="object-cover"
                  />
                </div>
                <div className="p-4 text-center">
                  <h3 className="text-xl font-bold mb-1">{trainer.name}</h3>
                  <p className="text-sm text-gray-400 mb-3">{trainer.profession}</p>
                  <div className="flex justify-center space-x-2">
    
                    <Link href="#" className="text-white hover:text-orange-500">
                      <Twitter size={16} />
                    </Link>
                    <Link href="#" className="text-white hover:text-orange-500">
                      <Instagram size={16} />
                    </Link>
                  </div>
                </div>
              </div>
            ))}
          </div>

        </div>
      </section>

      <section className="bg-gray-800 text-white py-16 relative">
      <div className="absolute inset-0 bg-black/50 z-10"></div>
      <div className="absolute inset-0 bg-cover bg-center" style={{ backgroundImage: "url('/placeholder.svg?height=500&width=1200')" }}></div>

      <div className="container mx-auto px-4 relative z-20 max-w-2xl bg-black/80 p-8 rounded-xl shadow-lg">
        <h2 className="text-3xl font-bold text-center mb-8 text-orange-500">BOOK A PERSONAL TRAINER</h2>

        <form className="space-y-6" onSubmit={handleSubmit}>
          <div>
            <label className="block text-sm font-medium mb-2">Select Trainer</label>
            <select
                value={trainer}
                onChange={(e) => setTrainer(e.target.value)}
                required
                className="w-full px-4 py-2 rounded-lg bg-gray-900 text-white border border-gray-700 focus:outline-none focus:ring-2 focus:ring-orange-500"
              >
                <option value="">-- Choose a Trainer --</option>
                {trainers.map((t) => (
                  <option key={t.id} value={t.name}>{t.name}</option>
                ))}
              </select>
          </div>

          

          <div>
            <label className="block text-sm font-medium mb-2">Select Date</label>
            <div className="flex gap-x-4">
              <input
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
                required
                className="w-full px-4 py-2 rounded-lg bg-gray-900 text-white border border-gray-700 focus:outline-none focus:ring-2 focus:ring-orange-500"
              />
              <input
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
                required
                className="w-full px-4 py-2 rounded-lg bg-gray-900 text-white border border-gray-700 focus:outline-none focus:ring-2 focus:ring-orange-500"
              />

            </div>
            
          </div>

          <button
            type="submit"
            className="w-full bg-orange-500 hover:bg-orange-600 text-white font-bold py-3 px-6 rounded-full transition duration-300"
          >
            Control
          </button>
        </form>
      </div>
    </section>
    {showModal && (
  <div className="fixed inset-0 bg-black bg-opacity-70 flex items-center justify-center z-50">
    <div className="bg-gray-900 text-white p-6 rounded-lg max-h-[95vh] w-full max-w-2xl max-h-[90vh] mx-4">
      {!changeActive ? (
        <>
        <div className="flex items-center justify-between mb-4">
        <h3 className="text-xl font-semibold mb-4 text-orange-400">Available appointment dates</h3>
          <div>
            <button
              onClick={() => {
                setShowModal(false);
                setChangeActive(false);
                setSelectedSlot(null);
              }}
              className="bg-red-400 hover:bg-orange-600 text-white px-2 py-2 rounded-full"
            >
              <CircleX />
            </button>
          </div>
        </div>
          
          {slots.length > 0 ? (
            <ul className="space-y-2 max-h-[300px] overflow-y-auto">
              {slots.map((slot, idx) => {
              const start = new Date(slot.start_time);
              const end = new Date(slot.end_time);
              const status=slot.slot_status;

              return (
                <li key={idx} className="bg-gray-800 p-3 rounded">
                  <p>
                    <span className="font-semibold">Date and time:</span>{" "}
                    {start.toLocaleDateString()} -{" "}
                    {start.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })} to{" "}
                    {end.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })}
                  </p>
          
                  <p><span className="font-semibold">Status:</span> {status}</p>
                  <button
                    onClick={() => {
                      setSelectedSlot(slot);
                      setChangeActive(true);
                    }}
                    className="bg-orange-400 hover:bg-orange-500 text-white text-sm px-2 py-2 mt-5 rounded-full"
                  >
                    Make an appointment
                  </button>
                </li>
              );
            })}

                
              
            </ul>
            

          ) : (
            <p className="text-center text-sm text-red-400">No available appointments were found on the dates you were looking for.</p>
          )}
        </>
      ) : (
        <>
          <h3 className="text-xl font-semibold mb-4 text-orange-400">Appointment Confirmation</h3>
          <div className="space-y-3 text-sm">
            <p><span className="font-semibold">User:</span> {session?.user?.name}</p>
            <p><span className="font-semibold">Start:</span> {new Date(selectedSlot?.start_time).toLocaleString()}</p>
            <p><span className="font-semibold">End:</span> {new Date(selectedSlot?.end_time).toLocaleString()}</p>
            <p className="mt-4">Do you confirm your appointment?</p>
            <div className="flex justify-end gap-4 mt-4">
              <button
                className="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-full"
                onClick={makeReservation}
              >
                Confirm
              </button>
              <button
                className="bg-gray-600 hover:bg-gray-700 text-white px-4 py-2 rounded-full"
                onClick={() => {
                  setChangeActive(false);
                  setSelectedSlot(null);
                }}
              >
                Back
              </button>
            </div>
          </div>
        </>
      )}

      
    </div>
  </div>
)}


      <section className="bg-orange-500 text-white py-8">
        <div className="container mx-auto px-4">
          <h2 className="text-3xl font-bold text-center">GYM FACTS</h2>
        </div>
      </section>
    </div>
  );
}
