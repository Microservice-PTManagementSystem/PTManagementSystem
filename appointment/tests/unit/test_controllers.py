from datetime import datetime
import pytest
from unittest.mock import patch, MagicMock
from app.controllers.appointment_controller import make_reservation, create_slot, cancel_appointment

@pytest.fixture
def mock_slot_reserved_event():
    with patch('app.controllers.appointment_controller.slot_reserved_event') as mock:
        yield mock

@pytest.fixture
def mock_appointment_canceled_event():
    with patch('app.controllers.appointment_controller.appointment_canceled_event') as mock:
        yield mock

@pytest.fixture
def mock_mongodb():
    with patch('app.controllers.appointment_controller.get_collection') as mock:
        yield mock

def test_make_reservation(mock_slot_reserved_event):
    reservation_data = {
        "slot_id": "123",
        "user_id": "user1",
        "timestamp": datetime.utcnow()
    }
    
    result = make_reservation(reservation_data)
    
    assert result["status"] == "success"
    assert result["data"] == reservation_data
    mock_slot_reserved_event.assert_called_once_with(reservation_data)

def test_create_slot(mock_mongodb):
    slot_data = {
        "id": "123",
        "trainer_id": "trainer1",
        "start_time": datetime.utcnow(),
        "end_time": datetime.utcnow(),
        "status": "available"
    }
    
    result = create_slot(slot_data)
    
    assert result["status"] == "success"
    assert result["data"] == slot_data
    mock_mongodb.assert_called_once_with("SlotDB")
    mock_mongodb.return_value.insert_one.assert_called_once()

def test_cancel_appointment(mock_appointment_canceled_event):
    cancellation_data = {
        "slot_id": "123",
        "user_id": "user1",
        "trainer_id": "trainer1",
        "reason": "schedule conflict",
        "timestamp": datetime.utcnow()
    }
    
    result = cancel_appointment(cancellation_data)
    
    assert result["status"] == "success"
    assert result["data"] == cancellation_data
    mock_appointment_canceled_event.assert_called_once_with(cancellation_data)