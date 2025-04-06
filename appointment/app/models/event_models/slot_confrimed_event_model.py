from datetime import datetime
from pydantic import BaseModel
from typing import Optional

class SlotConfirmedEvent(BaseModel):
    slot_id: str
    user_id: str
    timestamp: datetime = datetime.utcnow()