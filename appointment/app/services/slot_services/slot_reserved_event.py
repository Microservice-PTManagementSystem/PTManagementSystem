import pika
import json
 
def slot_reserved_event(reservation_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
 
    channel.queue_declare(queue='reservationQueue')  

    
    message = json.loads(reservation_data)

    channel.basic_publish(exchange='', routing_key='reservationQueue', body=message)
 
    print(" [x] Sent 'reservationMade' event")
    connection.close()
