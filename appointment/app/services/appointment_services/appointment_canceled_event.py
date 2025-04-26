import pika
import json
from services.db_services.mongodb_service import get_collection

def appointment_canceled_event(cancellation_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='cancellationQueue')

    # MongoDB'den silme işlemi
    collection = get_collection("AppointmentDB")

    slot_id = cancellation_data["slot_id"]

    delete_query = {"slot_id": slot_id}

    result = collection.delete_one(delete_query)

    if result.deleted_count > 0:
        print("[x] Appointment successfully deleted from MongoDB.")
    else:
        print("[!] No matching appointment found to delete.")

    # RabbitMQ'ya mesaj gönderme
    message = json.dumps(cancellation_data, default=str)

    channel.basic_publish(exchange='', routing_key='cancellationQueue', body=message)

    print("[x] Sent 'appointmentCanceled' event")
    connection.close()
