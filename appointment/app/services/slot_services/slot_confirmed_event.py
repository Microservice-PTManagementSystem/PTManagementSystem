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

app = FastAPI()

appointment_collection = get_collection("AppointmentDB")
def callback(ch, method, properties, body):
    try:
        print("Received PaymentSucceededEvent", flush=True)
        message = json.loads(body)
        print("Message content:", message, flush=True)
        appointment_collection.update_one(
            {"slot_id": message["SlotId"]},
            {"$set": {"status": "active"}}
        )
        ch.basic_ack(delivery_tag=method.delivery_tag)
    except Exception as e:
        print(f"Error processing message: {e}", flush=True)

def start_consumer():
    try:
        connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
        print("Connected to RabbitMQ", flush=True)
        channel = connection.channel()
        channel.queue_declare(queue='PaymentSucceededEvent', durable=True, exclusive=False, auto_delete=False)
        channel.basic_qos(prefetch_count=1)
        channel.basic_consume(queue='PaymentSucceededEvent', on_message_callback=callback)
        print("Waiting for payment event...", flush=True)
        channel.start_consuming()
    except Exception as e:
        print(f"Error connecting to RabbitMQ: {e}", flush=True)

if __name__ == "__main__":
    # Consumer'ı ayrı bir thread'de çalıştır
    consumer_thread = threading.Thread(target=start_consumer)
    consumer_thread.daemon = True  # Ana uygulama kapanırken thread de kapansın
    consumer_thread.start()


