# PTManagementSystem/notification/src/app/adapters/outbound/notification_sender.py
from app.domain.models import Notification, NotificationStatus
import logging

class MockNotificationSender:
    async def send(self, notification: Notification) -> Notification:
        try:
            logging.info(f"Sending {notification.type} notification to user {notification.user_id}")
            logging.info(f"Content: {notification.content}")
            
            # Simulate successful sending
            notification.status = NotificationStatus.SENT
            return notification
            
        except Exception as e:
            logging.error(f"Failed to send notification: {str(e)}")
            notification.status = NotificationStatus.FAILED
            return notification