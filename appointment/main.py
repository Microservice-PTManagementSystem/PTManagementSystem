from fastapi import FastAPI
from app.api.appointment import router as appointment_router
from app.services.slot_services.slot_confirmed_event import start_consumer
app = FastAPI(title=" API", description="", version="1.0")

app.include_router(appointment_router)

if __name__ == "__main__":
    import uvicorn
    import threading
    consumer_thread = threading.Thread(target=start_consumer)
    consumer_thread.daemon = True  # Ana uygulama kapanırken thread de kapansın
    consumer_thread.start()
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)

