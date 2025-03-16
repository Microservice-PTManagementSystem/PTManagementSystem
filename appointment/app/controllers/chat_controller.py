import json

from services.publish_reservation_event import publish_reservation_event

def make_reservation(data):
    
    print(f"DATAAAAAAAAAA   {data}")

    print(f"DATAAAAAAAAAA   {type(data)}")
 
    publish_reservation_event(data)
 
    return data


"""def make_reservation(data):
    return data"""
