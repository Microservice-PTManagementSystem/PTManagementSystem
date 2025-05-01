from fastapi.testclient import TestClient
from main import app  # Changed from app.main
from unittest.mock import patch

client = TestClient(app)

@patch("main.start_consumer")  # Updated path
@patch("main.start_cancel_consumer")  # Updated path
def test_send_notification_endpoint(mock_cancel_consumer, mock_consumer):
    notification_data = {
        "id": "123",
        "data": "Your appointment has been confirmed",
        "created_date": "2024-01-20",
        "recipient_id": "user1"
    }
    
    response = client.post("/send_notification/send_notification", json=notification_data)
    assert response.status_code == 200
    assert "response" in response.json()
    assert response.json()["response"] == notification_data