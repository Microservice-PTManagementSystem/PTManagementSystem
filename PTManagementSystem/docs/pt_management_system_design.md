# PT Management System - System Design Document

## Implementation Approach

### Technology Stack
- FastAPI (Python web framework)
- RabbitMQ (Message broker)
- Docker & Docker Compose (Containerization)
- Hexagonal Architecture pattern

### Key Components
1. **Appointment Service**
   - Manages appointment slots and their states (available, reserved, confirmed, completed, cancelled)
   - Publishes appointment-related events
   - RESTful API endpoints for slot management
   - Event handlers for payment-related events

2. **Notification Service**
   - Handles sending notifications to users and trainers
   - Supports multiple notification channels (email, etc.)
   - Event-driven architecture using RabbitMQ

3. **Message Broker (RabbitMQ)**
   - Enables asynchronous communication between services
   - Handles event publishing and subscription
   - Ensures reliable message delivery

### Implementation Considerations
1. **Event-Driven Architecture**
   - Services communicate through events
   - Loose coupling between services
   - Asynchronous processing

2. **Hexagonal Architecture**
   - Clear separation of concerns
   - Domain-driven design
   - Ports and adapters pattern

3. **Error Handling**
   - Retry mechanisms for failed notifications
   - Graceful degradation
   - Dead letter queues for failed messages

## API Specifications

### Appointment Service API

#### Endpoints

1. **POST /api/v1/slots**
   - Create new appointment slot
   - Request: `{"trainer_id": str, "start_time": datetime, "duration": int}`
   - Response: `{"slot_id": str, "status": "available"}`

2. **GET /api/v1/slots**
   - List available slots
   - Query params: trainer_id, date
   - Response: `{"slots": [{"slot_id": str, "trainer_id": str, "start_time": datetime, "status": str}]}`

3. **POST /api/v1/appointments**
   - Reserve a slot
   - Request: `{"slot_id": str, "user_id": str}`
   - Response: `{"appointment_id": str, "status": "reserved"}`

4. **PATCH /api/v1/appointments/{appointment_id}**
   - Update appointment status
   - Request: `{"status": str}`
   - Response: `{"appointment_id": str, "status": str}`

### Events

#### Published Events
1. `SlotReservedEvent`
2. `SlotConfirmedEvent`
3. `SlotReleasedEvent`
4. `AppointmentCompletedEvent`
5. `AppointmentRescheduledEvent`
6. `AppointmentCanceledEvent`

#### Consumed Events
1. `PaymentSuccessEvent`
2. `PaymentFailedEvent`

### Notification Service API

#### Endpoints
1. **POST /api/v1/notifications**
   - Send notification
   - Request: `{"recipient_id": str, "type": str, "message": str}`
   - Response: `{"notification_id": str, "status": "sent"}`

#### Events
1. `NotificationSentEvent`
2. `NotificationFailedEvent`

## Error Cases
1. Payment timeout
2. Notification delivery failure
3. Concurrent booking attempts
4. Invalid slot states
5. Service unavailability

## Scalability Considerations
1. Horizontal scaling of services
2. Message broker clustering
3. Load balancing
4. Caching strategies