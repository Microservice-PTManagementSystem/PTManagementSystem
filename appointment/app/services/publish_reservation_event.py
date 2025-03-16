import pika
import json
 

def publish_reservation_event(reservation_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
 
    channel.queue_declare(queue='reservationQueue')  
 
    message = json.dumps(reservation_data)
    channel.basic_publish(exchange='', routing_key='reservationQueue', body=message)
 
    print(" [x] Sent 'reservationMade' event")
    connection.close()