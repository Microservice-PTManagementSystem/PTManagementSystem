from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.api.appointment import router as appointment_router
from app.services.slot_services.slot_confirmed_event import start_consumer

app = FastAPI(title="API", description="", version="1.0")

# CORS ayarları - Her yerden gelen istekleri kabul et
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Tüm domainlere izin ver
    allow_credentials=True,
    allow_methods=["*"],  # Tüm HTTP metodlara izin ver
    allow_headers=["*"],  # Tüm headerlara izin ver
)

# Router'ı dahil et
app.include_router(appointment_router)

# Kafka (veya başka bir şey) dinleyicisini thread'de başlat
if __name__ == "__main__":
    import uvicorn
    import threading
    consumer_thread = threading.Thread(target=start_consumer)
    consumer_thread.daemon = True  # Ana uygulama kapanırken thread de kapansın
    consumer_thread.start()
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)