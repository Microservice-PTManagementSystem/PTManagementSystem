import pytest
from datetime import datetime
from app.models.slot_model import Slot
from app.models.event_models.slot_reserved_event_model import slot_reserved_event_model
from app.models.event_models.appointment_canceled_event_model import AppointmentCanceledEvent

@pytest.fixture
def sample_slot():
    return Slot(
        id="123",
        trainer_id="trainer1",
        start_time=datetime.utcnow(),
        end_time=datetime.utcnow(),
        status="available"
    )

@pytest.fixture
def sample_reservation():
    return slot_reserved_event_model(
        slot_id="123",
        user_id="user1",
        timestamp=datetime.utcnow()
    )

@pytest.fixture
def sample_cancellation():
    return AppointmentCanceledEvent(
        slot_id="123",
        user_id="user1",
        trainer_id="trainer1",
        reason="schedule conflict",
        timestamp=datetime.utcnow()
    )