from datetime import datetime
import pytest
from pydantic import ValidationError
from app.models.slot_model import Slot
from app.models.event_models.slot_reserved_event_model import slot_reserved_event_model
from app.models.event_models.appointment_canceled_event_model import AppointmentCanceledEvent

def test_slot_model_valid():
    slot_data = {
        "id": "123",
        "trainer_id": "trainer1",
        "start_time": datetime.utcnow(),
        "end_time": datetime.utcnow(),
        "status": "available"
    }
    slot = Slot(**slot_data)
    assert slot.id == "123"
    assert slot.trainer_id == "trainer1"
    assert slot.status == "available"

def test_slot_reserved_event_model_valid():
    event_data = {
        "slot_id": "123",
        "user_id": "user1",
        "timestamp": datetime.utcnow()
    }
    event = slot_reserved_event_model(**event_data)
    assert event.slot_id == "123"
    assert event.user_id == "user1"

def test_appointment_canceled_event_model_valid():
    event_data = {
        "slot_id": "123",
        "user_id": "user1",
        "trainer_id": "trainer1",
        "reason": "schedule conflict",
        "timestamp": datetime.utcnow()
    }
    event = AppointmentCanceledEvent(**event_data)
    assert event.slot_id == "123"
    assert event.reason == "schedule conflict"

def test_invalid_slot_model():
    with pytest.raises(ValidationError):
        Slot(id="123")  # Missing required fields