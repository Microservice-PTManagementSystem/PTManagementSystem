"use client"
import React, { useState } from "react"
import { Check, HelpCircle } from "lucide-react"

// Button Component
const Button = ({ className, children, ...props }) => (
  <button
    className={`inline-flex items-center justify-center rounded-md text-sm font-medium transition-colors focus:outline-none focus:ring-2 focus:ring-offset-2 disabled:opacity-50 disabled:pointer-events-none ${className}`}
    {...props}
  >
    {children}
  </button>
)

// Card Component
const Card = ({ className, children }) => (
  <div className={`bg-white rounded-xl shadow ${className}`}>{children}</div>
)

// Checkbox Component
const Checkbox = ({ id }) => (
  <input type="checkbox" id={id} className="w-4 h-4 rounded border-gray-300 text-[#00e5b3] focus:ring-[#00e5b3]" />
)

// Input Component
const Input = ({ className, ...props }) => (
  <input
    className={`block w-full rounded-md border px-3 py-2 shadow-sm focus:outline-none focus:ring-2 focus:ring-[#00e5b3] ${className}`}
    {...props}
  />
)

// Label Component
const Label = ({ htmlFor, children, className }) => (
  <label htmlFor={htmlFor} className={`block font-medium ${className}`}>
    {children}
  </label>
)

// Select Component (Simple Mock)
const Select = ({ children }) => <div className="w-full">{children}</div>
const SelectTrigger = ({ children, className }) => (
  <div className={`border rounded-md px-3 py-2 ${className}`}>{children}</div>
)
const SelectValue = ({ placeholder }) => <span className="text-gray-400">{placeholder}</span>
const SelectContent = ({ children }) => <div className="mt-2 bg-white border rounded-md">{children}</div>
const SelectItem = ({ children, value }) => (
  <div className="px-3 py-1 cursor-pointer hover:bg-gray-100" data-value={value}>
    {children}
  </div>
)


