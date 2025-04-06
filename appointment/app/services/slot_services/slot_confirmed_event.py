import pika
import json


def slot_confirmed_event(confirmation_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='confirmationQueue')

    message = confirmation_data.json()  # Assuming confirmation_data is a Pydantic model or similar

    channel.basic_publish(exchange='', routing_key='confirmationQueue', body=message)

    print(" [x] Sent 'slotConfirmed' event")
    connection.close()