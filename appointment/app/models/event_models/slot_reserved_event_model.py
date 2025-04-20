from datetime import datetime
from pydantic import BaseModel


class slot_reserved_event_model(BaseModel):
    slot_id: str
    user_id: str
    timestamp: datetime = datetime.utcnow()