export default function CheckoutPage() {
  const [selectedPayment, setSelectedPayment] = useState("mastercard")
  const [cardNumber, setCardNumber] = useState("")
  const [cardHolder, setCardHolder] = useState("")
  const [expiryMonth, setExpiryMonth] = useState("")
  const [expiryYear, setExpiryYear] = useState("")
  const [cvc, setCvc] = useState("")
  const [saveCard, setSaveCard] = useState(false)
  //const [price,setPrice] = useState("")

  const paymentMethods = [
    { id: "mastercard", name: "Mastercard", logo: "/assets/mastercard.png" },
    { id: "visa", name: "Visa", logo: "/assets/visa.png" },
    { id: "paypal", name: "PayPal", logo: "/assets/paypal.png" },
    { id: "cash", name: "Cash on Delivery", logo: "" },
  ]

  const price = 250
  const deliveryCost = 5.5
  const total = price + deliveryCost

  const handleConfirmAppointment = async () => {
    if (!selectedSlot?.slot_id || !session?.user?.id) {
      alert("Appointment or user information missing.");
      return;
    }
  
    try {
      
      const paymentResponse = await fetch("http://localhost:8006/payment-controller/payment/confirm", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          paymentId:0,
          success:true,
          message:"message",
          cardNumber:cardNumber,
          cardHolder:cardHolder,
          expiryMonth:expiryMonth,
          expiryYear:expiryYear,
          cvc:cvc,
          saveCard:saveCard,
          totalAmount: total,
          user_id: session.user.id,
          slot_id: selectedSlot.slot_id,
          paymentMethod: selectedPayment,
        }),
      });
  
      const paymentData = await paymentResponse.json();
  
      if (paymentData.success) {
        
        const reservationResponse = await fetch("http://localhost:8004/make_reservation/make_reservation", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            slot_id: selectedSlot.slot_id,
            user_id: session.user.id,
            timestamp: new Date().toISOString(),
          }),
        });
  
        const reservationData = await reservationResponse.json();
        alert("Your appointment has been confirmed.");
        setShowModal(false);
        setChangeActive(false);
        setSelectedSlot(null);
  
      } else {
        alert("Payment failed: " + (paymentData.message || "Please try again."));
      }
    } catch (error) {
      console.error("Error during confirmation:", error);
      alert("An error occurred while confirming your appointment.");
    }
  };
  
  const handlePayment = async () => {
    if ((selectedPayment === "mastercard" || selectedPayment === "visa") &&
        (!cardNumber || !cardHolder || !expiryMonth || !expiryYear || !cvc)) {
      alert("Please fill in all card details.")
      return
    }

    const payload = {
      paymentMethod: selectedPayment,
      cardNumber,
      cardHolder,
      expiryMonth,
      expiryYear,
      cvc,
      saveCard,
      totalAmount: total,
    }

    try {
      const response = await fetch("/api/payment/confirm", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(payload)
      })

      if (response.ok) {
        alert("Payment successful")
      } else {
        console.error("Payment failed")
        alert("Payment failed")
      }
    } catch (error) {
      console.error("Error submitting payment:", error)
      alert("Something went wrong.")
    }
  }
  
  return (
    <div className="min-h-screen flex items-center justify-center bg-[#e8f2f0] p-4">
      <Card className="w-full max-w-3xl p-8 shadow-lg">
        {/* Progress Steps */}
        <div className="flex justify-center mb-10">
          <div className="flex items-center gap-4 text-sm text-gray-500">
            <div className="flex flex-col items-center">
              <div className="w-8 h-8 rounded-full bg-[#00e5b3] flex items-center justify-center mb-2">
                <Check className="h-5 w-5 text-white" />
              </div>
              <span>CUSTOMER DETAILS</span>
            </div>
            <div className="w-16 h-[2px] bg-[#00e5b3]" />
            <div className="flex flex-col items-center">
              <div className="w-8 h-8 rounded-full border-2 border-[#00e5b3] flex items-center justify-center mb-2">
                <span className="text-[#00e5b3] text-sm font-medium">2</span>
              </div>
              <span className="text-[#00e5b3] font-medium">PAYMENT METHOD</span>
            </div>
            <div className="w-16 h-[2px] bg-gray-300" />
            <div className="flex flex-col items-center">
              <div className="w-8 h-8 rounded-full border-2 border-gray-300 flex items-center justify-center mb-2">
                <span className="text-gray-400 text-sm">3</span>
              </div>
              <span>CONFIRMATION</span>
            </div>
          </div>
        </div>

        {/* Payment Methods */}
        <div className="flex justify-center items-center gap-4 mb-8">
          {paymentMethods.map((method) => (
            <div
              key={method.id}
              className={`border rounded-md p-4 flex items-center justify-center cursor-pointer relative h-16 w-24 ${
                selectedPayment === method.id
                  ? "border-[#00e5b3]"
                  : "border-gray-200"
              }`}
              onClick={() => setSelectedPayment(method.id)}
            >
              <div className="absolute left-3 top-1/2 -translate-y-1/2">
                <div
                  className={`w-5 h-5 rounded-full border ${
                    selectedPayment === method.id
                      ? "border-[#00e5b3]"
                      : "border-gray-300"
                  } flex items-center justify-center`}
                >
                  {selectedPayment === method.id && (
                    <div className="w-3 h-3 rounded-full bg-[#00e5b3]" />
                  )}
                </div>
              </div>

              {method.id === "cash" ? (
                <div className="text-center text-gray-600 text-xs ml-4">Cash on Delivery</div>
              ) : (
                <div className="h-8 flex items-center justify-center ml-4">
                  <img
                    src={method.logo || "/placeholder.svg?height=30&width=48"}
                    alt={method.name}
                    className="h-8"
                  />
                </div>
              )}
            </div>
          ))}
        </div>

        {/* Card Details Form */}
        {(selectedPayment === "mastercard" || selectedPayment === "visa") && (
         <div className="space-y-4 mb-8">
           <div className="grid grid-cols-1 gap-4">
             <div>
                <Label htmlFor="cardNumber" className="text-sm text-gray-500">
                Card number <span className="text-red-500">*</span>
                </Label>
                <Input
                id="cardNumber"
                value={cardNumber}
                onChange={(e) => setCardNumber(e.target.value)}
                className="mt-1 border-gray-300"
                />
            </div>

            <div>
                <Label htmlFor="cardHolder" className="text-sm text-gray-500">
                Cardholder <span className="text-red-500">*</span>
                </Label>
                <Input
                id="cardHolder"
                value={cardHolder}
                onChange={(e) => setCardHolder(e.target.value)}
                className="mt-1 border-gray-300"
                />
            </div>

            <div className="grid grid-cols-3 gap-4">
                <div className="col-span-2">
                <Label htmlFor="expiryDate" className="text-sm text-gray-500">
                    Expiry date <span className="text-red-500">*</span>
                </Label>
                <div className="flex gap-2 mt-1">
                    <select
                    value={expiryMonth}
                    onChange={(e) => setExpiryMonth(e.target.value)}
                    className="border border-gray-300 rounded-md px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-[#00e5b3]"
                    >
                    <option value="">Month</option>
                    {Array.from({ length: 12 }, (_, i) => {
                        const value = (i + 1).toString().padStart(2, "0")
                        return <option key={value} value={value}>{value}</option>
                    })}
                    </select>

                    <select
                    value={expiryYear}
                    onChange={(e) => setExpiryYear(e.target.value)}
                    className="border border-gray-300 rounded-md px-3 py-2 w-full focus:outline-none focus:ring-2 focus:ring-[#00e5b3]"
                    >
                    <option value="">Year</option>
                    {Array.from({ length: 10 }, (_, i) => {
                        const year = new Date().getFullYear() + i
                        return <option key={year} value={year}>{year}</option>
                    })}
                    </select>
                </div>
                </div>

                <div>
                <div className="flex items-center">
                    <Label htmlFor="cvc" className="text-sm text-gray-500">
                    CVC
                    </Label>
                    <HelpCircle className="h-4 w-4 text-gray-400 ml-1" />
                </div>
                <Input
                    id="cvc"
                    value={cvc}
                    onChange={(e) => setCvc(e.target.value)}
                    className="mt-1 border-gray-300"
                />
                </div>
            </div>
            </div>

            <div className="flex items-center space-x-2">
            <Checkbox id="saveCard" />
            <Label htmlFor="saveCard" className="text-sm text-gray-500">
                Save my details for future purchases
            </Label>
            </div>
        </div>
        )}

        {/* Order Summary */}
        <div className="space-y-2 mb-6">
        <div className="flex justify-between text-sm text-gray-600">
            <span>Subtotal (2 items)</span>
            <span>${price.toFixed(2)}</span>
        </div>
        <div className="flex justify-between text-sm text-gray-600">
            <span>Home delivery cost</span>
            <span>${deliveryCost.toFixed(2)}</span>
        </div>
        <div className="flex justify-between text-gray-600 font-medium mt-4">
            <span>Total Amount</span>
            <span>${total.toFixed(2)}</span>
        </div>
        </div>

        {/* Confirm Button */}
        <Button
        onClick={handlePayment}
        className="w-full bg-[#e65c00] hover:bg-[#00c99f] text-white py-6"
        >
        Confirm Payment
        </Button>
    </Card>
    </div>

  )
}
