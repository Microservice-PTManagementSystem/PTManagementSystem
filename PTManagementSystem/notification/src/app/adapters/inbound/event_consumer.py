# PTManagementSystem/notification/src/app/adapters/inbound/event_consumer.py
import json
import aio_pika
from app.ports.inbound.notification_service import NotificationServicePort

class RabbitMQEventConsumer:
    def __init__(self, connection_string: str, notification_service: NotificationServicePort):
        self.connection_string = connection_string
        self.notification_service = notification_service
        self.connection = None
        self.channel = None

    async def connect(self):
        if not self.connection:
            self.connection = await aio_pika.connect_robust(self.connection_string)
            self.channel = await self.connection.channel()

    async def setup_queues(self):
        await self.connect()
        
        # Declare queues for different events
        queue_names = [
            "slot.released",
            "appointment.rescheduled",
            "appointment.canceled"
        ]
        
        for queue_name in queue_names:
            await self.channel.declare_queue(queue_name, durable=True)

    async def start_consuming(self):
        await self.setup_queues()
        
        # Setup consumers for each queue
        slot_released_queue = await self.channel.declare_queue("slot.released")
        appointment_rescheduled_queue = await self.channel.declare_queue("appointment.rescheduled")
        appointment_canceled_queue = await self.channel.declare_queue("appointment.canceled")

        await slot_released_queue.consume(self._handle_slot_released)
        await appointment_rescheduled_queue.consume(self._handle_appointment_rescheduled)
        await appointment_canceled_queue.consume(self._handle_appointment_canceled)

    async def _handle_slot_released(self, message: aio_pika.IncomingMessage):
        async with message.process():
            event_data = json.loads(message.body.decode())
            await self.notification_service.handle_slot_released(event_data)

    async def _handle_appointment_rescheduled(self, message: aio_pika.IncomingMessage):
        async with message.process():
            event_data = json.loads(message.body.decode())
            await self.notification_service.handle_appointment_rescheduled(event_data)

    async def _handle_appointment_canceled(self, message: aio_pika.IncomingMessage):
        async with message.process():
            event_data = json.loads(message.body.decode())
            await self.notification_service.handle_appointment_canceled(event_data)