from fastapi import APIRouter
from models import slot_reserved_event_model
from controllers.appointment_controller import make_reservation

router = APIRouter(prefix="/make_reservation", tags=["appointment"])

@router.post("/make_reservation")
async def get_reservation(reservation_data: slot_reserved_event_model):
    # Reservation verisini almak ve işlemi başlatmak için make_reservation fonksiyonu çağrılır
    response = make_reservation(reservation_data.dict())  # reservation_data'yı sözlük olarak gönderiyoruz
    return response
