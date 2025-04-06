import pika
import json

def appointment_completed_event(appointment_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='completedQueue')

    message = appointment_data.json() 

    channel.basic_publish(exchange='', routing_key='completedQueue', body=message)

    print(" [x] Sent 'appointmentCompleted' event")
    connection.close()