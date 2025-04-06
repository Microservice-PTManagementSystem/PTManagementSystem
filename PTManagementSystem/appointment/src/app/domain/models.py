# PTManagementSystem/appointment/src/app/domain/models.py
from enum import Enum
from datetime import datetime
from typing import Optional
from pydantic import BaseModel

class SlotStatus(str, Enum):
    AVAILABLE = "available"
    RESERVED = "reserved"
    CONFIRMED = "confirmed"

class Slot(BaseModel):
    id: str
    trainer_id: str
    start_time: datetime
    end_time: datetime
    status: SlotStatus = SlotStatus.AVAILABLE
    user_id: Optional[str] = None

class Appointment(BaseModel):
    slot_id: str
    user_id: str
    trainer_id: str
    start_time: datetime
    end_time: datetime
    status: SlotStatus