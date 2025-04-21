from fastapi import APIRouter
from models.event_models.slot_reserved_event_model import slot_reserved_event_model
from models.slot_model import Slot
from controllers.appointment_controller import make_reservation , create_slot

router = APIRouter(prefix="/make_reservation", tags=["appointment"])

@router.post("/make_reservation")
async def get_reservation(reservation_data: slot_reserved_event_model):
    response = make_reservation(reservation_data.dict())  
    return response

@router.post("/add_slot")
async def get_reservation(slot_data: Slot):
    response = create_slot(Slot.dict())  
    return response
