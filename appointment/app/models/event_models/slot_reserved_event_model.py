from datetime import datetime
from pydantic import BaseModel


class SlotReservedEvent(BaseModel):
    slot_id: str
    user_id: str
    timestamp: datetime = datetime.utcnow()