import pika
import json
from services.db_services.mongodb_service import get_collection

def appointment_canceled_event(cancellation_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='cancellationQueue')


    appointment_collection = get_collection("AppointmentDB")
    appointment_collection.update_one({"slot_id": cancellation_data["slot_id"]}, {"$set": {"status": "cancelled"}})
    appointment_collection.update_one({"slot_id": cancellation_data["slot_id"]}, {"$set": {"reason": cancellation_data["reason"]}})

    slot_collection = get_collection("SlotDB")
    slot_collection.update_one({"slot_id": cancellation_data["slot_id"]}, {"$set": {"status": "released"}})
    # RabbitMQ'ya mesaj gönderme
    message = json.dumps(cancellation_data, default=str)

    channel.basic_publish(exchange='', routing_key='cancellationQueue', body=message)

    print("[x] Sent 'appointmentCanceled' event")
    connection.close()
    
