# PTManagementSystem/appointment/src/app/ports/inbound/slot_service.py
from abc import ABC, abstractmethod
from typing import List
from domain.models import Slot

class SlotServicePort(ABC):
    @abstractmethod
    async def create_slot(self, slot: Slot) -> Slot:
        pass

    @abstractmethod
    async def get_available_slots(self) -> List[Slot]:
        pass

    @abstractmethod
    async def reserve_slot(self, slot_id: str, user_id: str) -> Slot:
        pass

    @abstractmethod
    async def confirm_slot(self, slot_id: str) -> Slot:
        pass

    @abstractmethod
    async def release_slot(self, slot_id: str, reason: str) -> Slot:
        pass

    @abstractmethod
    async def complete_appointment(self, slot_id: str) -> Slot:
        pass