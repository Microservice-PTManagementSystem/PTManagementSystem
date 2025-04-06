# PTManagementSystem/appointment/src/app/ports/outbound/event_publisher.py
from abc import ABC, abstractmethod
from domain.events import (
    SlotReservedEvent, SlotConfirmedEvent, SlotReleasedEvent,
    AppointmentCompletedEvent, AppointmentRescheduledEvent, AppointmentCanceledEvent
)

class EventPublisherPort(ABC):
    @abstractmethod
    async def publish_slot_reserved(self, event: SlotReservedEvent) -> None:
        pass

    @abstractmethod
    async def publish_slot_confirmed(self, event: SlotConfirmedEvent) -> None:
        pass

    @abstractmethod
    async def publish_slot_released(self, event: SlotReleasedEvent) -> None:
        pass

    @abstractmethod
    async def publish_appointment_completed(self, event: AppointmentCompletedEvent) -> None:
        pass

    @abstractmethod
    async def publish_appointment_rescheduled(self, event: AppointmentRescheduledEvent) -> None:
        pass

    @abstractmethod
    async def publish_appointment_canceled(self, event: AppointmentCanceledEvent) -> None:
        pass