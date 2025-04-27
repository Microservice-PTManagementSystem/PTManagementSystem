/*"use client"

import React, { useState, forwardRef } from "react";
import { Check, ArrowRight } from "lucide-react";
import Image from "next/image" 

// Utility function for merging class names
function cn(...classes) {
  return classes.filter(Boolean).join(" ");
}

// Button component
const Button = forwardRef(({ className, children, ...props }, ref) => {
  return (
    <button
      ref={ref}
      className={cn(
        "inline-flex items-center justify-center rounded-md text-sm font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-black disabled:opacity-50 disabled:pointer-events-none",
        className
      )}
      {...props}
    >
      {children}
    </button>
  );
});
Button.displayName = "Button";

// Input component
const Input = forwardRef(({ className, type = "text", ...props }, ref) => {
  return (
    <input
      type={type}
      ref={ref}
      className={cn(
        "flex h-10 w-full rounded-md border border-gray-300 px-3 py-2 text-sm placeholder:text-gray-500 focus:outline-none focus:ring-2 focus:ring-black",
        className
      )}
      {...props}
    />
  );
});
Input.displayName = "Input";

// Checkbox component
const Checkbox = ({ checked, onCheckedChange }) => {
  return (
    <div
      className={cn(
        "flex h-4 w-4 items-center justify-center rounded-sm border border-black cursor-pointer",
        checked ? "bg-black text-white" : ""
      )}
      onClick={() => onCheckedChange(!checked)}
    >
      {checked && <Check className="h-4 w-4" />}
    </div>
  );
};

// Main Page
export default function LoginPage() {
  return (
    <main className="flex min-h-screen items-center justify-center bg-gradient-to-br from-blue-900 via-blue-1100 to-black p-4">
      <LogInForm />
    </main>
  );
}

// LogInForm Component
function LogInForm() {
  const [formData, setFormData] = useState({
    
    email: "",
    password: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleCheckboxChange = (checked) => {
    setFormData((prev) => ({ ...prev, acceptTerms: checked }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Form submitted:", formData);
  };

  return (
    <div className="flex w-full max-w-4xl overflow-hidden rounded-3xl bg-white shadow-2xl">
      
      <div className="relative hidden w-1/2 bg-black md:block">
        <Image
          src="/login_photo.jpg"
          alt="login/register page"
          layout="fill"  // Resmi kapsadığı alanı dolduracak şekilde yerleştirir
          objectFit="cover"  // Resmin boyutları koruyarak alanı kaplamasını sağlar
          className="z-0" 
        />
        <div className="absolute inset-0 z-10 flex flex-col justify-center p-12 text-white bg-black/50">
          <h1 className="mb-2 text-3xl font-bold">Discover Your Power, Step Into New Beginnings!</h1>
          <p className="text-mg">Log in to your account or create a new account now! A healthy life starts with you.</p>
        </div>
      </div>

      
      <div className="w-full p-8 md:w-1/2 md:p-12">
        <div className="mb-8">
          <h2 className="text-2xl font-bold text-gray-700">Log In</h2>
        </div>
        <form onSubmit={handleSubmit} className="space-y-4">
          
          <Input
            type="email"
            name="email"
            placeholder="Email address"
            value={formData.email}
            onChange={handleChange}
          />
          <Input
            type="password"
            name="password"
            placeholder="Password"
            value={formData.password}
            onChange={handleChange}
          />

          

          <Button className="w-full bg-black py-3 text-white hover:bg-gray-800" href='/home'>
            Log in <ArrowRight className="ml-2 h-4 w-4" />
          </Button>

          <div className="relative my-6 text-center">
            <div className="absolute inset-0 flex items-center">
              <div className="w-full border-t border-gray-300"></div>
            </div>
            <div className="relative z-10 bg-white px-2 text-xs text-gray-500 uppercase">
              or
            </div>
          </div>

          <Button
            type="button"
            className="w-full border border-gray-300 py-3 hover:bg-gray-100 text-gray-600"
            onClick={() => console.log("Sign up with Google")}
          >
            <span className="mr-2">🌐</span> Sign in with Google
          </Button>

        
        </form>
      </div>
    </div>
  );
}*/
