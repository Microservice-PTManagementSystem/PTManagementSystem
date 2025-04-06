# PTManagementSystem/notification/src/app/application/notification_service.py
import uuid
from datetime import datetime
from app.domain.models import Notification, NotificationType, NotificationStatus
from app.ports.inbound.notification_service import NotificationServicePort
from app.adapters.outbound.notification_sender import MockNotificationSender

class NotificationService(NotificationServicePort):
    def __init__(self, notification_sender: MockNotificationSender):
        self.notification_sender = notification_sender

    async def send_notification(self, notification: Notification) -> Notification:
        return await self.notification_sender.send(notification)

    async def handle_slot_released(self, event_data: dict) -> None:
        notification = Notification(
            id=str(uuid.uuid4()),
            user_id=event_data["user_id"],
            type=NotificationType.EMAIL,
            content=f"Your appointment slot has been released. Reason: {event_data['reason']}",
            created_at=datetime.utcnow()
        )
        await self.send_notification(notification)

    async def handle_appointment_rescheduled(self, event_data: dict) -> None:
        notification = Notification(
            id=str(uuid.uuid4()),
            user_id=event_data["user_id"],
            type=NotificationType.EMAIL,
            content=f"Your appointment has been rescheduled from slot {event_data['old_slot_id']} to {event_data['new_slot_id']}",
            created_at=datetime.utcnow()
        )
        await self.send_notification(notification)

    async def handle_appointment_canceled(self, event_data: dict) -> None:
        notification = Notification(
            id=str(uuid.uuid4()),
            user_id=event_data["user_id"],
            type=NotificationType.EMAIL,
            content=f"Your appointment has been canceled. Reason: {event_data['reason']}",
            created_at=datetime.utcnow()
        )
        await self.send_notification(notification)