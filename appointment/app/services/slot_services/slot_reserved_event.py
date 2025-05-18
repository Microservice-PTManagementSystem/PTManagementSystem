import pika
import json
import copy
from datetime import datetime
from services.db_services.mongodb_service import get_collection
import threading
import time
from services.appointment_services.appointment_failed_event import appointment_failed_event

def datetime_serializer(obj):
    if isinstance(obj, datetime):
        return obj.isoformat() 
    raise TypeError("Type not serializable")

def reservation_timeout_checker(slot_id):
    time.sleep(10) 
    collection = get_collection("AppointmentDB")
    appointment = collection.find_one({"slot_id": slot_id})

    if appointment and appointment.get("status") != "active":
        
        print(f"Payment not received in time for slot {slot_id}. Marking as not_paid.", flush=True)
    
        collection.update_one({"slot_id": slot_id}, {"$set": {"status": "failed"}})
     
        SlotDB_collection = get_collection("SlotDB")
        SlotDB_collection.update_one({"slot_id": slot_id}, {"$set": {"slot_status": "released"}})
        appointment_failed_event({"user_id" : appointment["user_id"]})


def slot_reserved_event(reservation_data):
    connection = pika.BlockingConnection(pika.ConnectionParameters(host='rabbitmq'))
    channel = connection.channel()
    channel.queue_declare(queue='reservationQueue')

    reservation_data["reservation_time"] = datetime.utcnow()

    SlotDB_collection = get_collection("SlotDB")
    SlotDB_collection.update_one(
        {"slot_id": reservation_data["slot_id"]},
        {"$set": {"slot_status": "reserved"}}
    )

    collection = get_collection("AppointmentDB")
    collection.insert_one(reservation_data.copy())

    message = json.dumps(reservation_data, default=datetime_serializer)
    channel.basic_publish(exchange='', routing_key='reservationQueue', body=message)
    print(" [x] Sent 'reservationMade' event")
    connection.close()

    # Start a timeout watcher thread
    threading.Thread(target=reservation_timeout_checker, args=(reservation_data["slot_id"],), daemon=True).start()