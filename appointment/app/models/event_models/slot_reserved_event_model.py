from datetime import datetime
from pydantic import BaseModel


class slot_reserved_event_model(BaseModel):
    cardNumber : str
    message: str
    cardNumber: str
    cardHolder: str
    expiryMonth: str
    expiryYear: str
    cvc: str
    saveCard : bool
    totalAmount : str
    paymentMethod : str
    slot_id: str
    user_id: str
    timestamp: datetime = datetime.utcnow()
