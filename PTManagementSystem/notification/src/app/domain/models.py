# PTManagementSystem/notification/src/app/domain/models.py
from enum import Enum
from datetime import datetime
from pydantic import BaseModel

class NotificationType(str, Enum):
    EMAIL = "email"
    SMS = "sms"
    PUSH = "push"

class NotificationStatus(str, Enum):
    PENDING = "pending"
    SENT = "sent"
    FAILED = "failed"

class Notification(BaseModel):
    id: str
    user_id: str
    type: NotificationType
    content: str
    status: NotificationStatus = NotificationStatus.PENDING
    created_at: datetime = datetime.utcnow()
    sent_at: datetime = None