from datetime import datetime
from pydantic import BaseModel

class AppointmentRescheduledEvent(BaseModel):
    old_slot_id: str
    new_slot_id: str
    user_id: str
    trainer_id: str
    timestamp: datetime = datetime.utcnow()