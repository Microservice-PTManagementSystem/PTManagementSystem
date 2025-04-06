# PTManagementSystem/appointment/src/app/domain/events.py
from datetime import datetime
from pydantic import BaseModel
from typing import Optional

class SlotReservedEvent(BaseModel):
    slot_id: str
    user_id: str
    timestamp: datetime = datetime.utcnow()

class SlotConfirmedEvent(BaseModel):
    slot_id: str
    user_id: str
    timestamp: datetime = datetime.utcnow()

class SlotReleasedEvent(BaseModel):
    slot_id: str
    user_id: Optional[str]
    reason: str
    timestamp: datetime = datetime.utcnow()

class AppointmentCompletedEvent(BaseModel):
    slot_id: str
    user_id: str
    trainer_id: str
    timestamp: datetime = datetime.utcnow()

class AppointmentRescheduledEvent(BaseModel):
    old_slot_id: str
    new_slot_id: str
    user_id: str
    trainer_id: str
    timestamp: datetime = datetime.utcnow()

class AppointmentCanceledEvent(BaseModel):
    slot_id: str
    user_id: str
    trainer_id: str
    reason: str
    timestamp: datetime = datetime.utcnow()