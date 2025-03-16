import pika
import json
 
connection = pika.BlockingConnection(pika.ConnectionParameters('rabbitmq'))
channel = connection.channel()
 
channel.queue_declare(queue='events_queue')
 
event = {
    "event_type": "user_login",
    "user_id": 123,
    "timestamp": "2025-03-16T12:00:00"
}
 
channel.basic_publish(
    exchange='',
    routing_key='events_queue',
    body=json.dumps(event)
)
 
print("Event gönderildi:", event)
 
connection.close()