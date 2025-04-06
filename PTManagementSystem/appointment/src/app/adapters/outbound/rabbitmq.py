# PTManagementSystem/appointment/src/app/adapters/outbound/rabbitmq.py
import json
import aio_pika
from ports.outbound.event_publisher import EventPublisherPort
from domain.events import (
    SlotReservedEvent, SlotConfirmedEvent, SlotReleasedEvent,
    AppointmentCompletedEvent, AppointmentRescheduledEvent, AppointmentCanceledEvent
)

class RabbitMQEventPublisher(EventPublisherPort):
    def __init__(self, connection_string: str):
        self.connection_string = connection_string
        self.connection = None
        self.channel = None

    async def connect(self):
        if not self.connection:
            self.connection = await aio_pika.connect_robust(self.connection_string)
            self.channel = await self.connection.channel()

    async def _publish(self, routing_key: str, event: dict):
        await self.connect()
        message = aio_pika.Message(body=json.dumps(event).encode())
        await self.channel.default_exchange.publish(
            message,
            routing_key=routing_key
        )

    async def publish_slot_reserved(self, event: SlotReservedEvent):
        await self._publish("slot.reserved", event.dict())

    async def publish_slot_confirmed(self, event: SlotConfirmedEvent):
        await self._publish("slot.confirmed", event.dict())

    async def publish_slot_released(self, event: SlotReleasedEvent):
        await self._publish("slot.released", event.dict())

    async def publish_appointment_completed(self, event: AppointmentCompletedEvent):
        await self._publish("appointment.completed", event.dict())

    async def publish_appointment_rescheduled(self, event: AppointmentRescheduledEvent):
        await self._publish("appointment.rescheduled", event.dict())

    async def publish_appointment_canceled(self, event: AppointmentCanceledEvent):
        await self._publish("appointment.canceled", event.dict())