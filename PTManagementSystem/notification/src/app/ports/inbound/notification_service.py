# PTManagementSystem/notification/src/app/ports/inbound/notification_service.py
from abc import ABC, abstractmethod
from app.domain.models import Notification

class NotificationServicePort(ABC):
    @abstractmethod
    async def send_notification(self, notification: Notification) -> Notification:
        pass

    @abstractmethod
    async def handle_slot_released(self, event_data: dict) -> None:
        pass

    @abstractmethod
    async def handle_appointment_rescheduled(self, event_data: dict) -> None:
        pass

    @abstractmethod
    async def handle_appointment_canceled(self, event_data: dict) -> None:
        pass