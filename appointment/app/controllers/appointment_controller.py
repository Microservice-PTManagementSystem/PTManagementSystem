import json

from appointment.app.services.slot_reserved_event import slot_reserved_event

def make_reservation(data):
    
    print(f"DATA:  {data}")

    print(f"DATA:  {type(data)}")
 
    slot_reserved_event(data)
 
    return data


"""def make_reservation(data):
    return data"""
