"use client"

import { useState} from "react"
import Link from "next/link"
import { ChevronRight } from "lucide-react"
import { Facebook, Instagram, Twitter ,LogOut,CircleUser} from "lucide-react"
import { signOut,useSession } from "next-auth/react";
import { trainers } from "../lib/trainers"; 

export default function SettingsPage() {
  const [activeTab, setActiveTab] = useState("account")
  const { data: session } = useSession();  
  const [appointments, setAppointments] = useState([]);

  const [trainerMap, setTrainerMap] = useState({});

    


  const checkAppointments =async (e) => {
    
    
    try {
      const response = await fetch("http://localhost:8004/make_reservation/get_active_reservations_by_user_id", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
            user_id: session?.user?.id,
        }),
      });

      const data =await response.json();
      console.log(data)
      //console.log("card number",data[0].cardNumber)
      //setAppointments(data || []);
      setAppointments(data );
      console.log("appointments",appointments)
    } catch (error) {
      console.error("Error fetching slots:", error);
      setAppointments([]);
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
    <div>

   
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
    <div className="min-h-screen bg-blue-50 flex items-center justify-center p-4 relative">
      {/* Background circles */}
      
      <div className="absolute top-0 left-0 w-64 h-64 rounded-full bg-blue-100 -z-10"></div>
      <div className="absolute bottom-0 right-0 w-48 h-48 rounded-full bg-blue-200 -z-10"></div>
      <div className="absolute bottom-1/4 right-1/4 w-32 h-32 rounded-full bg-blue-100 -z-10"></div>

      <div className="bg-white rounded-3xl shadow-lg w-full max-w-5xl overflow-hidden">
        {/* Header */}
        <div className="flex justify-between items-center p-6 border-b">
          <div className="flex-1"></div>
          <div className="w-full flex items-start">
            <h2 className="text-3xl font-bold mb-6 text-blue-900">Settings</h2>
          </div>
          <div className="flex-1 flex justify-end">
            <Avatar />
          </div>
        </div>

        {/* Content */}
        <div className="flex">
          {/* Sidebar */}
          <div className="w-64 p-6 border-r">
            
            <nav className="space-y-1">
              <button
                onClick={() => setActiveTab("account")}
                className={`block w-full text-left py-2 px-4 rounded-lg font-medium ${
                  activeTab === "account"
                    ? "bg-orange-400 text-white"
                    : "text-gray-600 hover:bg-blue-50"
                }`}
              >
                Account
              </button>
              <button
                onClick={() => {
                    setActiveTab("appointments");
                    checkAppointments();
                  }}
                className={`block w-full text-left py-2 px-4 rounded-lg font-medium ${
                  activeTab === "appointments"
                    ? "bg-orange-400 text-white"
                    : "text-gray-600 hover:bg-blue-50"
                }`}
              >
                Appointments
              </button>
              <button
                onClick={() => setActiveTab("notifications")}
                className={`block w-full text-left py-2 px-4 rounded-lg font-medium ${
                  activeTab === "notifications"
                    ? "bg-orange-400 text-white"
                    : "text-gray-600 hover:bg-blue-50"
                }`}
              >
                Notifications
              </button>
              <button
                onClick={() => setActiveTab("paymentInfo")}
                className={`block w-full text-left py-2 px-4 rounded-lg font-medium ${
                  activeTab === "paymentInfo"
                    ? "bg-orange-400 text-white"
                    : "text-gray-600 hover:bg-blue-50"
                }`}
              >
                Payment Informations
              </button>
            </nav>
          </div>

          {/* Main Panel */}
          <div className="flex-1 p-8">
            {activeTab === "account" && (
              <>
                <h1 className="text-2xl font-bold mb-6 text-gray-600">Account Settings</h1>

                <div className="mb-8">
                  <h2 className="text-lg font-semibold mb-4 text-gray-600">Basic info</h2>

                  <div className="flex items-start mb-6">
                    <div className="w-32 font-medium text-gray-400">Profile Picture</div>
                    <div className="flex items-center gap-4">
                      <Avatar size="lg" />
                      <div className="text-sm">
                        <TextButton color="blue">Upload new picture</TextButton>
                        <br />
                        <TextButton color="red">Remove</TextButton>
                      </div>
                    </div>
                  </div>

                  <div className="text-gray-600">
                    <Item label="Name" value= {session?.user?.name}/>
                    <Item label="Date of birth" value=" " />
                    <Item label="Gender" value=" " />
                    <Item label="Email" value={session?.user?.email} />
                  </div>
                </div>

                <div className="text-gray-600">
                  <h2 className="text-lg font-semibold mb-4 text-gray-600">Account info</h2>
                  <Item label="User id" value={session?.user?.id}/>
                  <Item label="Username" value={session?.user?.username} />
                
                  
                </div>
              </>
            )}

            {activeTab === "appointments" && (
              <>
                <h1 className="text-2xl font-bold mb-6 text-gray-600">My Appointments</h1>
                <p className="text-gray-600 mb-4">Here you can see your upcoming and past appointments.</p>

                <div className="space-y-4 text-gray-600">
                {appointments.map((appointment) => {
                    const start = new Date(appointment.slot_detail.start_time);
                    const end = new Date(appointment.slot_detail.end_time);
                    const trainer = trainers.find(t => t.id === appointment.slot_detail.trainer_id);
                    const trainerName = trainer ? trainer.name : "Unknown Trainer";
                    const status = new Date(appointment.status);
                    const formattedDate = `${start.toLocaleDateString()} - ${start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} to ${end.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
                    return (
                    <AppointmentCard
                        key={appointment.slot_id}
                        title={`Trainer : ${trainerName}`}
                        date={formattedDate}
                    />
                    );
                })}
                </div>

              </>
            )}
            {activeTab === "notifications" && (
              <>
                <h1 className="text-2xl font-bold mb-6 text-gray-600">Notifications</h1>
                <p className="text-gray-600 mb-4">No notifications</p>

                <div className="space-y-4 text-gray-600">
                  
                </div>
              </>
            )}
            {activeTab === "paymentInfo" && (
              <>
                <h1 className="text-2xl font-bold mb-6 text-gray-600">Payment Informations</h1>
                <TextButton className="text-gray-600 mb-4 ">Click to add card</TextButton>

                <div className="space-y-4 text-gray-600 mt-10">
                  {Array.from(
                    new Map(
                      appointments.map((item) => [
                        // Benzersiz kartı anahtar olarak belirliyoruz (kart numarası + holder)
                        `${item.cardNumber}-${item.cardHolder}`,
                        item,
                      ])
                    ).values()
                  ).map((card, index) => (
                    <div
                      key={index}
                      className="border rounded-xl p-4 shadow-md bg-white"
                    >
                      <p><strong>Card Holder:</strong> {card.cardHolder}</p>
                      <p><strong>Card Number:</strong> •••• •••• •••• {card.cardNumber.slice(-4)}</p>
                      <p><strong>Expiry:</strong> {card.expiryMonth}/{card.expiryYear}</p>
                      <p><strong>Payment Method:</strong> {card.paymentMethod}</p>
                    </div>
                  ))}
                </div>
              </>
            )}

          </div>
        </div>
      </div>
    </div>
    </div>
  )
}


function Avatar({ size }) {
  const dimension = size === "lg" ? "h-12 w-12" : "h-10 w-10"
  return (
    <div className={`rounded-full overflow-hidden bg-blue-100 border-2 border-blue-100 ${dimension} flex items-center justify-center`}>
      <img
        src="/placeholder.svg?height=48&width=48"
        alt="User"
        className="object-cover w-full h-full"
      />
    </div>
  )
}

function TextButton({ children, color }) {
  const colorClass = color === "red" ? "text-red-500" : "text-blue-500"
  return (
    <button className={`h-auto p-0 text-sm underline ${colorClass}`}>
      {children}
    </button>
  )
}

function Item({ label, value }) {
  return (
    <div className="flex items-center py-3 border-t">
      <div className="w-32 font-medium">{label}</div>
      <div className="flex-1">{value}</div>
      <ChevronRight className="h-5 w-5 text-gray-400" />
    </div>
  )
}

function AppointmentCard({ title, date }) {
  return (
    <div className="border rounded-xl p-4 shadow-sm bg-blue-50">
      <div className="font-semibold">{title}</div>
      <div className="text-sm text-gray-600">{date}</div>
    </div>
  )
}
