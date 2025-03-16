from fastapi import FastAPI
from api.appointment import router as appointment_router

app = FastAPI(title=" API", description="", version="1.0")

app.include_router(appointment_router)

if __name__ == "__main__":
    import uvicorn
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)
