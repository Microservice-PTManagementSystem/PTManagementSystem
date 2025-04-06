from pydantic import BaseModel

class notification_model(BaseModel):
    id: str
    data: str
    created_date: str
    recipient_id: str

