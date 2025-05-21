import pika
import json

def appointment_completed_event(appointment_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='appointmentCompleted')

    message = json.dumps(appointment_data, default=str)
    print(type(message))
    print(f"Appointment Completed Message:::::  ", message)
    channel.basic_publish(exchange='', routing_key='appointmentCompleted', body=message)

    print(" [x] Sent 'appointmentCompleted' event")
    connection.close()
