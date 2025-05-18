from fastapi import FastAPI
from app.api.notification import router as appointment_router
from app.services.rabbitmq_consumer import start_consumer
from app.services.cancel_appointment_consumer import start_cancel_consumer
from app.services.failed_appointment_comsumer import failed_start_consumer
import threading

app = FastAPI(title="API", description="", version="1.0")

app.include_router(appointment_router)

if __name__ == "__main__":
    import uvicorn

    # Consumerları thread ile başlat
    threading.Thread(target=start_cancel_consumer, daemon=True).start()
    threading.Thread(target=start_consumer, daemon=True).start()
    threading.Thread(target=failed_start_consumer, daemon=True).start()

    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)
