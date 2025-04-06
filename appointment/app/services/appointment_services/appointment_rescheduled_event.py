import pika
import json

def appointment_rescheduled_event(reschedule_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='rescheduleQueue')

    message = reschedule_data.json()  # Assuming reschedule_data is a Pydantic model or similar

    channel.basic_publish(exchange='', routing_key='rescheduleQueue', body=message)

    print(" [x] Sent 'appointmentRescheduled' event")
    connection.close()