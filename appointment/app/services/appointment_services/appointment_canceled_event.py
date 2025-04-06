import pika
import json



def appointment_canceled_event(cancellation_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='cancellationQueue')

    message = cancellation_data.json()  # Assuming cancellation_data is a Pydantic model or similar

    channel.basic_publish(exchange='', routing_key='cancellationQueue', body=message)

    print(" [x] Sent 'appointmentCanceled' event")
    connection.close()