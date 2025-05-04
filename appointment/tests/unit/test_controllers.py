from datetime import datetime, timedelta, timezone
import pytest
from unittest.mock import patch, MagicMock
from app.controllers.appointment_controller import (
    make_reservation,
    create_slot,
    cancel_appointment,
    get_available_slots,
    get_appointments_with_user_id,
)


@pytest.fixture
def mock_mongodb():
    """Fixture for mocking MongoDB collection."""
    with patch("app.controllers.appointment_controller.get_collection") as mock:
        mock.return_value = MagicMock()
        yield mock


@pytest.fixture
def mock_slot_reserved_event():
    """Fixture for mocking slot_reserved_event."""
    with patch("app.controllers.appointment_controller.slot_reserved_event") as mock:
        yield mock


@pytest.fixture
def mock_appointment_canceled_event():
    """Fixture for mocking appointment_canceled_event."""
    with patch("app.controllers.appointment_controller.appointment_canceled_event") as mock:
        yield mock


@pytest.fixture
def reservation_data():
    """Fixture for reservation data."""
    return {
        "slot_id": "123",
        "user_id": "user1",
        "timestamp": datetime.now(timezone.utc),
    }


@pytest.fixture
def slot_data():
    """Fixture for slot data."""
    return {
        "trainer_id": "trainer1",
        "daily_working_start_hour": "09:00",
        "daily_working_end_hour": "17:00",
        "start_date": datetime.now(timezone.utc),
        "end_date": datetime.now(timezone.utc),
    }


@pytest.fixture
def cancellation_data():
    """Fixture for cancellation data."""
    return {
        "slot_id": "123",
        "user_id": "user1",
        "trainer_id": "trainer1",
        "reason": "schedule conflict",
        "timestamp": datetime.now(timezone.utc),
    }


@pytest.fixture
def available_slots_data():
    """Fixture for available slots data."""
    return {
        "trainer_id": "trainer1",
        "start_date": datetime.now(timezone.utc).date(),
        "end_date": datetime.now(timezone.utc).date(),
    }


@pytest.fixture
def user_id_data():
    """Fixture for user ID data."""
    return {"user_id": "user1"}


class TestAppointmentController:
    """Tests for appointment controller functions."""

    def test_make_reservation_success(self, mock_slot_reserved_event, reservation_data):
        result = make_reservation(reservation_data)
        assert result["data"] == reservation_data
        mock_slot_reserved_event.assert_called_once_with(reservation_data)

    def test_create_slot_success(self, mock_mongodb, slot_data):
        mock_collection = mock_mongodb.return_value
        result = create_slot(slot_data)
        assert result["response"] == 200
        mock_mongodb.assert_called_once_with("SlotDB")
        assert mock_collection.insert_one.called

    def test_cancel_appointment_success(self, mock_appointment_canceled_event, cancellation_data):
        result = cancel_appointment(cancellation_data)
        assert result["data"] == cancellation_data
        mock_appointment_canceled_event.assert_called_once_with(cancellation_data)

    def test_get_available_slots_success(self, mock_mongodb, available_slots_data):
        mock_collection = mock_mongodb.return_value
        mock_collection.find.return_value = [
            {
                "slot_id": "123",
                "trainer_id": "trainer1",
                "slot_status": "released",
                "start_time": datetime.now(timezone.utc),
                "end_time": datetime.now(timezone.utc) + timedelta(hours=1),
            }
        ]
        result = get_available_slots(available_slots_data)
        assert len(result) == 1
        assert result[0]["slot_id"] == "123"
        assert "_id" not in result[0]
        mock_mongodb.assert_called_once_with("SlotDB")

    def test_get_appointments_with_user_id_success(self, mock_mongodb, user_id_data):
        mock_appointment_collection = MagicMock()
        mock_slot_collection = MagicMock()
        mock_mongodb.side_effect = [mock_appointment_collection, mock_slot_collection]

        mock_appointment_collection.find.return_value = [
            {"slot_id": "123", "user_id": "user1", "status": "active"}
        ]
        mock_slot_collection.find_one.return_value = {
            "slot_id": "123",
            "trainer_id": "trainer1",
            "start_time": datetime.now(timezone.utc),
        }

        result = get_appointments_with_user_id(user_id_data, "active")
        assert len(result) == 1
        assert result[0]["slot_id"] == "123"
        assert "slot_detail" in result[0]
        assert result[0]["slot_detail"]["trainer_id"] == "trainer1"
        mock_mongodb.assert_any_call("AppointmentDB")
        mock_mongodb.assert_any_call("SlotDB")

    def test_get_available_slots_no_slots(self, mock_mongodb, available_slots_data):
        mock_collection = mock_mongodb.return_value
        mock_collection.find.return_value = []
        result = get_available_slots(available_slots_data)
        assert result == []
        mock_mongodb.assert_called_once_with("SlotDB")