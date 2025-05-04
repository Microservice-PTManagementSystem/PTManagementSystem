import pytest
from unittest.mock import patch
from datetime import datetime, UTC
from app.services.slot_services.slot_reserved_event import slot_reserved_event
from app.services.appointment_services.appointment_canceled_event import appointment_canceled_event

@pytest.mark.integration
def test_slot_reserved_event_integration():
    event_data = {
        "slot_id": "123",
        "user_id": "user1",
        "timestamp": datetime.now(UTC).isoformat()
    }
    
    with patch('pika.BlockingConnection') as mock_connection:
        mock_channel = mock_connection.return_value.channel.return_value
        mock_channel.queue_declare.return_value = None
        mock_channel.basic_publish.return_value = None
        
        slot_reserved_event(event_data)
        
        mock_channel.queue_declare.assert_called_once_with(queue='reservationQueue')
        mock_channel.basic_publish.assert_called_once()
        mock_connection.return_value.close.assert_called_once()

@pytest.mark.integration
def test_appointment_canceled_event_integration():
    event_data = {
        "slot_id": "123",
        "user_id": "user1",
        "trainer_id": "trainer1",
        "reason": "schedule conflict",
        "timestamp": datetime.now(UTC).isoformat()
    }
    
    with patch('pika.BlockingConnection') as mock_connection:
        mock_channel = mock_connection.return_value.channel.return_value
        mock_channel.queue_declare.return_value = None
        mock_channel.basic_publish.return_value = None
        
        appointment_canceled_event(event_data)
        
        mock_channel.queue_declare.assert_called_once_with(queue='cancellationQueue')
        mock_channel.basic_publish.assert_called_once()
        mock_connection.return_value.close.assert_called_once()