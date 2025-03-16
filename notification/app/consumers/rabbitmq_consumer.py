import pika
import json

def callback(ch, method, properties, body):
    reservation_data = json.loads(body)
    print(f"Received reservation event: {reservation_data}", flush=True)
    
    process_reservation_notification(reservation_data)
    
    ch.basic_ack(delivery_tag=method.delivery_tag)

def process_reservation_notification(reservation_data):
    print(f"Reservation date: {reservation_data['date']}", flush=True)
    
def start_consumer():
    try:
        connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
        print("Connected to RabbitMQ", flush=True)  
        channel = connection.channel()
        channel.queue_declare(queue='reservationQueue') 

        channel.basic_consume(queue='reservationQueue', on_message_callback=callback)

        print("Waiting for reservation events...", flush=True)
        channel.start_consuming()
    except Exception as e:
        print(f"Error connecting to RabbitMQ: {e}", flush=True)

if __name__ == "__main__":
    start_consumer()
