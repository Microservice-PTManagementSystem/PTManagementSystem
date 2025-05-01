import pytest
from pydantic import ValidationError
from app.models.notification_model import notification_model

def test_notification_model_valid():
    notification_data = {
        "id": "123",
        "data": "Your appointment has been confirmed",
        "created_date": "2024-01-20",
        "recipient_id": "user1"
    }
    
    notification = notification_model(**notification_data)
    assert notification.id == "123"
    assert notification.data == "Your appointment has been confirmed"
    assert notification.recipient_id == "user1"

def test_invalid_notification_model():
    with pytest.raises(ValidationError):
        notification_model(id="123")  # Missing required fields