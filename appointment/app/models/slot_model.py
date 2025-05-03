from enum import Enum
from datetime import  date , time 
from typing import Optional
from pydantic import BaseModel




class Slot(BaseModel):
    trainer_id: str
    daily_working_start_hour : str
    daily_working_end_hour : str
    start_date: date
    end_date: date

   
class GetSlot(BaseModel):
    trainer_id: str
    start_date: date
    end_date: date

class UserId(BaseModel):
    user_id: str
