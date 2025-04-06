import pika
import json

def slot_released_event(release_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='releaseQueue')

    message = release_data.json() 

    channel.basic_publish(exchange='', routing_key='releaseQueue', body=message)

    print(" [x] Sent 'slotReleased' event")
    connection.close()