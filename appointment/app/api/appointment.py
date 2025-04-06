from fastapi import APIRouter
from models.reservation_model import reservation_model
from controllers.appointment_controller import make_reservation

router = APIRouter(prefix="/make_reservation", tags=["appointment"])


@router.post("/make_reservation")
async def get_reservation(reservation_data: reservation_model):
    response = make_reservation(reservation_data)
    return response
