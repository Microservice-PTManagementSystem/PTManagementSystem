from fastapi import FastAPI
from app.api.notification import router as appointment_router

app = FastAPI(title="API", description="", version="1.0")

# Router'ları ekle
app.include_router(appointment_router)

if __name__ == "__main__":
    import uvicorn
    uvicorn.run("main:app", host="127.0.0.1", port=8000, reload=True)
