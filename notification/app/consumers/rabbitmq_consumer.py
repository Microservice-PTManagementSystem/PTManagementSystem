import pika
import json

def callback(ch, method, properties, body):
    reservation_data = json.loads(body)
    print(f"Received reservation event: {reservation_data}")
    
    process_reservation_notification(reservation_data)
    
    ch.basic_ack(delivery_tag=method.delivery_tag)

def process_reservation_notification(reservation_data):
    print(f"reservation date: {reservation_data['date']}")

def start_consumer():
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()

    channel.queue_declare(queue='reservationQueue') 
    
    channel.basic_consume(queue='reservationQueue', on_message_callback=callback)
    
    print("Waiting for reservation events...")
    channel.start_consuming()

if __name__ == "__main__":
    start_consumer()
