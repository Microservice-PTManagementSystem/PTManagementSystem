from fastapi import FastAPI
from api.notification import router as appointment_router
from services.rabbitmq_consumer import start_consumer
from services.cancel_appointment_consumer import start_cancel_consumer
import threading

app = FastAPI(title="API", description="", version="1.0")

app.include_router(appointment_router)

if __name__ == "__main__":
    import uvicorn

    # Consumerları thread ile başlat
    threading.Thread(target=start_cancel_consumer, daemon=True).start()
    threading.Thread(target=start_consumer, daemon=True).start()

    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)
