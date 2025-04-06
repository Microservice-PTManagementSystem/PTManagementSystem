# PTManagementSystem/appointment/src/app/main.py
from fastapi import FastAPI
from adapters.inbound.api import router as appointment_router, slot_service
from adapters.outbound.rabbitmq import RabbitMQEventPublisher
from application.slot_service import SlotService
import os

app = FastAPI(title="Appointment Service")

# Setup RabbitMQ connection
RABBITMQ_HOST = os.getenv("RABBITMQ_HOST", "localhost")
rabbitmq_url = f"amqp://guest:guest@{RABBITMQ_HOST}/"
event_publisher = RabbitMQEventPublisher(rabbitmq_url)

@app.on_event("startup")
async def startup():
    # Initialize RabbitMQ connection
    await event_publisher.connect()
    
    # Initialize slot service with event publisher
    global slot_service
    slot_service = SlotService(event_publisher)
    
    # Update the router's slot service reference
    appointment_router.slot_service = slot_service

app.include_router(appointment_router, prefix="/api/v1")

@app.get("/health")
async def health_check():
    return {"status": "healthy"}