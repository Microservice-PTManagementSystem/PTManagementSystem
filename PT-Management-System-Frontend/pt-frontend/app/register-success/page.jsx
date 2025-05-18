"use client";

import { useSession } from "next-auth/react";
import { useEffect, useState,useRef } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { Facebook, Instagram, Twitter ,LogOut,CircleUser,CircleX} from "lucide-react"
import styles from "../styles/home.module.css";
import { signOut} from "next-auth/react";

export default function RegisterSuccess() {
  const { data: session, status } = useSession();
  const router = useRouter();
  const hasSent = useRef(false); 

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
  useEffect(() => {
    if (status === "authenticated" && session && !hasSent.current) {
      hasSent.current = true;

      const sendRegistrationData = async () => {
        const role = localStorage.getItem("selectedRole");
        const token = session?.accessToken;

        console.log("RegisterSuccess: ", { role, token });

        if (!role || !token) return;

        try {
          const res = await fetch("http://localhost:8008/api/Auth/Register", {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({ token, role }),
          });
          console.log("res",res)

          
          if (!res.ok) throw new Error("Failed to send registration data");

          console.log("✅ Registration data sent successfully");

          setTimeout(() => {
            router.push("/home");
          }, 500); 
        } catch (err) {
          console.error("❌ Registration data send failed:", err);
        }
      };

      sendRegistrationData();
    }
  }, [status, session]);

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
      <p className="text-center mt-12 text-lg">Ana sayfaya yönlendiriliyorsunuz</p>
    </div>
  )

 ;
}
