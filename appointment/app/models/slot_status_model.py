class SlotStatus(str, Enum):
    AVAILABLE = "available"
    RESERVED = "reserved"
    CONFIRMED = "confirmed"