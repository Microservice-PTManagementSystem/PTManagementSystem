import pika
import json
from datetime import datetime

def datetime_serializer(obj):
    if isinstance(obj, datetime):
        return obj.isoformat()  # Converts datetime to ISO format string
    raise TypeError("Type not serializable")

def slot_reserved_event(reservation_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()

    channel.queue_declare(queue='reservationQueue')

    # Use custom serializer for datetime objects
    message = json.dumps(reservation_data, default=datetime_serializer)

    channel.basic_publish(exchange='', routing_key='reservationQueue', body=message)

    print(" [x] Sent 'reservationMade' event")
    connection.close()

