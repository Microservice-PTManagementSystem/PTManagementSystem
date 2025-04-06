from fastapi import FastAPI
from api.notification import router as appointment_router
from services.rabbitmq_consumer import start_consumer

app = FastAPI(title="API", description="", version="1.0")

app.include_router(appointment_router)

if __name__ == "__main__":
    import uvicorn
    start_consumer()
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)






    