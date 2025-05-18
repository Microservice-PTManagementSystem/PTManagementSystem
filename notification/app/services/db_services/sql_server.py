import os
import pyodbc
from dotenv import load_dotenv

load_dotenv()

def get_sql_connection():
    connection_string = os.getenv("SQLSERVER__ConnectionString")
    return pyodbc.connect(connection_string)

def insert_notification(user_id, message):
    conn = get_sql_connection()
    cursor = conn.cursor()

    cursor.execute("INSERT INTO Notifications (UserId, Message) VALUES (?, ?)", (user_id, message))
    conn.commit()
    conn.close()

def get_all_notifications():
    conn = get_sql_connection()
    cursor = conn.cursor()

    cursor.execute("SELECT * FROM Notifications")
    results = cursor.fetchall()
    conn.close()
    return results

def get_notifications_by_user_id(user_id):
    conn = get_sql_connection()
    cursor = conn.cursor()

    cursor.execute("SELECT * FROM Notifications WHERE UserId = ?", (user_id,))
    results = cursor.fetchall()
    conn.close()
    return results

