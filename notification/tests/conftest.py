import pytest
from app.models.notification_model import notification_model

@pytest.fixture
def sample_notification():
    return notification_model(
        id="123",
        data="Your appointment has been confirmed",
        created_date="2024-01-20",
        recipient_id="user1"
    )
