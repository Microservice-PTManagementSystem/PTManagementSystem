from pydantic import BaseModel

class reservation_model(BaseModel):
    id: str
    appointment_date: str
    appointment_time: str
    created_date: str
    trainer_id: str
    customer_id: str
    

