from pymongo import MongoClient
import os

def get_mongo_client():
    connection_string = os.getenv("MongoDB__ConnectionString")
    return MongoClient(connection_string)
#mongodb+srv://dorukyelken:XBBoy9I7ZoWnp887@cluster0.hupvg8a.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0
#mongodb+srv://eryilmazmuhammet07:0irCC60yUYilGmzC@appointmentdb.wqol0nu.mongodb.net/
def get_appointment_db_collection():
    client = get_mongo_client()
    appointment_dbname = os.getenv("MongoDB__AppointmentDBName")
    db = client[appointment_dbname]
    collection = db["appointment_collection"]
    return collection

def get_slot_db_collection():
    client = get_mongo_client()
    slot_dbname = os.getenv("MongoDB__SlotDBName")
    db = client[slot_dbname]
    collection = db["slot_collection"]
    return collection

def get_collection(usecase):
    mongo_service = {
        "AppointmentDB": get_appointment_db_collection(),
        "SlotDB": get_slot_db_collection()
    }
    return mongo_service[usecase]
