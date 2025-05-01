import pytest
from unittest.mock import patch
from datetime import datetime
from app.services.db_services.mongodb_service import get_collection

@pytest.mark.integration
def test_mongodb_connection():
    with patch('app.services.db_services.mongodb_service.MongoClient') as mock_client:
        collection = get_collection("SlotDB")
        assert collection is not None
        assert mock_client.called

@pytest.mark.integration
def test_mongodb_slot_operations():
    slot_data = {
        "id": "123",
        "trainer_id": "trainer1",
        "start_time": datetime.utcnow(),
        "end_time": datetime.utcnow(),
        "status": "available"
    }
    
    with patch('app.services.db_services.mongodb_service.MongoClient') as mock_client:
        mock_collection = mock_client.return_value.__getitem__.return_value.__getitem__.return_value
        
        collection = get_collection("SlotDB")
        collection.insert_one(slot_data)
        
        mock_collection.insert_one.assert_called_once_with(slot_data)