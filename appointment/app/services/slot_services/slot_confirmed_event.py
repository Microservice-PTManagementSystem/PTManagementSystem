"""import pika
import json

def callback(ch, method, properties, body):
    try:
        print("Received PaymentSucceededEvent", flush=True)
        message = json.loads(body)
        print("Message content:", message, flush=True)

        # Örnek işlem: mesaj içeriğini kullanarak bir işlem yapılabilir
        # Örneğin, bir log kaydı, veritabanı kaydı veya başka bir servise istek gönderme
        # payment_id = message.get("payment_id")
        # user_id = message.get("user_id")
        # print(f"Payment succeeded for user {user_id} with payment ID {payment_id}")

        # Mesaj başarılı şekilde işlendiğinde RabbitMQ'ya onay gönder
        ch.basic_ack(delivery_tag=method.delivery_tag)

    except Exception as e:
        print(f"Error processing message: {e}", flush=True)
        # İstersen burada `basic_nack` ile mesajı reddedebilirsin:
        # ch.basic_nack(delivery_tag=method.delivery_tag, requeue=False)

def payment_succeeded(message):
    print(message, flush=True)

def start_consumer():
    try:
        connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
        print("Connected to RabbitMQ", flush=True)  
        channel = connection.channel()
        channel.queue_declare(queue='PaymentSucceededEvent' , durable=True , exclusive=False, auto_delete=False)
        channel.basic_qos(prefetch_count=1)  # Her seferinde bir mesaj al

        channel.basic_consume(queue='PaymentSucceededEvent', on_message_callback=callback)

        print("Waiting for payment event...", flush=True)
        channel.start_consuming()
    except Exception as e:
        print(f"Error connecting to RabbitMQ: {e}", flush=True)

if __name__ == "__main__":
    start_consumer()"""


import pika
import json
import threading
import uvicorn
from fastapi import FastAPI
from services.db_services.mongodb_service import get_collection
from services.appointment_services.appointment_completed_event import appointment_completed_event
from services.appointment_services.appointment_failed_event import appointment_failed_event

app = FastAPI()

appointment_collection = get_collection("AppointmentDB")
def succeeded_callback(ch, method, properties, body):
    try:
        print("Received PaymentSucceededEvent", flush=True)
        message = json.loads(body)
        print("Message content:", message, flush=True)
        appointment_collection.update_one(
            {"slot_id": message["slotId"]},
            {"$set": {"status": "active"}}
        )
        ch.basic_ack(delivery_tag=method.delivery_tag)

        print("Published appointment_completed_event", flush=True)
        appointment_completed_event(message)
    except Exception as e:
        print(f"Error processing message: {e}", flush=True)

def failed_callback(ch, method, properties, body):
    try:
        print("Received PaymentFailedEvent", flush=True)
        message = json.loads(body)
        print("Message content:", message, flush=True)
        appointment_collection.update_one(
            {"slot_id": message["slotId"]},
            {"$set": {"status": "released"}}
        )
        ch.basic_ack(delivery_tag=method.delivery_tag)

        print("Published appointment_failed_event", flush=True)
        appointment_failed_event(message)
    except Exception as e:
        print(f"Error processing message: {e}", flush=True)

def start_payment_succeeded_consumer():
    try:
        connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
        print("Connected to RabbitMQ", flush=True)
        channel = connection.channel()
        channel.queue_declare(queue='PaymentSucceededEvent', durable=True, exclusive=False, auto_delete=False)
        channel.basic_qos(prefetch_count=1)
        channel.basic_consume(queue='PaymentSucceededEvent', on_message_callback=succeeded_callback)
        print("Waiting for payment event...", flush=True)
        channel.start_consuming()
    except Exception as e:
        print(f"Error connecting to RabbitMQ: {e}", flush=True)


def start_payment_failed_consumer():
    try:
        connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
        print("Connected to RabbitMQ", flush=True)
        channel = connection.channel()
        channel.queue_declare(queue='PaymentFailedEvent', durable=True, exclusive=False, auto_delete=False)
        channel.basic_qos(prefetch_count=1)
        channel.basic_consume(queue='PaymentFailedEvent', on_message_callback=failed_callback)
        print("Waiting for payment event...", flush=True)
        channel.start_consuming()
    except Exception as e:
        print(f"Error connecting to RabbitMQ: {e}", flush=True)

if __name__ == "__main__":
    # Her bir consumer için ayrı thread başlat
    succeeded_thread = threading.Thread(target=start_payment_succeeded_consumer, daemon=True)
    failed_thread = threading.Thread(target=start_payment_failed_consumer, daemon=True)

    succeeded_thread.start()
    failed_thread.start()

    print("Both consumers started. Running FastAPI app...", flush=True)

    # FastAPI uygulamasını başlat
    uvicorn.run(app, host="0.0.0.0", port=8000)
