from fastapi import APIRouter
from models.notification_model import notification_model
from controllers.chat_controller import send_notification1

router = APIRouter(prefix="/send_notification", tags=["notification"])


@router.post("/send_notification")
async def send_notification(notification_data: notification_model):
    response = send_notification1(notification_data)
    return {"response": response}
