"use client"
import Image from "next/image"
import Link from "next/link"
import React, { useState } from "react"
import { Facebook, Instagram, Twitter ,LogOut,CircleUser} from "lucide-react"
import styles from "../styles/home.module.css";
import { signOut,useSession } from "next-auth/react";
import { useRouter } from 'next/navigation';


export default function Home() {

  const [trainer, setTrainer] = useState("");
  const [frequency, setFrequency] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [slots, setSlots] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const router = useRouter();
  const { data: session } = useSession();  

  const trainers = [
    {
      id: 1,
      name: "JAMIE WARRING",
      profession: "Fitness Trainer",
      imageUrl: "/trainer_jamie.jpg",
    },
    {
      id: 2,
      name: "MARK",
      profession: "Fitness Trainer",
      imageUrl: "/mark.jpg",
    },
    {
      id: 3,
      name: "RAPHAEL",
      profession: "Fitness Trainer",
      imageUrl: "/raphael.jpeg",
    },
  ];

  const handleSubmit = (e) => {
    e.preventDefault();
    const selectedTrainer = trainers.find((t) => t.name === trainer);

    if (!selectedTrainer) {
      alert("Trainer not found.");
      return;
    }

    try {
      const response = fetch("http://localhost:3004/make_reservation/available_slots", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          trainerId: selectedTrainer.id,
          startDate:startDate,
          endDate:endDate,
        }),
      });

      const data =response.json();
      setSlots(data || []);
      setShowModal(true);
    } catch (error) {
      console.error("Error fetching slots:", error);
      setSlots([]);
      setShowModal(true);
    }
  };

  /*const handleSignOut = async () => {
    await signOut({
      redirect: true,
      callbackUrl:
        "http://localhost:8080/realms/ptmanagement/protocol/openid-connect/logout" +
        "?post_logout_redirect_uri=http://localhost:3000" +
        "&client_id=nextjs-app",
    }); // Çıkış yapma işlemi
    router.push('/');
    console.log("--------------ÇIKIŞ YAPILDI---------------");
  };*/
  /*const handleSignOut = () => {
    
    signOut({ redirect: false }).then(() => {
      
      const base   = "http://localhost:8080";
      const realm  = "ptmanagement";
      const client = "nextjs-app";
  
      const redirect = encodeURIComponent("http://localhost:3000");
      const idToken  = encodeURIComponent(session.idToken);   
  
      const kcLogout =
        `${base}/realms/${realm}/protocol/openid-connect/logout` +
        `?id_token_hint=${idToken}` +
        `&post_logout_redirect_uri=${redirect}` +
        `&client_id=${client}`;
  
      window.location.href = kcLogout;
    });
  };*/

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
            <label className="block text-sm font-medium mb-2">Session Frequency</label>
            <select
              value={frequency}
              onChange={(e) => setFrequency(e.target.value)}
              required
              className="w-full px-4 py-2 rounded-lg bg-gray-900 text-white border border-gray-700 focus:outline-none focus:ring-2 focus:ring-orange-500"
            >
              <option value="">-- Choose Frequency --</option>
              <option value="daily">Daily</option>
              <option value="weekly">Weekly</option>
              <option value="monthly">Monthly</option>
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
            Book Now
          </button>
        </form>
      </div>
    </section>
    {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-70 flex items-center justify-center z-50">
          <div className="bg-gray-900 text-white p-6 rounded-lg w-full max-w-md mx-4">
            <h3 className="text-xl font-semibold mb-4 text-orange-400">Available Slots</h3>
            {slots.length > 0 ? (
              <ul className="space-y-2 max-h-[300px] overflow-y-auto">
                {slots.map((slot, idx) => (
                  <li key={idx} className="bg-gray-800 p-3 rounded">
                    {slot}
                  </li>
                ))}
              </ul>
            ) : (
              <p className="text-center text-sm text-red-400">Aradığınız tarihlerde boş randevu bulunamadı.</p>
            )}
            <div className="mt-6 text-right">
              <button
                onClick={() => setShowModal(false)}
                className="bg-orange-500 hover:bg-orange-600 text-white px-4 py-2 rounded-full"
              >
                Kapat
              </button>
            </div>
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

/* <a 
                  onClick={() => signOut({ callbackUrl: "/auth/signin" })}
                  className="cursor-pointer text-sm hover:text-orange-500"
                  >
                  <LogOut size={20} />
                </a>*/