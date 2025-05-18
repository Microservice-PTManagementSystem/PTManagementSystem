import pika
import json

from app.services.db_services.sql_server import insert_notification

def callback(ch, method, properties, body):
    try:
        print(f"Received appointment event: ", flush=True)
        message = json.loads(body)
        print("Message content:", message, flush=True)

      
        user_id = message.get("user_id")
        notif_message = message.get("message", "Appointment Completed")

        insert_notification(user_id, notif_message)

        print("Notification saved to DB!", flush=True)
        ch.basic_ack(delivery_tag=method.delivery_tag)
        
    except Exception as e:
        print(f"Error Notifiy service consumer appointmentCompleted message: {e}", flush=True)

    
    
    
    #process_reservation_notification(reservation_data)

"""def process_reservation_notification(reservation_data):
    print(f"Reservation date: {reservation_data['appointment_date']}", flush=True)"""
    
def start_consumer():
    try:
        connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
        print("Connected to RabbitMQ", flush=True)  
        channel = connection.channel()
        channel.queue_declare(queue='appointmentCompleted') 

        channel.basic_consume(queue='appointmentCompleted', on_message_callback=callback)

        print("Waiting for appointment completed events...", flush=True)
        channel.start_consuming()
    except Exception as e:
        print(f"Error connecting to RabbitMQ: {e}", flush=True)

if __name__ == "__main__":
    start_consumer()
