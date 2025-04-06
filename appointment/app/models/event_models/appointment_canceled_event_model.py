from datetime import datetime
from pydantic import BaseModel


class AppointmentCanceledEvent(BaseModel):
    slot_id: str
    user_id: str
    trainer_id: str
    reason: str
    timestamp: datetime = datetime.utcnow()