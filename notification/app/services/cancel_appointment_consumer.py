import pika
import json

def callback(ch, method, properties, body):
    try:
        print(f"Received cancel appointment event: ", flush=True)
        message = json.loads(body)
        print("Message content:", message, flush=True)

        print("Message Received and notification sending... (BURASINI TAMAMLA!!!!)") 

        ch.basic_ack(delivery_tag=method.delivery_tag)

        
        
    except Exception as e:
        print(f"Error Notifiy service consumer appointmentCanceled message: {e}", flush=True)
    
    
    
    #process_reservation_notification(reservation_data)

"""def process_reservation_notification(reservation_data):
    print(f"Reservation date: {reservation_data['appointment_date']}", flush=True)"""
    
def start_cancel_consumer():
    try:
        connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
        print("Connected to RabbitMQ", flush=True)  
        channel = connection.channel()
        channel.queue_declare(queue='cancellationQueue') 

        channel.basic_consume(queue='cancellationQueue', on_message_callback=callback)

        print("Waiting for cancel appointment events...", flush=True)
        channel.start_consuming()
    except Exception as e:
        print(f"Error connecting to RabbitMQ: {e}", flush=True)

if __name__ == "__main__":
    start_cancel_consumer()
