import pytest
from app.controllers.notification_controller import send_notification1
from app.models.notification_model import notification_model

def test_send_notification():
    notification_data = notification_model(
        id="123",
        data="Your appointment has been confirmed",
        created_date="2024-01-20",
        recipient_id="user1"
    )
    
    result = send_notification1(notification_data)
    assert result == notification_data