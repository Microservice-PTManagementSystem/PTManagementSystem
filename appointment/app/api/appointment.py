from fastapi import APIRouter
from models.event_models.slot_reserved_event_model import slot_reserved_event_model
from models.slot_model import Slot , GetSlot , UserId
from controllers.appointment_controller import make_reservation , create_slot, cancel_appointment , get_available_slots , get_appointments_with_user_id
from models.event_models.appointment_canceled_event_model import AppointmentCanceledEvent


router = APIRouter(prefix="/make_reservation", tags=["appointment"])

@router.post("/make_reservation")
async def make_reservation_(reservation_data: slot_reserved_event_model):
    response = make_reservation(reservation_data.dict())  
    return response

@router.post("/add_slot")
async def add_slot_(slot_data: Slot):
    response = create_slot(slot_data.dict())
    return response

@router.post("/available_slots")
async def available_slots_(get_available_slot: GetSlot):
    response = get_available_slots(get_available_slot.dict())  
    return response

@router.post("/get_cancelled_reservations_by_user_id")
async def get_user_reservations(UserId: UserId):
    response = get_appointments_with_user_id(UserId.dict() , "cancelled")  
    return response

@router.post("/get_active_reservations_by_user_id")
async def get_user_reservations_(UserId: UserId):
    response = get_appointments_with_user_id(UserId.dict() , "active")  
    return response

@router.post("/cancel_appointment")
async def cancel_reservation(cancellation_data: AppointmentCanceledEvent):
    response = cancel_appointment(cancellation_data.dict())  
    return response

