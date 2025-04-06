from pydantic import BaseModel
from enum import Enum
from datetime import datetime
from typing import Optional
from pydantic import BaseModel
import appointment.app.models.slot_status_model as slot_status_model

class Appointment(BaseModel):
    slot_id: str
    user_id: str
    trainer_id: str
    start_time: datetime
    end_time: datetime
    status: slot_status_model