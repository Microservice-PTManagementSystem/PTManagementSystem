"use client";

import { openKeycloakRegister } from "../../utils/getKeycloakRegisterUrl"; // doğru yolu ayarla
import Image from "next/image";

export default function ChooseRolePage() {

  const handleRoleSelect = (role)=> {
    localStorage.setItem("selectedRole", role);
    openKeycloakRegister(); 
  };

  return (
    <div className="flex flex-col items-center justify-center h-screen bg-gray-50">
      
      <div className="flex w-full max-w-4xl overflow-hidden rounded-3xl bg-white shadow-2xl">
      
      <div className="relative hidden w-1/2 bg-black md:block">
        <Image
          src="/assets/login_photo.jpg"
          alt="login/register page"
          fill 
          style={{ objectFit: "cover" }} 
          className="z-0"
        />
        <div className="absolute inset-0 z-10 flex flex-col justify-center p-12 text-white bg-black/50">
          <h1 className="mb-2 text-3xl font-bold">Discover Your Power, Step Into New Beginnings!</h1>
          <p className="text-mg">Log in to your account or create a new account now! A healthy life starts with you.</p>
        </div>
      </div>

      
      <div className="w-full p-8 md:w-1/2 md:p-12">
      <h2 className="text-2xl item-center font-semibold ml-10 mb-5 pt-10 pb-10 text-gray-500">First select your role</h2>
      <div className="flex justify-center space-x-6">
        <button
          className="px-6 py-3 bg-black text-white rounded-lg hover:bg-gray-800"
          onClick={() => handleRoleSelect("trainer")}
        >
          Trainer
        </button>
        <button
          className="px-6 py-3 bg-black text-white rounded-lg hover:bg-gray-800"
          onClick={() => handleRoleSelect("client")}
        >
          Client
        </button>
      </div>
        
      </div>
    </div>

    </div>
    
  );
}
