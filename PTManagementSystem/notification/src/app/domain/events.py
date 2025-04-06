# PTManagementSystem/notification/src/app/domain/events.py
from datetime import datetime
from pydantic import BaseModel

class NotificationSentEvent(BaseModel):
    notification_id: str
    user_id: str
    sent_at: datetime = datetime.utcnow()

class NotificationFailedEvent(BaseModel):
    notification_id: str
    user_id: str
    reason: str
    failed_at: datetime = datetime.utcnow()