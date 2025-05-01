import pika
import json
import copy
from datetime import datetime
from services.db_services.mongodb_service import get_collection
def datetime_serializer(obj):
    if isinstance(obj, datetime):
        return obj.isoformat() 
    raise TypeError("Type not serializable")

def slot_reserved_event(reservation_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='reservationQueue')
    mongo_insert_data = reservation_data.copy()

    collection = get_collection("AppointmentDB")
    collection.insert_one(mongo_insert_data)

    message = json.dumps(reservation_data, default=datetime_serializer)

    channel.basic_publish(exchange='', routing_key='reservationQueue', body=message)
    print(" [x] Sent 'reservationMade' event")
    connection.close()


