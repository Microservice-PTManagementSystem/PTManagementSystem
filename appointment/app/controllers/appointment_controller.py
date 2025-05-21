import json
import uuid
from  datetime import datetime , timedelta

from services.slot_services.slot_reserved_event import slot_reserved_event

from services.slot_services.slot_released_event import slot_released_event
from services.appointment_services.appointment_completed_event import appointment_completed_event
from services.appointment_services.appointment_rescheduled_event import appointment_rescheduled_event
from services.appointment_services.appointment_canceled_event import appointment_canceled_event
from services.db_services.mongodb_service import get_collection
def make_reservation(data):
    print(f"DATA:  {data}")
    print(f"DATA TYPE:  {type(data)}")
    """    collection = get_collection()
    collection.insert_one(data)"""

    collection = get_collection("SlotDB")

    result = collection.find_one({"slot_id" : data["slot_id"]})

    data["hourly_price"] = result["hourly_price"]
    slot_reserved_event(data)

    return {"successs": True}

def create_slot(data):
    collection = get_collection("SlotDB")
    current_day = data["start_date"]
    slots = []

    # Convert time strings to datetime.time objects
    start_hour = datetime.strptime(data["daily_working_start_hour"], "%H:%M").time()
    end_hour = datetime.strptime(data["daily_working_end_hour"], "%H:%M").time()

    while current_day <= data["end_date"]:
        current_time = datetime.combine(current_day, start_hour)
        end_time = datetime.combine(current_day, end_hour)

        while current_time < end_time:
            next_hour = current_time + timedelta(hours=1)

            # Store slot data with datetime for start and end times
            slot_data = {
                "trainer_id": data["trainer_id"],
                "slot_id": str(uuid.uuid4()),
                "slot_status": "released",
                "start_time": current_time,  # Save datetime object
                "end_time": next_hour,  # Save datetime object
                "hourly_price": data["hourly_price"]
            }

            # Insert slot into database
            collection.insert_one(slot_data)

            # Add created slots to the list
            slots.append(slot_data)

            # Move to the next hour
            current_time = next_hour

        # Move to the next day
        current_day += timedelta(days=1)

    return {"response": 200}


# Get Available Slots Function
def get_available_slots(data):
    collection = get_collection("SlotDB")

    # Convert start and end date to datetime at the start of the day (00:00:00)
    start_datetime = datetime.combine(data["start_date"], datetime.min.time())
    end_datetime = datetime.combine(data["end_date"], datetime.min.time())

    filter_criteria = {
        "trainer_id": data["trainer_id"],
        "slot_status": "released",
        "start_time": {"$gte": start_datetime},  # Compare with datetime objects
        "end_time": {"$lte": end_datetime}  # Compare with datetime objects
    }

    # Fetch available slots from the database
    slots = list(collection.find(filter_criteria))

    # Remove the _id field from each slot
    for slot in slots:
        if "_id" in slot:
            del slot["_id"]

    return slots

def cancel_appointment(data):
    print(f"CANCELED RESERVATION:  {data}")
    print(f"DATA TYPE:  {type(data)}")
    appointment_canceled_event(data)
    return {"data": data}

def get_appointments_with_user_id(data , usecase):
    appointment_collection = get_collection("AppointmentDB")
    results = appointment_collection.find({"user_id": data["user_id"], "status": usecase}, {"_id": 0})
    slots = list(results)  

    slot_collection = get_collection("SlotDB")

    for item in slots:
        slot_id = item["slot_id"]
        slot_detail = slot_collection.find_one({"slot_id": slot_id}, {"_id": 0})
        
        item["slot_detail"] = slot_detail
        
    return slots




