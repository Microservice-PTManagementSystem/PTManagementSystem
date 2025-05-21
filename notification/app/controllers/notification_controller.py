import json

from app.services.db_services.sql_server import get_notifications_by_user_id

def send_notification1(data):
    return data


def get_user_notifications(user_id: str):
    notifications = get_notifications_by_user_id(user_id)
    return notifications
