import json
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

    slot_reserved_event(data)




    return {"status": "success", "data": data}

def create_slot(data):
    collection = get_collection("SlotDB")
    mongo_insert_data = data.copy()
    collection.insert_one(mongo_insert_data)
    return {"status": "success", "data": data}
