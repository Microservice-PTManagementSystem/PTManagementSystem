import pika
import json

def appointment_failed_event(appointment_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='appointmentFailed')

    message = json.dumps(appointment_data, default=str)
    print(type(message))
    print(f"Appointment Failed Message:::::  ", message)
    channel.basic_publish(exchange='', routing_key='appointmentFailed', body=message)

    print(" [x] Sent 'appointmentFailed' event")
    connection.close()