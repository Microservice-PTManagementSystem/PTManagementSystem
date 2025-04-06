from datetime import datetime
from pydantic import BaseModel
from typing import Optional

class SlotReleasedEvent(BaseModel):
    slot_id: str
    user_id: Optional[str]
    reason: str
    timestamp: datetime = datetime.utcnow()