from fastapi.testclient import TestClient
from datetime import datetime, UTC
from unittest.mock import patch
from main import app
import json

client = TestClient(app)

@patch("main.start_consumer")
@patch("pika.BlockingConnection")
def test_make_reservation_endpoint(mock_pika, mock_consumer):
    mock_pika.return_value.channel.return_value.queue_declare.return_value = None
    mock_pika.return_value.channel.return_value.basic_publish.return_value = None
    reservation_data = {
        "slot_id": "123",
        "user_id": "user1",
        "timestamp": datetime.now(UTC).isoformat()
    }
    
    response = client.post("/make_reservation/make_reservation", json=reservation_data)
    assert response.status_code == 200
    #assert response.json()["status"] == "success"
    assert "data" in response.json()

"""@patch("main.start_consumer")
@patch("pika.BlockingConnection")
def test_add_slot_endpoint(mock_pika, mock_consumer):
    mock_pika.return_value.channel.return_value.queue_declare.return_value = None
    mock_pika.return_value.channel.return_value.basic_publish.return_value = None
    slot_data = {
        "trainer_id": "4",
        "daily_working_start_hour": "09:00",
        "daily_working_end_hour": "17:00",
        "start_date": "2025-05-04",
        "end_date": "2025-05-09"
}
    response = client.post("/make_reservation/add_slot", json=slot_data)
    print("RESPONSE::::::  ",response.text)
    #response = json.loads(str(response.text))
    assert response.text['response'] == 200
    #assert response.json()["status"] == "success"
    assert "data" in response.json()
"""
@patch("main.start_consumer")
@patch("pika.BlockingConnection")
def test_cancel_appointment_endpoint(mock_pika, mock_consumer):
    mock_pika.return_value.channel.return_value.queue_declare.return_value = None
    mock_pika.return_value.channel.return_value.basic_publish.return_value = None
    cancellation_data = {
        "slot_id": "123",
        "user_id": "user1",
        "trainer_id": "trainer1",
        "reason": "schedule conflict",
        "timestamp": datetime.now(UTC).isoformat()
    }
    
    response = client.post("/make_reservation/cancel_appointment", json=cancellation_data)
    assert response.status_code == 200
    #assert response.json()["status"] == "success"
    assert "data" in response.json()