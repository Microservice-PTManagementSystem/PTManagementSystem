import json
from appointment.app.services.slot_services.slot_reserved_event import slot_reserved_event
from appointment.app.services.slot_services.slot_confirmed_event import slot_confirmed_event
from appointment.app.services.slot_services.slot_released_event import slot_released_event
from appointment.app.services.appointment_services.appointment_completed_event import appointment_completed_event
from appointment.app.services.appointment_services.appointment_rescheduled_event import appointment_rescheduled_event
from appointment.app.services.appointment_services.appointment_canceled_event import appointment_canceled_event

def make_reservation(data):
    print(f"DATA:  {data}")
    print(f"DATA TYPE:  {type(data)}")


    slot_reserved_event(data)

    slot_confirmed_event(data)


    return {"status": "success", "data": data}
