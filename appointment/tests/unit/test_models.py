from datetime import date, datetime, timezone
import pytest
from pydantic import ValidationError
from app.models.slot_model import Slot, GetSlot, UserId
from app.models.event_models.slot_reserved_event_model import slot_reserved_event_model
from app.models.event_models.appointment_canceled_event_model import AppointmentCanceledEvent


@pytest.fixture
def valid_slot_data():
    """Fixture for valid slot data."""
    return {
        "trainer_id": "trainer1",
        "daily_working_start_hour": "09:00",
        "daily_working_end_hour": "17:00",
        "start_date": date.today(),
        "end_date": date.today(),
    }


@pytest.fixture
def valid_slot_reserved_event_data():
    """Fixture for valid slot reserved event data."""
    return {
        "slot_id": "123",
        "user_id": "user1",
        "timestamp": datetime.now(timezone.utc),
    }


@pytest.fixture
def valid_appointment_canceled_event_data():
    """Fixture for valid appointment canceled event data."""
    return {
        "slot_id": "123",
        "user_id": "user1",
        "trainer_id": "trainer1",
        "reason": "schedule conflict",
        "timestamp": datetime.now(timezone.utc),
    }


class TestSlotModel:
    """Tests for Slot model."""

    def test_slot_model_valid(self, valid_slot_data):
        slot = Slot(**valid_slot_data)
        assert slot.trainer_id == valid_slot_data["trainer_id"]
        assert slot.daily_working_start_hour == valid_slot_data["daily_working_start_hour"]

    def test_invalid_slot_model_missing_fields(self):
        with pytest.raises(ValidationError, match="Field required"):
            Slot(trainer_id="123")  # Missing required fields



class TestSlotReservedEventModel:
    """Tests for SlotReservedEvent model."""

    def test_slot_reserved_event_model_valid(self, valid_slot_reserved_event_data):
        event = slot_reserved_event_model(**valid_slot_reserved_event_data)
        assert event.slot_id == valid_slot_reserved_event_data["slot_id"]
        assert event.user_id == valid_slot_reserved_event_data["user_id"]

    def test_invalid_slot_reserved_event_missing_slot_id(self):
        with pytest.raises(ValidationError, match="Field required"):
            slot_reserved_event_model(user_id="user1", timestamp=datetime.now(timezone.utc))


class TestAppointmentCanceledEventModel:
    """Tests for AppointmentCanceledEvent model."""

    def test_appointment_canceled_event_model_valid(self, valid_appointment_canceled_event_data):
        event = AppointmentCanceledEvent(**valid_appointment_canceled_event_data)
        assert event.slot_id == valid_appointment_canceled_event_data["slot_id"]
        assert event.reason == valid_appointment_canceled_event_data["reason"]

    def test_invalid_appointment_canceled_event_missing_reason(self, valid_appointment_canceled_event_data):
        invalid_data = valid_appointment_canceled_event_data.copy()
        invalid_data.pop("reason")
        with pytest.raises(ValidationError, match="Field required"):
            AppointmentCanceledEvent(**invalid_data)


class TestGetSlotModel:
    """Tests for GetSlot model."""

    def test_get_slot_model_valid(self):
        data = {
            "trainer_id": "trainer1",
            "start_date": date.today(),
            "end_date": date.today(),
        }
        slot = GetSlot(**data)
        assert slot.trainer_id == data["trainer_id"]

    def test_invalid_get_slot_model_missing_trainer_id(self):
        with pytest.raises(ValidationError, match="Field required"):
            GetSlot(start_date=date.today(), end_date=date.today())


class TestUserIdModel:
    """Tests for UserId model."""

    def test_user_id_model_valid(self):
        data = {"user_id": "user1"}
        user_id = UserId(**data)
        assert user_id.user_id == data["user_id"]

    def test_invalid_user_id_model_missing_user_id(self):
        with pytest.raises(ValidationError, match="Field required"):
            UserId()