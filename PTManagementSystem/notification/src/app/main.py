# PTManagementSystem/notification/src/app/main.py
from fastapi import FastAPI
from app.adapters.inbound.event_consumer import RabbitMQEventConsumer
from app.adapters.outbound.notification_sender import MockNotificationSender
from app.application.notification_service import NotificationService
import os

app = FastAPI(title="Notification Service")

# Setup RabbitMQ connection
RABBITMQ_HOST = os.getenv("RABBITMQ_HOST", "localhost")
rabbitmq_url = f"amqp://guest:guest@{RABBITMQ_HOST}/"

# Initialize notification components
notification_sender = MockNotificationSender()
notification_service = NotificationService(notification_sender)

@app.on_event("startup")
async def startup():
    # Initialize and start the event consumer
    event_consumer = RabbitMQEventConsumer(
        connection_string=rabbitmq_url,
        notification_service=notification_service
    )
    await event_consumer.start_consuming()

@app.get("/health")
async def health_check():
    return {"status": "healthy"}