from enum import Enum
from datetime import datetime
from typing import Optional
from pydantic import BaseModel

import datetime
import appointment.app.models.slot_status_model as slot_status_model

class Slot(BaseModel):
    id: str
    trainer_id: str
    start_time: datetime
    end_time: datetime
    status: slot_status_model = slot_status_model.AVAILABLE
    user_id: Optional[str] = None