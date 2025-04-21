from enum import Enum
from datetime import datetime
from typing import Optional
from pydantic import BaseModel




class Slot(BaseModel):
    id: str
    trainer_id: str
    start_time: datetime = datetime.utcnow()
    end_time: datetime = datetime.utcnow()
    status: str
    
    #user_id: Optional[str] = None