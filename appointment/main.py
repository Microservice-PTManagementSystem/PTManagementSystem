from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.api.appointment import router as appointment_router
from app.services.slot_services.slot_confirmed_event import start_payment_succeeded_consumer, start_payment_failed_consumer


import threading
import uvicorn

app = FastAPI(title="API", description="", version="1.0")

# CORS ayarları
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Router'ı dahil et
app.include_router(appointment_router)

def start_all_consumers():
    succeeded_thread = threading.Thread(target=start_payment_succeeded_consumer, daemon=True)
    failed_thread = threading.Thread(target=start_payment_failed_consumer, daemon=True)

    succeeded_thread.start()
    failed_thread.start()

if __name__ == "__main__":
    start_all_consumers()
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)
