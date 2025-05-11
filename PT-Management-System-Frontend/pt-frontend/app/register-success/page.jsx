"use client";

import { useSession } from "next-auth/react";
import { useEffect } from "react";
import { useRouter } from "next/navigation";
import axios from "axios";

export default function RegisterSuccess() {
  const { data: session, status } = useSession();
  const router = useRouter();

  useEffect(() => {
    if (status === "authenticated") {
      const role = localStorage.getItem("selectedRole");
      const token = session?.accessToken;

      if (!role || !token) return;

      axios.post("/api/Auth/Register", {
        token,
        role,
      })
      .then(() => {
        router.push("/home"); 
      })
      .catch((err) => {
        console.error("Registration data send failed:", err);
      });
    }
  }, [status, session]);

  return <p className="text-center mt-12 text-lg">Finalizing registration...</p>;
}
