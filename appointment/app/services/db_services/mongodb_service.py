from pymongo import MongoClient

def get_mongo_client():
    return MongoClient("mongodb+srv://eryilmazmuhammet07:0irCC60yUYilGmzC@appointmentdb.wqol0nu.mongodb.net/")

def get_appointment_db_collection():
    client = get_mongo_client()
    db = client["AppointmentDB"]
    collection = db["appointment_collection"]
    return collection

def get_slot_db_collection():
    client = get_mongo_client()
    db = client["SlotDB"]
    collection = db["slot_collection"]
    return collection

def get_collection(usecase):
    mongo_service = {
        "AppointmentDB": get_appointment_db_collection(),
        "SlotDB": get_slot_db_collection()
    }
    return mongo_service[usecase]
