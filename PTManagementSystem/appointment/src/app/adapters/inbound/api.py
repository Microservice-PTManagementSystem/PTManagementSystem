# PTManagementSystem/appointment/src/app/adapters/inbound/api.py
from fastapi import APIRouter, HTTPException
from typing import List
from domain.models import Slot, SlotStatus
from ports.inbound.slot_service import SlotServicePort
from datetime import datetime

router = APIRouter()
slot_service: SlotServicePort = None

@router.post("/slots", response_model=Slot)
async def create_slot(slot: Slot):
    return await slot_service.create_slot(slot)

@router.get("/slots/available", response_model=List[Slot])
async def get_available_slots():
    return await slot_service.get_available_slots()

@router.post("/slots/{slot_id}/reserve")
async def reserve_slot(slot_id: str, user_id: str):
    try:
        slot = await slot_service.reserve_slot(slot_id, user_id)
        return slot
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))

@router.post("/slots/{slot_id}/confirm")
async def confirm_slot(slot_id: str):
    try:
        slot = await slot_service.confirm_slot(slot_id)
        return slot
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))

@router.post("/slots/{slot_id}/release")
async def release_slot(slot_id: str, reason: str):
    try:
        slot = await slot_service.release_slot(slot_id, reason)
        return slot
    except Exception as e:
        raise HTTPException(status_code=400, detail=str(e))