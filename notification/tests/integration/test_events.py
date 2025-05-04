import pytest
from unittest.mock import patch, MagicMock, ANY  
from app.services.rabbitmq_consumer import start_consumer
from app.services.cancel_appointment_consumer import start_cancel_consumer

@pytest.mark.integration
def test_rabbitmq_consumer_integration():
    
    with patch('pika.BlockingConnection') as mock_connection:
        mock_channel = MagicMock()
        mock_connection.return_value.channel.return_value = mock_channel
        
        start_consumer()

        mock_channel.basic_consume.assert_called_once_with(queue='appointmentCompleted', on_message_callback=ANY)

@pytest.mark.integration
def test_cancel_appointment_consumer_integration():
    with patch('pika.BlockingConnection') as mock_connection:
        mock_channel = MagicMock()
        mock_connection.return_value.channel.return_value = mock_channel
        
        start_cancel_consumer()

        mock_channel.basic_consume.assert_called_once_with(queue='cancellationQueue', on_message_callback=ANY)
