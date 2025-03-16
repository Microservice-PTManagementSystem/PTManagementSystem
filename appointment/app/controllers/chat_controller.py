import json

from services.publish_reservation_event import publish_reservation_event




def make_reservation(data):
   
    reservation_response = str(data) 
 
    publish_reservation_event(reservation_response)
 
    return reservation_response
  