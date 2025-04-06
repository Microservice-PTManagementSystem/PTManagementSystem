# PTManagementSystem/appointment/src/app/application/slot_service.py
from typing import List, Dict
from datetime import datetime
from domain.models import Slot, SlotStatus
from ports.inbound.slot_service import SlotServicePort
from ports.outbound.event_publisher import EventPublisherPort
from domain.events import (
    SlotReservedEvent, SlotConfirmedEvent, SlotReleasedEvent,
    AppointmentCompletedEvent, AppointmentRescheduledEvent
)

class SlotService(SlotServicePort):
    def __init__(self, event_publisher: EventPublisherPort):
        self.event_publisher = event_publisher
        # In-memory storage for slots
        self.slots: Dict[str, Slot] = {}
        
        # Initialize some sample slots
        sample_slots = [
            Slot(
                id=f"slot_{i}",
                trainer_id="trainer_1",
                start_time=datetime(2024, 2, 1, 10 + i, 0),
                end_time=datetime(2024, 2, 1, 11 + i, 0),
                status=SlotStatus.AVAILABLE
            ) for i in range(5)
        ]
        for slot in sample_slots:
            self.slots[slot.id] = slot

    async def create_slot(self, slot: Slot) -> Slot:
        self.slots[slot.id] = slot
        return slot

    async def get_available_slots(self) -> List[Slot]:
        return [slot for slot in self.slots.values() if slot.status == SlotStatus.AVAILABLE]

    async def reserve_slot(self, slot_id: str, user_id: str) -> Slot:
        if slot_id not in self.slots:
            raise ValueError("Slot not found")
            
        slot = self.slots[slot_id]
        if slot.status != SlotStatus.AVAILABLE:
            raise ValueError("Slot is not available")
            
        slot.status = SlotStatus.RESERVED
        slot.user_id = user_id
        
        await self.event_publisher.publish_slot_reserved(
            SlotReservedEvent(slot_id=slot_id, user_id=user_id)
        )
        return slot

    async def confirm_slot(self, slot_id: str) -> Slot:
        if slot_id not in self.slots:
            raise ValueError("Slot not found")
            
        slot = self.slots[slot_id]
        if slot.status != SlotStatus.RESERVED:
            raise ValueError("Slot is not reserved")
            
        slot.status = SlotStatus.CONFIRMED
        
        await self.event_publisher.publish_slot_confirmed(
            SlotConfirmedEvent(slot_id=slot_id, user_id=slot.user_id)
        )
        return slot

    async def release_slot(self, slot_id: str, reason: str) -> Slot:
        if slot_id not in self.slots:
            raise ValueError("Slot not found")
            
        slot = self.slots[slot_id]
        old_user_id = slot.user_id
        
        slot.status = SlotStatus.AVAILABLE
        slot.user_id = None
        
        await self.event_publisher.publish_slot_released(
            SlotReleasedEvent(slot_id=slot_id, user_id=old_user_id, reason=reason)
        )
        return slot

    async def complete_appointment(self, slot_id: str) -> Slot:
        if slot_id not in self.slots:
            raise ValueError("Slot not found")
            
        slot = self.slots[slot_id]
        if slot.status != SlotStatus.CONFIRMED:
            raise ValueError("Appointment is not confirmed")
            
        await self.event_publisher.publish_appointment_completed(
            AppointmentCompletedEvent(
                slot_id=slot_id,
                user_id=slot.user_id,
                trainer_id=slot.trainer_id
            )
        )
        return slot