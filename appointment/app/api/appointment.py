from fastapi import APIRouter
from models.event_models.slot_reserved_event_model import slot_reserved_event_model
from models.slot_model import Slot
from controllers.appointment_controller import make_reservation , create_slot, cancel_appointment
from models.event_models.appointment_canceled_event_model import AppointmentCanceledEvent


router = APIRouter(prefix="/make_reservation", tags=["appointment"])

@router.post("/make_reservation")
async def get_reservation(reservation_data: slot_reserved_event_model):
    response = make_reservation(reservation_data.dict())  
    return response

@router.post("/add_slot")
async def get_reservation(slot_data: Slot):
    response = create_slot(slot_data.dict())
 
    return response

@router.post("/cancel_appointment")
async def cancel_reservation(cancellation_data: AppointmentCanceledEvent):
    response = cancel_appointment(cancellation_data.dict())  
    return response
