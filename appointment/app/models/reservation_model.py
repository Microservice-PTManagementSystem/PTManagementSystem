from pydantic import BaseModel

class reservation_model(BaseModel):
    date: str
    trainer_name: str
